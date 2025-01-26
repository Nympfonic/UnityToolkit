using JetBrains.Annotations;
using System;

namespace UnityToolkit.Structures;

/// <summary>
/// A closure struct, which can be managed manually compared to C#'s implicit closure.
/// </summary>
/// <typeparam name="TContext">The external data type to be passed into the closure.</typeparam>
[UsedImplicitly]
public struct Closure<TContext>
{
	private Delegate _delegate;
	private TContext _context;
	
	public Closure(Delegate @delegate, TContext context = default)
	{
		_delegate = @delegate;
		_context = context;
	}
	
	public void Invoke()
	{
		switch (_delegate)
		{
			case Action action:
				action();
				break;
			case Action<TContext> actionWithContext:
				actionWithContext(_context);
				break;
			default:
				throw new InvalidOperationException("Unsupported delegate type for Invoke.");
		}
	}
	
	public TResult Invoke<TResult>()
	{
		return _delegate switch
		{
			Func<TResult> func => func(),
			Func<TContext, TResult> funcWithContext => funcWithContext(_context),
			_ => throw new InvalidOperationException("Unsupported delegate type for Invoke<TResult>.")
		};
	}
	
	public void Set(Delegate @delegate, TContext context)
	{
		_delegate = @delegate;
		_context = context;
	}
	
	public static Closure<TContext> Create(Action action) => new(action);
	public static Closure<TContext> Create(Action<TContext> action, TContext context) => new(action, context);
	public static Closure<TContext> Create<TResult>(Func<TResult> func) => new(func);
	public static Closure<TContext> Create<TResult>(Func<TContext, TResult> func, TContext context) => new(func, context);
}