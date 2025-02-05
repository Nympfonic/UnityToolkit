using JetBrains.Annotations;
using System;

namespace UnityToolkit.Structures.EventBus;

internal interface IEventBinding<T>
{
	[NotNull] public Action<T> OnEvent { get; set; }
	[NotNull] public Action OnEventNoArgs { get; set; }
}

/// <summary>
/// Used to bind events to the event bus.
/// </summary>
/// <typeparam name="T">The type to pass as an argument into events with arguments.</typeparam>
[UsedImplicitly]
public sealed class EventBinding<T> : IEventBinding<T>
{
	private Action<T> _onEvent = _ => { };
	private Action _onEventNoArgs = () => { };
	
	/// <summary>
	/// The actions with T data for the event bus to invoke.
	/// </summary>
	public Action<T> OnEvent
	{
		get => _onEvent;
		set => _onEvent = value;
	}
	
	/// <summary>
	/// The actions without arguments for the event bus to invoke.
	/// </summary>
	public Action OnEventNoArgs
	{
		get => _onEventNoArgs;
		set => _onEventNoArgs = value;
	}
	
	/// <summary>
	/// Constructor for event binding with T data.
	/// </summary>
	/// <param name="action">The action to bind.</param>
	public EventBinding(Action<T> action) => _onEvent = action;
	
	/// <summary>
	/// Constructor for event binding without arguments.
	/// </summary>
	/// <param name="action">The action to bind.</param>
	public EventBinding(Action action) => _onEventNoArgs = action;
	
	/// <summary>
	/// Adds a new action with T data to the event binding.
	/// </summary>
	/// <param name="action">The action to add.</param>
	public void Add(Action<T> action) => _onEvent += action;
	
	/// <summary>
	/// Removes an action with T data from the event binding.
	/// </summary>
	/// <param name="action">The action to remove.</param>
	public void Remove(Action<T> action) => _onEvent -= action;
	
	/// <summary>
	/// Adds a new action without arguments to the event binding.
	/// </summary>
	/// <param name="action">The action to add.</param>
	public void Add(Action action) => _onEventNoArgs += action;
	
	/// <summary>
	/// Removes an action without arguments from the event binding.
	/// </summary>
	/// <param name="action">The action to remove.</param>
	public void Remove(Action action) => _onEventNoArgs -= action;
}