namespace UnityToolkit;

public static class MonoBehaviourExtensions
{
	public static T OrNull<T>(this T self) where T : UnityEngine.Object => self ? self : null;
}