﻿//#define RECHECK
using BSAsharp;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using TaleOfTwoWastelands.ProgressTypes;

namespace TaleOfTwoWastelands.Patching
{
    class BuildPatchDB
    {
        const string SOURCE_DIR = "BuildDB";
        const string BUILD_DIR = "Checksums";

        //Shameless code duplication. So sue me.
        public static void Build()
        {
#if RECHECK
            bool keepGoing = true;
#endif

            Directory.CreateDirectory(BUILD_DIR);

            var dirTTWMain = Path.Combine(SOURCE_DIR, Installer.MainDir);
            var dirTTWOptional = Path.Combine(SOURCE_DIR, Installer.OptDir);

            var bethKey = Installer.GetBethKey();

            var fo3Key = bethKey.CreateSubKey("Fallout3");
            var Fallout3Path = fo3Key.GetValue("Installed Path", "").ToString();
            var dirFO3Data = Path.Combine(Fallout3Path, "Data");

//            foreach (var ESM in Installer.CheckedESMs)
//            {
//                var fixPath = Path.Combine(BUILD_DIR, ESM + ".pat");
//                if (!File.Exists(fixPath))
//                {
//                    var dataESM = Path.Combine(dirFO3Data, ESM);
//                    var ttwESM = Path.Combine(dirTTWMain, ESM);

//                    //var fvOriginal = FileValidation.FromFile(dataESM);

//                    var patch = PatchInfo.FromFile("", dataESM, ttwESM);

//                    using (var fixStream = File.OpenWrite(fixPath))
//                        Serializer.Serialize(fixStream, patch);
//                }
//#if RECHECK
//                using (var fixStream = File.OpenRead(fixPath))
//                {
//                    var p = Serializer.Deserialize<PatchInfo>(fixStream);
//                    if (keepGoing)
//                        Debugger.Break();
//                }
//#endif
//            }

            var progressLog = new Progress<string>(s => Debug.Write(s));
            var progressUIMinor = new Progress<OperationProgress>();
            var token = new CancellationTokenSource().Token;
            foreach (var kvpBsa in Installer.BuildableBSAs)
            {
                var inBsaName = kvpBsa.Key;
                var outBsaName = kvpBsa.Value;

                string outBSAFile = Path.ChangeExtension(outBsaName, ".bsa");
                string outBSAPath = Path.Combine(dirTTWMain, outBSAFile);

                string inBSAFile = Path.ChangeExtension(inBsaName, ".bsa");
                string inBSAPath = Path.Combine(dirFO3Data, inBSAFile);

                IDictionary<string, string> renameDict = null;
                { //comment this out if you don't need it, but set renameDict
                    var renDict = ReadOldDict(outBsaName, "RenameFiles.dict");
                    if (renDict != null)
                    {
                        renameDict = new Dictionary<string, string>(renDict);
                        var newRenPath = Path.Combine(BUILD_DIR, Path.ChangeExtension(outBsaName, ".ren"));
                        if (!File.Exists(newRenPath))
                            using (var stream = File.OpenWrite(newRenPath))
                                Serializer.Serialize(stream, renameDict);
                    }
                    else
                    {
                        renameDict = new Dictionary<string, string>();
                    }
                }
                Debug.Assert(renameDict != null);

                var patPath = Path.Combine(BUILD_DIR, Path.ChangeExtension(outBsaName, ".pat"));
                if (File.Exists(patPath))
                    continue;

                using (var inBSA = new BSAWrapper(inBSAPath))
                using (var outBSA = new BSAWrapper(outBSAPath))
                {
                    {
                        var renameGroup = from folder in inBSA
                                          from file in folder
                                          join kvp in renameDict on file.Filename equals kvp.Value
                                          let a = new { folder, file, kvp }
                                          select a;

                        var renameCopies = from g in renameGroup
                                           let newFilename = g.kvp.Key
                                           let newDirectory = Path.GetDirectoryName(newFilename)
                                           let a = new { g.folder, g.file, newFilename }
                                           group a by newDirectory into outs
                                           select outs;

                        var newBsaFolders = from g in renameCopies
                                            let folderAdded = inBSA.Add(new BSAFolder(g.Key))
                                            select g;
                        newBsaFolders.ToList();

                        var renameFixes = from g in newBsaFolders
                                          from a in g
                                          join newFolder in inBSA on g.Key equals newFolder.Path
                                          let newFile = a.file.DeepCopy(g.Key, Path.GetFileName(a.newFilename))
                                          let addedFile = newFolder.Add(newFile)
                                          let cleanedDict = renameDict.Remove(a.newFilename)
                                          select new { a.folder, a.file, newFolder, newFile, a.newFilename };
                        renameFixes.ToList(); // execute query
                    }

                    var oldFiles = inBSA.SelectMany(folder => folder).ToList();
                    var newFiles = outBSA.SelectMany(folder => folder).ToList();

                    var oldChkDict = FileValidation.FromBSA(inBSA);
                    var newChkDict = FileValidation.FromBSA(outBSA);

                    var joinedPatches = from patKvp in newChkDict
                                        join oldKvp in oldChkDict on patKvp.Key equals oldKvp.Key into foundOld
                                        join oldBsaFile in oldFiles on patKvp.Key equals oldBsaFile.Filename
                                        join newBsaFile in newFiles on patKvp.Key equals newBsaFile.Filename
                                        select new
                                        {
                                            oldBsaFile,
                                            newBsaFile,
                                            file = patKvp.Key,
                                            patch = patKvp.Value,
                                            oldChk = foundOld.SingleOrDefault()
                                        };

                    var checkDict = new Dictionary<string, PatchInfo>();
                    foreach (var join in joinedPatches)
                    {
                        if (string.IsNullOrEmpty(join.oldChk.Key))
                            Debug.Fail("File not found: " + join.file);

                        var oldFilename = join.oldBsaFile.Filename;
                        if (oldFilename.StartsWith(BSADiff.voicePrefix))
                        {
                            checkDict.Add(join.file, new PatchInfo());
                            continue;
                        }

                        var oldChkLazy = join.oldChk.Value;
                        var newChkLazy = join.patch;

                        var oldChk = oldChkLazy.Value;
                        var newChk = newChkLazy.Value;

                        PatchInfo patchInfo;
                        if (!newChk.Equals(oldChk))
                        {
                            var antiqueOldChk = GetChecksum(join.oldBsaFile.GetContents(true));
                            var antiqueNewChk = GetChecksum(join.newBsaFile.GetContents(true));

                            patchInfo = PatchInfo.FromFileChecksum(outBsaName, oldFilename, antiqueOldChk, antiqueNewChk, newChk);
                            Debug.Assert(patchInfo.Data != null);
                        }
                        else
                            //without this, we will generate sparse (patch-only) fixups
                            patchInfo = new PatchInfo { Metadata = newChk };
                        checkDict.Add(join.file, patchInfo);
                    }

#if RECHECK
                    var ancCheckDict = ReadOldDict(outBsaName, "Checksums.dict");
                    Trace.Assert(ancCheckDict.Keys.SequenceEqual(checkDict.Keys.OrderBy(key => key)));
#endif

                    using (var stream = File.OpenWrite(patPath))
                        Serializer.Serialize(stream, checkDict);
                }
            }
        }

        private static IDictionary<string, string> ReadOldDict(string outFilename, string dictName)
        {
            var dictPath = Path.Combine(BSADiff.PatchDir, outFilename, dictName);
            if (!File.Exists(dictPath))
                return null;
            using (var stream = File.OpenRead(dictPath))
                return (IDictionary<string, string>)new BinaryFormatter().Deserialize(stream);
        }

        private static string GetChecksum(byte[] buf)
        {
            MD5 fileHash = MD5.Create();
            return BitConverter.ToString(fileHash.ComputeHash(buf)).Replace("-", "");
        }
    }
}