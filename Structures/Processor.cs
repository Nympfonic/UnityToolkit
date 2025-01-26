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
	void Process(TData data);
	bool TryProcess(TData data);
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
	UniTask ProcessAsync(TData data);
	UniTask<bool> TryProcessAsync(TData data);
}

[UsedImplicitly]
public abstract class ProcessorBase<TData> : IProcessor<ProcessorBase<TData>, TData>
{
	private ProcessorBase<TData> _nextProcessor;
	
	public ProcessorBase<TData> SetNext(ProcessorBase<TData> nextProcessor) => _nextProcessor = nextProcessor;
	
	public virtual void Process(TData data) => _nextProcessor?.Process(data);
	
	public virtual bool TryProcess(TData data)
	{
		return _nextProcessor == null || _nextProcessor.TryProcess(data);
	}
}

[UsedImplicitly]
public abstract class AsyncProcessorBase<TData> : IAsyncProcessor<AsyncProcessorBase<TData>, TData>
{
	private AsyncProcessorBase<TData> _nextProcessor;
	
	public AsyncProcessorBase<TData> SetNext(AsyncProcessorBase<TData> nextProcessor) => _nextProcessor = nextProcessor;
	
	public virtual async UniTask ProcessAsync(TData data)
	{
		if (_nextProcessor == null)
		{
			return;
		}
		
		await _nextProcessor.ProcessAsync(data);
	}
	
	public virtual async UniTask<bool> TryProcessAsync(TData data)
	{
		return _nextProcessor == null || await _nextProcessor.TryProcessAsync(data);
	}
}