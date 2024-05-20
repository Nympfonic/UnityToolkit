using BepInEx;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace UnityToolkit
{
    [BepInPlugin("com.Arys.UnityToolkit", "Unity Toolkit", "1.0.0")]
    public class UnityToolkitPlugin : BaseUnityPlugin
    {
        private readonly HashSet<string> _assemblyFileNames = [
            "UniTask.dll",
            "UniTask.Linq.dll",
            "UniTask.DOTween.dll",
            "UniTask.TextMeshPro.dll",
            "Unity.Collections.dll"
        ];
        private readonly HashSet<Assembly> _assembliesInMemory = [];

        public UnityToolkitPlugin()
        {
            string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Load assemblies into memory
            foreach (var assemblyFileName in _assemblyFileNames)
            {
                string assemblyPath = Path.Combine(directory, assemblyFileName);
                var assemblyName = AssemblyName.GetAssemblyName(assemblyPath);
                var assembly = Assembly.Load(assemblyName);
                _assembliesInMemory.Add(assembly);
            }
        }

        // How to use this library:
        // 1. Download the latest release
        // 2. Copy the assemblies **EXCEPT** the UnityToolkit.dll from the release zip to where you store your project's assembly references
        // 3. Add the assemblies as assembly references to your project
        // 4. Add [BepInDependency("com.Arys.UnityToolkit")] attribute to your plugin class
        // 5. You can now use UniTask and Unity.Collections in your mod

        // What is this library for?
        // This library aims to provide additional tools to client modders which will (hopefully) allow them to write more optimised code
        // and reduce the performance cost and memory allocation of their code even further

        // What features does this library provide?
        // 1. UniTask is a near zero-allocating, performant version of C# Tasks that's suited for Unity than the standard C# implementation.
        //    It doesn't use threads or SynchronizationContext/ExecutionContext so the result is faster performance and lower allocation
        //    while matching Unity threading (single-thread).
        //    It is also possible to replace Unity Coroutine usage, which has poor performance and higher memory allocation, with UniTask instead.
        //    Documentation: https://github.com/Cysharp/UniTask
        //
        // 2. Unity.Collections is included in this library for the additional NativeContainer types it provides
        //    such as NativeList, NativeHashMap, NativeMultiHashMap, and NativeQueue.
        //    These are useful data types when you are working with Unity's Job system as they are thread-safe.
        //    Documentation: https://docs.unity3d.com/Packages/com.unity.collections@0.9/manual/index.html

        // Is it safe?
        // Yes, both assemblies are just compiled versions of their original git repo
        // I've also included a VirusTotal scan of both on the GitHub releases page
    }
}
