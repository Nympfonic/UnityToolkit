using JetBrains.Annotations;
using System.Collections.Concurrent;

namespace UnityToolkit.Structures;

public enum ObjectPoolType
{
	DiscardOldest,
	DiscardNewest
}

public interface IObjectPool<T>
{
	T Get();
	void Return(T obj);
}

/// <summary>
/// A thread-safe object pool pattern.
/// </summary>
/// <param name="maxPoolSize">The maximum pool size. If set to zero, the pool can expand to max integer size.</param>
/// <param name="poolType">The pool type based on <see cref="ObjectPoolType"/>.</param>
/// <typeparam name="T">The type to store in the pool.</typeparam>
[UsedImplicitly]
public class ObjectPool<T>(int maxPoolSize = 0, ObjectPoolType poolType = ObjectPoolType.DiscardNewest) : IObjectPool<T>
	where T : new()
{
	private readonly ConcurrentBag<T> _pool = [];
	private int _count;
	
	public T Get()
	{
		if (_count == 0)
		{
			return new T();
		}
		
		if (_pool.TryTake(out T obj))
		{
			_count--;
		}
		
		return obj;
	}
	
	public void Return(T obj)
	{
		if (maxPoolSize == 0 || _count + 1 <= maxPoolSize)
		{
			_pool.Add(obj);
			_count++;
			return;
		}
		
		if (poolType == ObjectPoolType.DiscardOldest && _pool.TryTake(out _))
		{
			_count--;
			_pool.Add(obj);
			_count++;
		}
	}
}