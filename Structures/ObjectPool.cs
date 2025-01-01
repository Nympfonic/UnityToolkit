using JetBrains.Annotations;
using System.Collections.Concurrent;

namespace UnityToolkit.Structures;

public enum ObjectPoolType
{
	DiscardOldest,
	DiscardNewest
}

[UsedImplicitly]
public class ObjectPool<T>
	where T : new()
{
	private readonly int _maxPoolSize;
	private readonly ObjectPoolType _poolType;
	private readonly ConcurrentQueue<T> _objectPool = [];
	private int _count;

	public ObjectPool(int maxPoolSize, ObjectPoolType poolType = ObjectPoolType.DiscardNewest)
	{
		_maxPoolSize = maxPoolSize;
		_poolType = poolType;
	}

	public void AddToPool(T obj)
	{
		if (_count + 1 <= _maxPoolSize)
		{
			_objectPool.Enqueue(obj);
			_count++;
			return;
		}

		if (_poolType == ObjectPoolType.DiscardOldest && _objectPool.TryDequeue(out T _))
		{
			_count--;
			_objectPool.Enqueue(obj);
			_count++;
		}
	}

	public T TakeFromPool()
	{
		T obj = default;
		if (_count == 0)
		{
			return obj;
		}

		if (_objectPool.TryDequeue(out obj))
		{
			_count--;
		}

		return obj;
	}
}