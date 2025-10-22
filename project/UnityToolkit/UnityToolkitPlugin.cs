using BepInEx;
using BepInEx.Logging;
using Cysharp.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityToolkit.Patches;
using UnityToolkit.Utils;
using ZLinq;

namespace UnityToolkit;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// Entry point for UnityToolkit library mod.
/// </summary>
[BepInPlugin("com.arys.unitytoolkit", "Unity Toolkit", PluginMetadata.VERSION)]
[BepInDependency("com.SPT.core", MinimumDependencyVersion: "4.0.0")]
public class UnityToolkitPlugin : BaseUnityPlugin
{
	private void Awake()
	{
		var currentAssembly = Assembly.GetExecutingAssembly();
		string directory = Path.GetDirectoryName(currentAssembly.Location)!;
		
		AssemblyLoader.LoadAssemblies(directory, Logger);
		
		new InjectPlayerLoopSystems().Enable();
		
		TestZStringLog(Logger);
		TestZLinq(Logger);
	}
	
	[Conditional("DEBUG")]
	private static void TestZStringLog(ManualLogSource logger)
	{
		using Utf8ValueStringBuilder sb = ZString.CreateUtf8StringBuilder();
		sb.AppendLine("This is a test string to verify ZString works.");
		sb.AppendFormat("{0} is the answer to life!", 42);
		logger.LogInfo(sb.ToString());
	}

	[Conditional("DEBUG")]
	private static void TestZLinq(ManualLogSource logger)
	{
		List<int> numbers = [0, 4, 2, 8, 9, 5, 1, 6, 7, 3, 5];
		int numberOfTimes5Appears = numbers.AsValueEnumerable().Where(i => i == 5).Count();
		logger.LogInfo($"Number of times 5 appears is {numberOfTimes5Appears}");
	}
}