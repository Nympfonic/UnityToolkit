using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace UnityToolkit.Architectures.Processors;

internal interface IAsyncProcessor<TProcessor, in TData>
{
	TProcessor SetNext(TProcessor nextProcessor);
	UniTask<bool> ProcessAsync(TData data);
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
	/// A UniTask with a boolean type.
	/// True if it has reached the end of the chain or the next processor's processing succeeds or otherwise false.
	/// </returns>
	public virtual async UniTask<bool> ProcessAsync(TData data)
	{
		return _nextProcessor == null || await _nextProcessor.ProcessAsync(data);
	}
}