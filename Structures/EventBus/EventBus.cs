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
	/// <summary>
	/// Raises an event.
	/// </summary>
	/// <param name="event">The event to raise.</param>
	/// <typeparam name="T">The event type.</typeparam>
	public static void Raise<T>(T @event) where T : IEvent
	{
		EventBus<T>.Raise(@event);
	}
	
	/// <summary>
	/// Registers an event binding.
	/// </summary>
	/// <param name="binding">The binding to register.</param>
	/// <typeparam name="T">The event type.</typeparam>
	public static void Register<T>(EventBinding<T> binding) where T : IEvent
	{
		EventBus<T>.Register(binding);
	}
	
	/// <summary>
	/// Deregisters an event binding.
	/// </summary>
	/// <param name="binding">The binding to deregister.</param>
	/// <typeparam name="T">The event type.</typeparam>
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
	
	/// <summary>
	/// Registers an event binding.
	/// </summary>
	/// <param name="binding">The binding to register.</param>
	public static void Register(EventBinding<T> binding) => _bindingsToAdd.Enqueue(binding);
	
	/// <summary>
	/// Deregisters an event binding.
	/// </summary>
	/// <param name="binding">The binding to deregister.</param>
	public static void Deregister(EventBinding<T> binding) => _bindingsToRemove.Enqueue(binding);
	
	/// <summary>
	/// Raises an event.
	/// </summary>
	/// <param name="event">The event to raise.</param>
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
	
	/// <summary>
	/// Clears all bindings.
	/// </summary>
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