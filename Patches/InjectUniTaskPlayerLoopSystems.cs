using CustomPlayerLoopSystem;
using Cysharp.Threading.Tasks;
using HarmonyLib;
using JetBrains.Annotations;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine.LowLevel;

namespace UnityToolkit.Patches;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// This patch is required to inject UniTask's PlayerLoopSystems after Tarkov has injected its custom PlayerLoopSystems.
/// </summary>
[UsedImplicitly]
public class InjectUniTaskPlayerLoopSystems : ModulePatch
{
	protected override MethodBase GetTargetMethod()
	{
		return AccessTools.Method(typeof(CustomPlayerLoopSystemsInjector),
			nameof(CustomPlayerLoopSystemsInjector.Injection));
	}

	[PatchPostfix]
	private static void PatchPostfix()
	{
		PlayerLoopSystem playerLoop = PlayerLoop.GetCurrentPlayerLoop();
		PlayerLoopHelper.Initialize(ref playerLoop);
	}
}