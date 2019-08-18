using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZeShmouttsAssets.DataContainers
{
	[CreateAssetMenu(fileName = "New Scene Data", menuName = "ZeShmoutt's Assets/Data Containers/Scene")]
	public class SceneData : ScriptableObject, ISerializationCallbackReceiver
	{
		#region Variables

#if UNITY_EDITOR
#pragma warning disable 0649
		/// <summary>
		/// Editor-only scene object. Do not use in a runtime script !
		/// </summary>
		[SerializeField] private UnityEditor.SceneAsset scene;
#pragma warning restore 0649
#endif

		/// <summary>
		/// The scene's name.
		/// </summary>
		public string SceneName { get { return sceneName; } }

		/// <summary>
		/// The scene's build index.
		/// </summary>
		public int SceneIndex { get { return sceneIndex; } }

		/// <summary>
		/// The scene's path.
		/// </summary>
		public string ScenePath { get { return scenePath; } }

		/// <summary>
		/// Internal scene name.
		/// </summary>
		[SerializeField] private string sceneName = null;

		/// <summary>
		/// Internal scene index.
		/// </summary>
		[SerializeField] private int sceneIndex = -2;

		/// <summary>
		/// Internal scene path.
		/// </summary>
		[SerializeField] private string scenePath = null;

		#endregion

		#region Serialization Callback

		public void OnAfterDeserialize()
		{

		}

		public void OnBeforeSerialize()
		{
#if UNITY_EDITOR

			string nameValue = (scene != null) ? scene.name : null;
			int indexValue = -2;
			string pathValue = (scene != null) ? UnityEditor.AssetDatabase.GetAssetPath(scene) : null;

			UnityEditor.EditorBuildSettingsScene[] buildSettingsScenes = UnityEditor.EditorBuildSettings.scenes;
			if (buildSettingsScenes.Length > 0)
			{
				for (int i = 0; i < buildSettingsScenes.Length; i++)
				{
					if (UnityEditor.EditorBuildSettings.scenes[i].path == pathValue)
					{
						indexValue = i;
						break;
					}
				}
			}

			sceneName = nameValue;
			sceneIndex = indexValue;
			scenePath = pathValue;

#endif
		}

		#endregion

		#region Load From Default

		/// <summary>
		/// Shortcut to load the scene asynchronously in the background.
		/// </summary>
		/// <param name="mode">If LoadSceneMode.Single then all current Scenes will be unloaded before loading.</param>
		/// <returns>Use the AsyncOperation to determine if the operation has completed.</returns>
		public AsyncOperation LoadAsync(LoadSceneMode mode)
		{
			if (sceneIndex >= 0)
			{
				return LoadAsyncFromBuildIndex(mode);
			}
			else
			{
				return LoadAsyncFromPath(mode);
			}
		}

		/// <summary>
		/// Shortcut to load the Scene asynchronously in the background.
		/// </summary>
		/// <param name="parameters">Struct that collects the various parameters into a single place except for the name and index.</param>
		/// <returns>Use the AsyncOperation to determine if the operation has completed.</returns>
		public AsyncOperation LoadAsync(LoadSceneParameters parameters)
		{
			if (sceneIndex >= 0)
			{
				return LoadAsyncFromBuildIndex(parameters);
			}
			else
			{
				return LoadAsyncFromPath(parameters);
			}
		}

		#endregion

		#region Load From Name

		/// <summary>
		/// Shortcut to load the scene asynchronously in the background, using the scene's name.
		/// </summary>
		/// <param name="mode">If LoadSceneMode.Single then all current Scenes will be unloaded before loading.</param>
		/// <returns>Use the AsyncOperation to determine if the operation has completed.</returns>
		public AsyncOperation LoadAsyncFromName(LoadSceneMode mode)
		{
			return SceneManager.LoadSceneAsync(sceneName, mode);
		}

		/// <summary>
		/// Shortcut to load the Scene asynchronously in the background, using the scene's name.
		/// </summary>
		/// <param name="parameters">Struct that collects the various parameters into a single place except for the name and index.</param>
		/// <returns>Use the AsyncOperation to determine if the operation has completed.</returns>
		public AsyncOperation LoadAsyncFromName(LoadSceneParameters parameters)
		{
			return SceneManager.LoadSceneAsync(sceneName, parameters);
		}

		#endregion

		#region Load From Path

		/// <summary>
		/// Shortcut to load the scene asynchronously in the background, using the scene's path.
		/// </summary>
		/// <param name="mode">If LoadSceneMode.Single then all current Scenes will be unloaded before loading.</param>
		/// <returns>Use the AsyncOperation to determine if the operation has completed.</returns>
		public AsyncOperation LoadAsyncFromPath(LoadSceneMode mode)
		{
			return SceneManager.LoadSceneAsync(scenePath, mode);
		}

		/// <summary>
		/// Shortcut to load the Scene asynchronously in the background, using the scene's path.
		/// </summary>
		/// <param name="parameters">Struct that collects the various parameters into a single place except for the name and index.</param>
		/// <returns>Use the AsyncOperation to determine if the operation has completed.</returns>
		public AsyncOperation LoadAsyncFromPath(LoadSceneParameters parameters)
		{
			return SceneManager.LoadSceneAsync(scenePath, parameters);
		}

		#endregion

		#region Load From Build Index

		/// <summary>
		/// Shortcut to load the scene asynchronously in the background, using the scene's build index.
		/// </summary>
		/// <param name="mode">If LoadSceneMode.Single then all current Scenes will be unloaded before loading.</param>
		/// <returns>Use the AsyncOperation to determine if the operation has completed.</returns>
		public AsyncOperation LoadAsyncFromBuildIndex(LoadSceneMode mode)
		{
			return SceneManager.LoadSceneAsync(sceneIndex, mode);
		}

		/// <summary>
		/// Shortcut to load the Scene asynchronously in the background, using the scene's build index.
		/// </summary>
		/// <param name="parameters">Struct that collects the various parameters into a single place except for the name and index.</param>
		/// <returns>Use the AsyncOperation to determine if the operation has completed.</returns>
		public AsyncOperation LoadAsyncFromBuildIndex(LoadSceneParameters parameters)
		{
			return SceneManager.LoadSceneAsync(sceneIndex, parameters);
		}

		#endregion
	}
}