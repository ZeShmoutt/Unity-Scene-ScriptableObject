# Unity Scenes as ScriptableObjects
A way of storing scenes in a ScriptableObject.

You directly assign a scene on the ScriptableObject, while the Editor script automatically assign the corresponding string and int (respectively the scene's name and index).

Because of the `#if UNITY_EDITOR`, the scene object variable is discarded in builds (which doesn't matter because it's an editor-only feature anyway), leaving the name and index ready for use in a cozy property, avoiding any risk of overwriting.
