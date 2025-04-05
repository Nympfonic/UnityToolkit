using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityToolkit.Structures.EventBus;

/// <summary>
/// Used to initialize event buses automatically based on the target assembly.
/// </summary>
public class EventBusInitializer(Assembly assembly)
{
	private readonly IReadOnlyList<Type> _eventTypes = GetEventTypesFromAssembly(assembly);
	private Type[] _eventBusTypes;
	private MethodInfo[] _eventBusClearMethods;
	
	/// <summary>
	/// Initializes all event buses based on the types in <see cref="_eventTypes"/>.
	/// </summary>
	public void Initialize()
	{
		if (_eventBusTypes != null)
		{
			return;
		}
		
		int count = _eventTypes.Count;
		var eventBusTypes = new Type[count];
		var eventBusClearMethods = new MethodInfo[count];
		Type eventBusType = typeof(EventBus<>);
		
		var index = 0;
		foreach (Type eventType in _eventTypes)
		{
			Type genericType = eventBusType.MakeGenericType(eventType);
			eventBusTypes[index] = genericType;
			MethodInfo clearMethod = AccessTools.Method(genericType, "Clear");
			eventBusClearMethods[index] = clearMethod;
			index++;
		}
		
		_eventBusTypes = eventBusTypes;
		_eventBusClearMethods = eventBusClearMethods;
	}
	
	/// <summary>
	/// Calls the <see cref="EventBus{T}.Clear"/> method on all buses in this <see cref="EventBusInitializer"/> instance.
	/// </summary>
	public void ClearAllBuses()
	{
		if (_eventBusTypes == null)
		{
			throw new InvalidOperationException("EventBusInitializer has not been initialized.");
		}
		
		foreach (MethodInfo clearMethod in _eventBusClearMethods)
		{
			clearMethod.Invoke(null, null);
		}
	}
	
	/// <summary>
	/// Gets a list of all events in the assembly that implement the <see cref="IEvent"/> interface.
	/// </summary>
	private static IReadOnlyList<Type> GetEventTypesFromAssembly(Assembly assembly)
	{
		var list = new List<Type>();
		Type eventType = typeof(IEvent);
		Type[] types = assembly.GetTypes();
		
		foreach (Type type in types)
		{
			if (type.IsAssignableFrom(eventType))
			{
				list.Add(type);
			}
		}
		
		return list;
	}
}