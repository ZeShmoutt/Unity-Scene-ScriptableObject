# Unity Scenes as ScriptableObjects

A way of storing scenes in a ScriptableObject.

## Uses and restrictions

As per the MIT license, this is pretty much unrestricted in use and modification.

I'd appreciate a lot if you could mention me somewhere if you use it, though.

## Links

 - [License](LICENSE)
 - [Changelog](CHANGELOG.md)

## Installation

 - Unity 2019 or newer : Open the Package manager, select  and import the package with the git URL.
 - Unity 2018 or older : Clone/download the repo and put it in your Unity project's assets.

-----

## Why would I need to store a scene in a ScriptableObject ?

In Unity, you can load a scene by either its name, its path or its build index - so the common way of picking the scene is by adding a `public int` or a `public string` to your script and load from that.

Six months later, you have dozens of scenes and dozens of different scripts loading scenes. If you need to change a single scene (remove it from the build settings, rename it, whatever), you *will* forget to do the corresponding change somewhere, resulting in incorrect scenes being loaded or trying to load a scene that doesn't even exist anymore.

By using my SceneData, you can directly reference the SceneData anywhere you need it, and let it do the work of figuring where the scene is in your built game. It doesn't care about you changing the name, path or build index : as long as you tell it which scene it must point to, it *will* find it.

With all that free time you suddenly get from no longer having to debug every single scene, you can now focus on important stuff like how terrible Unity's default way of loading scenes is.

## How to create a Scene ScriptableObject

As usual with ScriptableObjects :

* Using Unity's menu bar : `Assets/Create/ZeShmoutt's Assets/Data Containers/Scene Data`
* In the Project window : `Create/ZeShmoutt's Assets/Data Containers/Scene Data`
* As a context menu on a SceneAsset : `Create/ZeShmoutt's Assets/Data Containers/Scene Data from SceneAsset`

## How to use in your scripts

**Step 1 :** Add `using ZeShmouttsAssets.DataContainers;` at the top with the other `using`s.

**Step 2 :** Directly assign a scene on the ScriptableObject, while the Editor script automatically saves the corresponding name, path and build index.

**Step 3 :** You can then use one of the `LoadAsync` methods directly from the ScriptableObject like you'd do with `SceneManagement` : `LoadAsyncFromName()`, `LoadAsyncFromPath()`, and `LoadAsyncFromBuildIndex()`. As a "default" option, `LoadAsync()` is a shortcut that will attempt to use the build index, or the scene's path if the build index is unavailable or invalid. You can also directly get all three values (name, path, or build index) through public properties : `SceneName`, `ScenePath`, and `SceneIndex`.

**IMPORTANT :** If no scene is assigned, `SceneName` and `ScenePath` return `null` as expected, but `SceneIndex` returns `-2`, because as explained [here](https://docs.unity3d.com/ScriptReference/SceneManagement.Scene-buildIndex.html), `Scene.buildIndex` already returns `-1` if the scene comes from an AssetBundle. `SceneIndex` will also return `-2` if the scene is assigned but not in the build settings.
