using BepInEx;
using BepInEx.Logging;
using Cysharp.Text;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityToolkit.Utils;
using VContainer;
using VContainer.Unity;

namespace UnityToolkit;

/// <summary>
/// Entry point for UnityToolkit mod framework.
/// </summary>
[BepInPlugin("com.Arys.UnityToolkit", "Unity Toolkit", "1.3.0")]
[BepInDependency("com.SPT.core", MinimumDependencyVersion: "3.11.0")]
public class UnityToolkitPlugin : BaseUnityPlugin
{
	private void Awake()
	{
		var currentAssembly = Assembly.GetExecutingAssembly();
		string directory = Path.GetDirectoryName(currentAssembly.Location)!;
		
		AssemblyLoader.LoadAssemblies(directory, Logger);
		
		new ModulePatchManager(currentAssembly).EnableAllPatches();
		
		TestZStringLog(Logger);
		TestVContainer();
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
	private static void TestVContainer()
	{
		var scope = new GameObject().AddComponent<TestLifetimeScope>();
		scope.autoRun = true;
		scope.Build();
	}
}

internal class TestLifetimeScope : LifetimeScope
{
	protected override void Configure(IContainerBuilder builder)
	{
		builder.Register(_ => BepInEx.Logging.Logger.CreateLogSource("UnityToolkit-Test"), Lifetime.Singleton);
		builder.RegisterEntryPoint<HelloWorldService>();
	}
}

[method: Inject]
internal class HelloWorldService(ManualLogSource logger) : IStartable
{
	void IStartable.Start()
	{
		logger.LogInfo("Hello world! This message means that VContainer was successfully initialized.");
	}
}