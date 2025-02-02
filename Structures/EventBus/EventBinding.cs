using JetBrains.Annotations;
using System;

namespace UnityToolkit.Structures.EventBus;

internal interface IEventBinding<T>
{
	[NotNull] public Action<T> OnEvent { get; set; }
	[NotNull] public Action OnEventNoArgs { get; set; }
}

[UsedImplicitly]
public sealed class EventBinding<T> : IEventBinding<T>
{
	private Action<T> _onEvent = _ => { };
	private Action _onEventNoArgs = () => { };
	
	public Action<T> OnEvent
	{
		get => _onEvent;
		set => _onEvent = value;
	}
	
	public Action OnEventNoArgs
	{
		get => _onEventNoArgs;
		set => _onEventNoArgs = value;
	}
	
	public EventBinding(Action<T> action) => _onEvent = action;
	public EventBinding(Action action) => _onEventNoArgs = action;
	
	public void Add(Action<T> action) => _onEvent += action;
	public void Remove(Action<T> action) => _onEvent -= action;
	
	public void Add(Action action) => _onEventNoArgs += action;
	public void Remove(Action action) => _onEventNoArgs -= action;
}