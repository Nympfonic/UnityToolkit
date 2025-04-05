using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace UnityToolkit.Structures;

internal interface IProcessor<TProcessor, in TData>
{
	TProcessor SetNext(TProcessor nextProcessor);
	bool Process(TData data);
}

internal interface IAsyncProcessor<TProcessor, in TData>
{
	TProcessor SetNext(TProcessor nextProcessor);
	UniTask<bool> ProcessAsync(TData data);
}

/// <summary>
/// Chain of Responsibility (COR) pattern. Allows you to create a chain of processors to process data while being modular.
/// </summary>
/// <typeparam name="TData">The data needed for the processor.</typeparam>
/// <remarks>All processors should be derived from this class.</remarks>
/// <seealso cref="AsyncProcessorBase{TData}"/>
[UsedImplicitly]
public abstract class ProcessorBase<TData> : IProcessor<ProcessorBase<TData>, TData>
{
	private ProcessorBase<TData> _nextProcessor;
	
	/// <summary>
	/// Sets the next processor to process the data.
	/// </summary>
	/// <param name="nextProcessor">The next processor.</param>
	/// <returns>The next processor or otherwise the current processor.</returns>
	public ProcessorBase<TData> SetNext(ProcessorBase<TData> nextProcessor) => _nextProcessor = nextProcessor;
	
	/// <summary>
	/// The data is processed within this method. Must be overridden by derived classes to customize the data processing.
	/// </summary>
	/// <param name="data">The data type.</param>
	/// <returns>True if it has reached the end of the chain or the next processor's processing succeeds. False otherwise.</returns>
	public virtual bool Process(TData data)
	{
		return _nextProcessor == null || _nextProcessor.Process(data);
	}
}

/// <summary><inheritdoc cref="ProcessorBase{TData}"/></summary>
/// <typeparam name="TData"><inheritdoc cref="ProcessorBase{TData}"/></typeparam>
/// <remarks>All async processors should be derived from this class.</remarks>
/// <seealso cref="ProcessorBase{TData}"/>
[UsedImplicitly]
public abstract class AsyncProcessorBase<TData> : IAsyncProcessor<AsyncProcessorBase<TData>, TData>
{
	private AsyncProcessorBase<TData> _nextProcessor;
	
	/// <inheritdoc cref="ProcessorBase{TData}.SetNext"/>
	public AsyncProcessorBase<TData> SetNext(AsyncProcessorBase<TData> nextProcessor) => _nextProcessor = nextProcessor;
	
	/// <inheritdoc cref="ProcessorBase{TData}.Process"/>
	/// <returns>
	/// A UniTask with a boolean type. True if it has reached the end of the chain or the next processor's processing succeeds. False otherwise.
	/// </returns>
	public virtual async UniTask<bool> ProcessAsync(TData data)
	{
		return _nextProcessor == null || await _nextProcessor.ProcessAsync(data);
	}
}