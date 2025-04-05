# UnityToolkit

## What is this library for?

This library aims to provide additional tools to client modders which will (hopefully) allow them to write more optimised code
and reduce the performance cost and memory allocation of their code even further.

## How to use this library:

1. Download the latest release
2. Copy the assemblies from the release zip to where you store your project's assembly references
3. Add all the assemblies as assembly references to your project
4. Add `[BepInDependency("com.Arys.UnityToolkit")]` attribute to your plugin class (I recommend you add a minimum version string to the attribute)
5. You can now use <span style="color:#0090AA">UniTask</span>, <span style="color:#B00090">Unity.Collections</span> and <span style="color:#009000">ZString</span> in your mod

## What features does this library provide?

1. <span style="color:#0090AA">UniTask</span> is a near zero-allocating, performant version of C# Tasks that's suited for Unity than the standard C# implementation
   - It doesn't use threads or `SynchronizationContext`/`ExecutionContext` so the result is faster performance and lower allocation while matching Unity threading (single-thread)
   - It is also possible to replace Unity Coroutine usage, which has poor performance and higher memory allocation, with UniTask instead
   - Documentation: https://github.com/Cysharp/UniTask
2. <span style="color:#B00090">Unity.Collections</span> is included in this library for the additional `NativeContainer` types it provides
   - `NativeList`, `NativeHashMap`, `NativeMultiHashMap`, and `NativeQueue`: these are useful data types when you are working with Unity's Jobs system as they are thread-safe
   - Documentation: https://docs.unity3d.com/Packages/com.unity.collections@2.6/manual/collections-overview.html
3. <span style="color:#009000">ZString</span> is a near zero-allocating string builder, which is also made by the developer of UniTask.
4. Useful Unity-related extension methods for types such as `UnityEngine.Object` and `Vector3`
5. Generic structures/design patterns which can be adapted to any type
   - `Processor` - Chain of Responsibility pattern (aka middleware)
   - `Closure` - a struct to allow manual creation and control of closures
   - `EventBus` - decoupled event binding and invocation

## Is it safe?

Yes, all assemblies are just compiled versions of their original git repo
I've also included a VirusTotal scan of all assemblies on the GitHub releases page

## Building from source

1. Clone the repository:
    ```
    git clone https://github.com/Nympfonic/UnityToolkit.git
    ```
2. Place the compiled assemblies for UniTask, Unity.Collections and ZString in `project\UnityToolkit\References`.
    - ZString has two dependencies: `System.Runtime.CompilerServices.Unsafe.dll` and `System.Memory.dll`
    - You will need to place `System.Runtime.CompilerServices.Unsafe.dll` in `project\UnityToolkit.Prepatcher\CopyToOutput`
    - `System.Memory.dll` can be placed in the References folder mentioned previously
3. Adjust the reference paths, macros and build events in the .csproj files
4. Open solution in your preferred C# IDE
5. Build solution