using JetBrains.Annotations;
using System.Collections.Generic;

namespace UnityToolkit.Structures.EventBus;

/// <summary>
/// Event types must derive from this interface.
/// </summary>
public interface IEvent;

/// <summary>
/// Non-generic event bus which invokes the corresponding generic event bus.
/// </summary>
/// <seealso cref="EventBus{T}"/>
[UsedImplicitly]
public static class EventBus
{
	public static void Raise<T>(T @event) where T : IEvent
	{
		EventBus<T>.Raise(@event);
	}
	
	public static void Register<T>(EventBinding<T> binding) where T : IEvent
	{
		EventBus<T>.Register(binding);
	}
	
	public static void Deregister<T>(EventBinding<T> binding) where T : IEvent
	{
		EventBus<T>.Deregister(binding);
	}
}

/// <summary>
/// A decoupled publisher and subscriber system.
/// </summary>
/// <remarks>Deregistration of event bindings takes precedent over registration.</remarks>
[UsedImplicitly]
public static class EventBus<T> where T : IEvent
{
	private static readonly Queue<IEventBinding<T>> _bindingsToAdd = [];
	private static readonly Queue<IEventBinding<T>> _bindingsToRemove = [];
	private static readonly HashSet<IEventBinding<T>> _bindings = [];
	
	public static void Register(EventBinding<T> binding) => _bindingsToAdd.Enqueue(binding);
	public static void Deregister(EventBinding<T> binding) => _bindingsToRemove.Enqueue(binding);

	public static void Raise(T @event)
	{
		RemoveBindings();
		AddBindings();
		
		foreach (IEventBinding<T> binding in _bindings)
		{
			binding.OnEvent.Invoke(@event);
			binding.OnEventNoArgs.Invoke();
		}
	}

	public static void Clear()
	{
		_bindingsToAdd.Clear();
		_bindingsToRemove.Clear();
		_bindings.Clear();
	}
	
	private static void RemoveBindings()
	{
		while (_bindingsToRemove.Count > 0)
		{
			IEventBinding<T> binding = _bindingsToRemove.Dequeue();
			_bindings.Remove(binding);
		}
	}
	
	private static void AddBindings()
	{
		while (_bindingsToAdd.Count > 0)
		{
			IEventBinding<T> binding = _bindingsToAdd.Dequeue();
			_bindings.Add(binding);
		}
	}
}