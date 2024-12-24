using Cysharp.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace UnityToolkit.Extensions;

public static class MonoBehaviourExtensions
{
	/// <summary>Allows null-coalescing operators to be used with Unity's <see cref="UnityEngine.Object"/>.</summary>
	/// <param name="self">The Unity object instance.</param>
	/// <typeparam name="TObject">The Unity object type.</typeparam>
	/// <returns>The Unity object instance or null.</returns>
	/// <example><code>Singleton&lt;GameWorld&gt;.Instance.OrNull()?.MainPlayer;</code></example>
	[UsedImplicitly]
	[CanBeNull]
	public static TObject OrNull<TObject>(this TObject self) where TObject : Object => self ? self : null;

	/// <summary>Gets the full path of the transform in the scene hierarchy.</summary>
	/// <seealso cref="GetPathNonAlloc"/>
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

	/// <summary><inheritdoc cref="GetPath"/></summary>
	/// <remarks>Very low memory allocation due to usage of <see cref="ZString"/>.</remarks>
	[UsedImplicitly]
	[NotNull]
	public static string GetPathNonAlloc(this Transform transform, string delimiter = "/")
	{
		using Utf16ValueStringBuilder stringBuilder = ZString.CreateStringBuilder();
		while (transform.parent)
		{
			stringBuilder.Insert(0, transform.name);
			stringBuilder.Insert(0, delimiter);
			transform = transform.parent;
		}
		stringBuilder.Insert(0, transform.name);

		return stringBuilder.ToString();
	}
}