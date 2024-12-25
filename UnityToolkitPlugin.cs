using BepInEx;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine.LowLevel;

namespace UnityToolkit;

[BepInPlugin("com.Arys.UnityToolkit", "Unity Toolkit", "1.1.0")]
public class UnityToolkitPlugin : BaseUnityPlugin
{
	private readonly List<string> _assemblyFileNames =
	[
		"UniTask.dll",
		"UniTask.Linq.dll",
		"UniTask.DOTween.dll",
		"UniTask.TextMeshPro.dll",
		"Unity.Collections.dll"
	];
	
	[UsedImplicitly] private readonly List<Assembly> _assembliesLoaded = [];

	private void Awake()
	{
		string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

		// Load assemblies into memory
		foreach (string assemblyFileName in _assemblyFileNames)
		{
			string assemblyPath = Path.Combine(directory, assemblyFileName);
			var assemblyName = AssemblyName.GetAssemblyName(assemblyPath);
			Assembly assembly = Assembly.Load(assemblyName);
			_assembliesLoaded.Add(assembly);
		}
		
		PlayerLoopSystem playerLoop = PlayerLoop.GetCurrentPlayerLoop();
		PlayerLoopHelper.Initialize(ref playerLoop);
	}
}