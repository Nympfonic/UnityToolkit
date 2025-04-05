using BepInEx.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace UnityToolkit.Utils;

internal static class AssemblyLoader
{
	internal static void LoadAssemblies(string directory, ManualLogSource logger)
	{
		string[] paths = Directory.GetFiles(path: directory, searchPattern: "*.dll");
		string json = File.ReadAllText(Path.Combine(directory, "Assemblies.jsonc"));
		string[] targetAssemblies = JsonConvert.DeserializeObject<string[]>(json);
		
		LogAssembliesJson(targetAssemblies, logger);
		
		for (var i = 0; i < paths.Length; i++)
		{
			string assemblyPath = paths[i];
			string fileName = Path.GetFileName(assemblyPath);
			
			if (fileName != "UnityToolkit.dll" && !targetAssemblies.Contains(fileName))
			{
				logger.LogWarning($"'{fileName}' detected in '{directory}' but isn't a UnityToolkit assembly! Please remove it!");
				continue;
			}
			
			var assemblyName = AssemblyName.GetAssemblyName(assemblyPath);
			Assembly.Load(assemblyName);
		}
	}
	
	[Conditional("DEBUG")]
	private static void LogAssembliesJson(string[] assemblies, ManualLogSource logger)
	{
		logger.LogInfo("Assemblies.jsonc: " + string.Join(", ", assemblies));
	}
}