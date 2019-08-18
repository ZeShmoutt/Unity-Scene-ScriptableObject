# Unity Scenes as ScriptableObjects
A way of storing scenes in a ScriptableObject.

You directly pick a scene on the ScriptableObject, while the Editor script automatically saves the corresponding name, path and build index. You can then use one of the `LoadAsync` methods directly from the ScriptableObject like you'd do with `SceneManagement` : `LoadAsyncFromName()`, `LoadAsyncFromPath()`, and `LoadAsyncFromBuildIndex()`. As a "default" option, `LoadAsync()` is a shortcut that will attempt to use the build index, or the scene's path if the build index is unavailable or invalid. You can also directly get all three values (name, path, or build index) through public properties : `SceneName`, `ScenePath`, and `SceneIndex`.

Note that if no scene is assigned, `SceneName` and `ScenePath` return `null` as expected, but `SceneIndex` returns `-2`, because as explained [here](https://docs.unity3d.com/ScriptReference/SceneManagement.Scene-buildIndex.html), `Scene.buildIndex` already returns `-1` if the scene comes from an AssetBundle. `SceneIndex` will also return `-2` if the scene is assigned but not in the build settings.
