﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TaleOfTwoWastelands.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TaleOfTwoWastelands.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} is easiest to install via a mod manager like {1}. Manual installation is possible but not suggested.
        ///
        ///Would like the installer to automatically build FOMODs?.
        /// </summary>
        internal static string BuildFOMODsPrompt {
            get {
                return ResourceManager.GetString("BuildFOMODsPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Build FOMODs?.
        /// </summary>
        internal static string BuildFOMODsQuestion {
            get {
                return ResourceManager.GetString("BuildFOMODsQuestion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fallout 3.
        /// </summary>
        internal static string Fallout3 {
            get {
                return ResourceManager.GetString("Fallout3", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fallout New Vegas.
        /// </summary>
        internal static string FalloutNewVegas {
            get {
                return ResourceManager.GetString("FalloutNewVegas", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} has been installed successfully..
        /// </summary>
        internal static string InstalledSuccessfully {
            get {
                return ResourceManager.GetString("InstalledSuccessfully", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} must be run as Administrator..
        /// </summary>
        internal static string MustBeElevated {
            get {
                return ResourceManager.GetString("MustBeElevated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to New Vegas Script Extender (NVSE) was not found, but is required to play A Tale of Two Wastelands.
        ///
        ///Would you like to install NVSE?.
        /// </summary>
        internal static string NVSE_InstallPrompt {
            get {
                return ResourceManager.GetString("NVSE_InstallPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to FOMM.
        /// </summary>
        internal static string SuggestedModManager {
            get {
                return ResourceManager.GetString("SuggestedModManager", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tale of Two Wastelands.
        /// </summary>
        internal static string TTW {
            get {
                return ResourceManager.GetString("TTW", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An uncaught exception occurred and the program will now exit. Please submit a crash report with your installation log..
        /// </summary>
        internal static string UncaughtExceptionMessage {
            get {
                return ResourceManager.GetString("UncaughtExceptionMessage", resourceCulture);
            }
        }
    }
}
