using JetBrains.Annotations;
using UnityEngine;

namespace UnityToolkit.Extensions;

/// <summary>
/// Extensions for Unity's Vector3
/// </summary>
public static class VectorExtensions
{
	/// <summary>
	/// Sets the x-axis for the specified Vector3.
	/// </summary>
	/// <param name="vector">The Vector3 to modify.</param>
	/// <param name="value">The value to set to.</param>
	/// <returns>The modified Vector3.</returns>
	[UsedImplicitly]
	public static Vector3 SetX(this Vector3 vector, float value)
	{
		vector.Set(value, vector.y, vector.z);
		return vector;
	}
	
	/// <summary>
	/// Sets the y-axis for the specified Vector3.
	/// </summary>
	/// <param name="vector">The Vector3 to modify.</param>
	/// <param name="value">The value to set to.</param>
	/// <returns>The modified Vector3.</returns>
	[UsedImplicitly]
	public static Vector3 SetY(this Vector3 vector, float value)
	{
		vector.Set(vector.x, value, vector.z);
		return vector;
	}
	
	/// <summary>
	/// Sets the z-axis for the specified Vector3.
	/// </summary>
	/// <param name="vector">The Vector3 to modify.</param>
	/// <param name="value">The value to set to.</param>
	/// <returns>The modified Vector3.</returns>
	[UsedImplicitly]
	public static Vector3 SetZ(this Vector3 vector, float value)
	{
		vector.Set(vector.x, vector.y, vector.z);
		return vector;
	}
	
	/// <summary>
	/// Adds to the x-axis for the specified Vector3.
	/// </summary>
	/// <param name="vector">The Vector3 to modify.</param>
	/// <param name="value">The value to add.</param>
	/// <returns>The modified Vector3.</returns>
	[UsedImplicitly]
	public static Vector3 AddX(this Vector3 vector, float value)
	{
		vector.Set(vector.x + value, vector.y, vector.z);
		return vector;
	}
	
	/// <summary>
	/// Adds to the y-axis for the specified Vector3.
	/// </summary>
	/// <param name="vector">The Vector3 to modify.</param>
	/// <param name="value">The value to add.</param>
	/// <returns>The modified Vector3.</returns>
	[UsedImplicitly]
	public static Vector3 AddY(this Vector3 vector, float value)
	{
		vector.Set(vector.x, vector.y + value, vector.z);
		return vector;
	}
	
	/// <summary>
	/// Adds to the z-axis for the specified Vector3.
	/// </summary>
	/// <param name="vector">The Vector3 to modify.</param>
	/// <param name="value">The value to add.</param>
	/// <returns>The modified Vector3.</returns>
	[UsedImplicitly]
	public static Vector3 AddZ(this Vector3 vector, float value)
	{
		vector.Set(vector.x, vector.y, vector.z + value);
		return vector;
	}
}