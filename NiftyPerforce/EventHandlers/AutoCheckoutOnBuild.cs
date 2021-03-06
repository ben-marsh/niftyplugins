// Copyright (C) 2006-2010 Jim Tilander. See COPYING for and README for more details.
using System;
using System.IO;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using System.Collections.Generic;

namespace Aurora
{
	namespace NiftyPerforce
	{
		class AutoCheckoutOnBuild : PreCommandFeature
		{
			public AutoCheckoutOnBuild(Plugin plugin)
				: base(plugin, "AutoCheckoutOnBuild", "Automatically checks out the source when building")
			{
				if(!Singleton<Config>.Instance.autoCheckoutOnBuild)
				{
					return;
				}

				Log.Info("Adding handlers for automatically checking out dirty files when you hit build");
				
				RegisterHandler("Build.BuildSolution", OnCheckoutModifiedSource);
				RegisterHandler("Build.Compile", OnCheckoutModifiedSource);
				RegisterHandler("Build.BuildOnlyProject", OnCheckoutModifiedSource);
				RegisterHandler("Debug.Start", OnCheckoutModifiedSource);
				RegisterHandler("ClassViewContextMenus.ClassViewProject.Build", OnCheckoutModifiedSource);
				RegisterHandler("ClassViewContextMenus.ClassViewProject.Rebuild", OnCheckoutModifiedSource);
				RegisterHandler("ClassViewContextMenus.ClassViewProject.Debug.Startnewinstance", OnCheckoutModifiedSource);
			}

			private void OnCheckoutModifiedSource(string Guid, int ID, object CustomIn, object CustomOut, ref bool CancelDefault)
			{
				foreach(Document doc in mPlugin.App.Documents)
				{
					if(!doc.Saved && doc.ReadOnly)
						P4Operations.EditFileImmediate(mPlugin.OutputPane, doc.FullName);
				}
			}
		}
	}
}
