using CustomPlayerLoopSystem;
using HarmonyLib;
using JetBrains.Annotations;
using SPT.Reflection.Patching;
using System;
using System.Reflection;
using UnityEngine.LowLevel;
#if DEBUG
using BepInEx.Logging;
using VContainer;
using VContainer.Unity;
#endif

namespace UnityToolkit.Patches;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// This patch is required to inject UniTask and VContainer's PlayerLoopSystems after EFT has injected its custom PlayerLoopSystems.
/// </summary>
[UsedImplicitly]
public class InjectPlayerLoopSystems : ModulePatch
{
	protected override MethodBase GetTargetMethod()
	{
		return AccessTools.Method(typeof(CustomPlayerLoopSystemsInjector),
			nameof(CustomPlayerLoopSystemsInjector.Injection));
	}

	[PatchPostfix]
	private static void PatchPostfix()
	{
		InjectUniTaskPlayerLoopSystems();
		InjectVContainerPlayerLoopSystems();
	}

	private static void InjectUniTaskPlayerLoopSystems()
	{
		PlayerLoopSystem playerLoop = PlayerLoop.GetCurrentPlayerLoop();
		Cysharp.Threading.Tasks.PlayerLoopHelper.Initialize(ref playerLoop);
	}

	private static void InjectVContainerPlayerLoopSystems()
	{
		Type playerLoopHelperType = AccessTools.TypeByName(
			"VContainer.Unity.PlayerLoopHelper, VContainer, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
		AccessTools.Method(playerLoopHelperType, "EnsureInitialized").Invoke(null, null);
#if DEBUG
		TestVContainer();
#endif
	}

#if DEBUG
	private static void TestVContainer()
	{
		var builder = new ContainerBuilder();
		builder.RegisterInstance(BepInEx.Logging.Logger.CreateLogSource("VContainer"));
		builder.RegisterEntryPoint<HelloWorldService>();
		builder.Build();
	}
#endif
}

#if DEBUG
public class HelloWorldService : IStartable
{
	private readonly ManualLogSource _logger;

	public HelloWorldService(ManualLogSource logger)
	{
		_logger = logger;
	}

	public void Start()
	{
		_logger.LogInfo("Hello world! This message means that VContainer was successfully initialized.");
	}
}
#endif