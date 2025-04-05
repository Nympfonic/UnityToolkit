using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace UnityToolkit.Prepatcher
{
	/// <summary>
	/// Replaces System.Runtime.CompilerServices.Unsafe.dll with an updated version for ZString.
	/// </summary>
	public static class Patcher
	{
		private const string ASSEMBLY_NAME = "System.Runtime.CompilerServices.Unsafe.dll";
		
		private static readonly string s_assemblyPath;
		private static AssemblyDefinition s_assemblyDefinition;
		
		public static IEnumerable<string> TargetDLLs { get; } = new[] {ASSEMBLY_NAME};
		
		static Patcher()
		{
			string currentPath = Path.GetFullPath(Assembly.GetExecutingAssembly().Location);
			s_assemblyPath = Path.Combine(currentPath, ASSEMBLY_NAME);
		}
		
		public static void Initialize()
		{
			if (!File.Exists(s_assemblyPath)) return;
			
			try
			{
				Console.WriteLine($"[UnityToolkit-Prepatcher] Loading new version of assembly '{ASSEMBLY_NAME}' from {s_assemblyPath}");
				s_assemblyDefinition = AssemblyDefinition.ReadAssembly(s_assemblyPath);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[UnityToolkit-Prepatcher] Failed to load assembly '{ASSEMBLY_NAME}' from {s_assemblyPath}:\n{ex.StackTrace}");
			}
		}
		
		public static void Patch(ref AssemblyDefinition assembly)
		{
			if (s_assemblyDefinition != null)
			{
				assembly = s_assemblyDefinition;
			}
		}
	}
}