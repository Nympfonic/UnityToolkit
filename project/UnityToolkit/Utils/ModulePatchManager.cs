using JetBrains.Annotations;
using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityToolkit.Utils;

/// <summary>
/// Disables the <see cref="ModulePatch"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DisablePatchAttribute : Attribute;

/// <summary>
/// Each new instance of <see cref="ModulePatchManager"/> will register non-disabled <see cref="ModulePatch"/>es depending on the current executing assembly.
/// </summary>
[UsedImplicitly]
public class ModulePatchManager
{
	private readonly List<ModulePatch> _patches = [];
	
	/// <summary>
	/// Creates an instance of the patch manager, targeting a particular assembly to get all of its <see cref="ModulePatch"/>es.
	/// </summary>
	/// <param name="targetAssembly">The assembly to look through for <see cref="ModulePatch"/>es.</param>
	public ModulePatchManager(Assembly targetAssembly)
	{
		foreach (Type type in targetAssembly.GetTypes())
		{
			if (type.BaseType == typeof(ModulePatch) &&
				type.GetCustomAttribute(typeof(DisablePatchAttribute)) == null)
			{
				_patches.Add((ModulePatch)Activator.CreateInstance(type));
			}
		}
	}
	
	/// <summary>
	/// Enables all patches stored within this instance of <see cref="ModulePatchManager"/>.
	/// </summary>
	public void EnableAllPatches()
	{
		foreach (ModulePatch patch in _patches)
		{
			patch.Enable();
		}
	}

	/// <summary>
	/// Disables all patches stored within this instance of <see cref="ModulePatchManager"/>.
	/// </summary>
	public void DisableAllPatches()
	{
		foreach (ModulePatch patch in _patches)
		{
			patch.Disable();
		}
	}

	/// <summary>
	/// Enables a specific patch of type T within this instance of <see cref="ModulePatchManager"/>.
	/// </summary>
	/// <typeparam name="T">Subclass of <see cref="ModulePatch"/></typeparam>
	/// <exception cref="ArgumentException">When T is not a subclass of <see cref="ModulePatch"/>.</exception>
	public void EnablePatch<T>() where T : ModulePatch
	{
		if (!typeof(T).IsSubclassOf(typeof(ModulePatch)))
		{
			throw new ArgumentException("Type " + typeof(T).FullName + " is not a subclass of ModulePatch");
		}
		
		foreach (ModulePatch patch in _patches)
		{
			if (patch.GetType() == typeof(T))
			{
				patch.Enable();
				return;
			}
		}
	}

	/// <summary>
	/// Disables a specific patch of type T within this instance of <see cref="ModulePatchManager"/>.
	/// </summary>
	/// <typeparam name="T"><inheritdoc cref="EnablePatch{T}"/></typeparam>
	/// <exception cref="ArgumentException"><inheritdoc cref="EnablePatch{T}"/></exception>
	public void DisablePatch<T>() where T : ModulePatch
	{
		if (!typeof(T).IsSubclassOf(typeof(ModulePatch)))
		{
			throw new ArgumentException("Type " + typeof(T).FullName + " is not a subclass of ModulePatch");
		}
		
		foreach (ModulePatch patch in _patches)
		{
			if (patch.GetType() == typeof(T))
			{
				patch.Disable();
				return;
			}
		}
	}
}