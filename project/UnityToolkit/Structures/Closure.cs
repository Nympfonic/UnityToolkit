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
	
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="delegate">The delegate to invoke.</param>
	/// <param name="context">The data needed for the delegate to execute.</param>
	public Closure(Delegate @delegate, TContext context = default)
	{
		_delegate = @delegate;
		_context = context;
	}
	
	/// <summary>
	/// Invokes the delegate.
	/// </summary>
	/// <exception cref="InvalidOperationException">
	/// When the provided delegate is not an <see cref="Action"/> or <see cref="Action{TContext}"/>.
	/// </exception>
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
	
	/// <summary>
	/// Invokes the delegate and returns a result.
	/// </summary>
	/// <typeparam name="TResult">The result type.</typeparam>
	/// <returns>The result from the delegate invocation.</returns>
	/// <exception cref="InvalidOperationException">
	/// When the provided delegate is not a <see cref="Func{TResult}"/> or <see cref="Func{TContext, TResult}"/>.
	/// </exception>
	public TResult Invoke<TResult>()
	{
		return _delegate switch
		{
			Func<TResult> func => func(),
			Func<TContext, TResult> funcWithContext => funcWithContext(_context),
			_ => throw new InvalidOperationException("Unsupported delegate type for Invoke<TResult>.")
		};
	}
	
	/// <summary>
	/// Sets the closure's delegate and context data.
	/// </summary>
	/// <param name="delegate">The delegate to set.</param>
	/// <param name="context">The context data to set.</param>
	public void Set(Delegate @delegate, TContext context = default)
	{
		_delegate = @delegate;
		_context = context;
	}
	
	/// <summary>
	/// Creates a new closure with the specified action.
	/// </summary>
	/// <param name="action">The action for the closure to invoke.</param>
	/// <returns>A new closure object which invokes the specified action.</returns>
	public static Closure<TContext> Create(Action action) => new(action);
	
	/// <summary>
	/// Creates a new closure with the specified action and context data.
	/// </summary>
	/// <param name="action">The action for the closure to invoke.</param>
	/// <param name="context">The context data to provide to the action.</param>
	/// <returns>A new closure object which invokes the specified action with the specified context data.</returns>
	public static Closure<TContext> Create(Action<TContext> action, TContext context) => new(action, context);
	
	/// <summary>
	/// Creates a new closure with the specified Func.
	/// </summary>
	/// <param name="func">The Func for the closure to invoke and return a result.</param>
	/// <typeparam name="TResult">The result type.</typeparam>
	/// <returns>A new closure object which invokes the specified Func.</returns>
	public static Closure<TContext> Create<TResult>(Func<TResult> func) => new(func);
	
	/// <summary>
	/// Creates a new closure with the specified Func and context data.
	/// </summary>
	/// <param name="func">The Func for the closure to invoke and return a result.</param>
	/// <param name="context">The context data to provide to the action.</param>
	/// <typeparam name="TResult">The result type.</typeparam>
	/// <returns>A new closure object which invokes the specified Func with the specified context data.</returns>
	public static Closure<TContext> Create<TResult>(Func<TContext, TResult> func, TContext context) => new(func, context);
}