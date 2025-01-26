using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace UnityToolkit.Structures;

/// <summary>
/// Interface for Chain of Responsibility (COR) pattern.
/// </summary>
/// <typeparam name="TProcessor">The processor type.</typeparam>
/// <typeparam name="TData">The data needed for the processor.</typeparam>
/// <remarks>All processors should be derived from this interface or <see cref="ProcessorBase{TData}"/>.</remarks>
/// <seealso cref="IAsyncProcessor{TProcessor,TData}"/>
public interface IProcessor<TProcessor, in TData>
{
	TProcessor SetNext(TProcessor nextProcessor);
	bool Process(TData data);
}

/// <summary><inheritdoc cref="IProcessor{TProcessor,TData}"/></summary>
/// <typeparam name="TProcessor">The processor type.</typeparam>
/// <typeparam name="TData">The data needed for the processor.</typeparam>
/// <remarks>
/// All async processors should be derived from this interface or <see cref="AsyncProcessorBase{TData}"/>.
/// </remarks>
/// <seealso cref="IProcessor{TProcessor,TData}"/>
public interface IAsyncProcessor<TProcessor, in TData>
{
	TProcessor SetNext(TProcessor nextProcessor);
	UniTask<bool> ProcessAsync(TData data);
}

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