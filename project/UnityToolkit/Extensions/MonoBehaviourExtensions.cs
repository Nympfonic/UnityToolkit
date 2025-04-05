using JetBrains.Annotations;
using UnityEngine;

namespace UnityToolkit.Extensions;

/// <summary>
/// Extensions for Unity's MonoBehaviour
/// </summary>
public static class MonoBehaviourExtensions
{
	/// <summary>Allows null-coalescing operators to be used with Unity's <see cref="UnityEngine.Object"/>.</summary>
	/// <param name="self">The Unity object instance.</param>
	/// <typeparam name="TObject">The Unity object type.</typeparam>
	/// <returns>The Unity object instance or null.</returns>
	/// <example><code>Singleton&lt;GameWorld&gt;.Instance.OrNull()?.MainPlayer;</code></example>
	[UsedImplicitly]
	[CanBeNull]
	public static TObject OrNull<TObject>(this TObject self) where TObject : UnityEngine.Object
	{
		return self ? self : null;
	}
	
	/// <summary>Gets the full path of the transform in the scene hierarchy.</summary>
	/// <remarks>This allocates memory due to string concatenation.</remarks>
	[UsedImplicitly]
	[NotNull]
	public static string GetPath(this Transform transform, string delimiter = "/")
	{
		if (!transform.parent)
		{
			return transform.name;
		}
		
		return transform.parent.GetPath(delimiter) + delimiter + transform.name;
	}
}