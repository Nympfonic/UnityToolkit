﻿using BepInEx;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityToolkit.Utils;

namespace UnityToolkit;

/// <summary>
/// Entry point for UnityToolkit mod framework.
/// </summary>
[BepInPlugin("com.Arys.UnityToolkit", "Unity Toolkit", "1.2.0")]
public class UnityToolkitPlugin : BaseUnityPlugin
{
	private readonly List<string> _assemblyFileNames =
	[
		"UniTask.dll",
		"UniTask.Linq.dll",
		"UniTask.DOTween.dll",
		"UniTask.TextMeshPro.dll",
		"Unity.Collections.dll",
		// ZString has dependencies on newer versions of System.Runtime.CompilerServices.Unsafe.dll, System.Buffer.dll, and System.Memory.dll
		"ZString.dll"
	];
	
	[UsedImplicitly] private readonly List<Assembly> _assembliesLoaded = [];
	
	private void Awake()
	{
		var currentAssembly = Assembly.GetExecutingAssembly();
		string directory = Path.GetDirectoryName(currentAssembly.Location)!;
		
		// Load assemblies into memory
		foreach (string assemblyFileName in _assemblyFileNames)
		{
			string assemblyPath = Path.Combine(directory, assemblyFileName);
			var assemblyName = AssemblyName.GetAssemblyName(assemblyPath);
			Assembly assembly = Assembly.Load(assemblyName);
			_assembliesLoaded.Add(assembly);
		}
		
		new ModulePatchManager(currentAssembly).EnableAllPatches();
	}
}