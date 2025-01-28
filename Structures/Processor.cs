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
	
	public ProcessorBase<TData> SetNext(ProcessorBase<TData> nextProcessor) => _nextProcessor = nextProcessor;
	
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
	
	public AsyncProcessorBase<TData> SetNext(AsyncProcessorBase<TData> nextProcessor) => _nextProcessor = nextProcessor;
	
	public virtual async UniTask<bool> ProcessAsync(TData data)
	{
		return _nextProcessor == null || await _nextProcessor.ProcessAsync(data);
	}
}