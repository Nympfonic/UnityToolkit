using JetBrains.Annotations;

namespace UnityToolkit.Structures;

/// <summary>All processors should be derived from this interface.</summary>
/// <typeparam name="TProcessor">The processor type.</typeparam>
/// <typeparam name="TData">The data needed for the processor.</typeparam>
[UsedImplicitly]
public interface IProcessor<TProcessor, in TData>
{
	TProcessor SetNext(TProcessor nextProcessor);
	void Process(TData data);
}