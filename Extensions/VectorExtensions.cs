using JetBrains.Annotations;
using UnityEngine;

namespace UnityToolkit.Extensions;

public static class VectorExtensions
{
	[UsedImplicitly]
	public static Vector3 SetX(this Vector3 vector, float value)
	{
		vector.Set(value, vector.y, vector.z);
		return vector;
	}

	[UsedImplicitly]
	public static Vector3 SetY(this Vector3 vector, float value)
	{
		vector.Set(vector.x, value, vector.z);
		return vector;
	}

	[UsedImplicitly]
	public static Vector3 SetZ(this Vector3 vector, float value)
	{
		vector.Set(vector.x, vector.y, vector.z);
		return vector;
	}

	[UsedImplicitly]
	public static Vector3 AddX(this Vector3 vector, float value)
	{
		vector.Set(vector.x + value, vector.y, vector.z);
		return vector;
	}

	[UsedImplicitly]
	public static Vector3 AddY(this Vector3 vector, float value)
	{
		vector.Set(vector.x, vector.y + value, vector.z);
		return vector;
	}

	[UsedImplicitly]
	public static Vector3 AddZ(this Vector3 vector, float value)
	{
		vector.Set(vector.x, vector.y, vector.z + value);
		return vector;
	}
}