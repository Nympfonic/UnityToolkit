using BepInEx;
using System.IO;
using System.Reflection;

namespace UnityToolkit
{
    [BepInPlugin("com.Arys.UnityToolkit", "Unity Toolkit", "1.0.0")]
    public class UnityToolkitPlugin : BaseUnityPlugin
    {
        public UnityToolkitPlugin()
        {
            string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string uniTaskAssemblyPath = Path.Combine(directory, "UniTask.dll");
            string unityCollectionsAssemblyPath = Path.Combine(directory, "Unity.Collections.dll");

            // Load assemblies into memory
            Assembly.Load(uniTaskAssemblyPath);
            Assembly.Load(unityCollectionsAssemblyPath);

            // How to use this library:
            // 1. Add [BepInDependency("com.Arys.UnityToolkit")] attribute to your plugin class
            // 2. Add the UniTask.dll and Unity.Collections.dll as assembly references to your project
            // 3. You can now use UniTask and Unity Collections in your mod

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
            // 2. Unity Collections is included in this library for the additional NativeContainer types it provides
            //    such as NativeList, NativeHashMap, NativeMultiHashMap, and NativeQueue.
            //    These are useful data types when you are working with Unity's Job system as they are thread-safe.
            //    Documentation: https://docs.unity3d.com/Packages/com.unity.collections@0.9/manual/index.html

            // Is it safe?
            // Yes, both assemblies are just compiled versions of their original git repo
            // I've also included a VirusTotal scan of both on the GitHub releases page
        }
    }
}
