using JetBrains.Annotations;

namespace UnityToolkit.Architectures.Processors;

internal interface IProcessor<TProcessor, in TData>
{
	TProcessor SetNext(TProcessor nextProcessor);
	bool Process(TData data);
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