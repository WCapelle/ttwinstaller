﻿using System.Threading;

namespace TaleOfTwoWastelands.Progress
{
    public interface IInstallStatus : IInstallStatusUpdate
	{
		void Finish();
	}
}