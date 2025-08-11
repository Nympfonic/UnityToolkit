namespace System.Runtime.CompilerServices;

using ComponentModel;

/// <summary>
/// Reserved to be used by the compiler for tracking metadata.
/// This class should not be used by developers in source code.
/// </summary>
/// <remarks>Allows the usage of the 'init' keyword from C# 9.0 for property setters in netstandard2.x</remarks>
[EditorBrowsable(EditorBrowsableState.Never)]
internal static class IsExternalInit;