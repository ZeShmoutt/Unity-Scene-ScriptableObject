using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ZeShmouttsAssets.DataContainers.EditorScripts
{
	[CustomEditor(typeof(SceneData))]
	[CanEditMultipleObjects]
	public class SceneData_Editor : Editor
	{
		#region Context Menu

		private const string MenuItemPath = "Assets/Create/ZeShmoutt's Assets/Data Containers/Scene Data from SceneAsset";

		[MenuItem(MenuItemPath, false, 10)]
		private static void CreateFromSceneObject()
		{
			Object file = Selection.activeObject;
			if (file is SceneAsset)
			{
				CreateSceneData(file as SceneAsset);
			}
			else
			{
				Debug.LogError("Selected Object isn't a SceneAsset.");
			}
		}

		[MenuItem(MenuItemPath, true, 10)]
		private static bool IsSceneAsset()
		{
			Object file = Selection.activeObject;
			return (file is SceneAsset);
		}

		/// <summary>
		/// Creates a new SceneData based on an existing SceneAsset in the same folder.
		/// </summary>
		/// <param name="scene">SceneAsset used as a reference.</param>
		private static void CreateSceneData(SceneAsset scene)
		{
			SceneData assetObject = ScriptableObject.CreateInstance<SceneData>();
			
			BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
			FieldInfo field = typeof(SceneData).GetField("scene", bindFlags);
			field.SetValue(assetObject, scene);

			string scenePath = AssetDatabase.GetAssetPath(scene);
			string folderPath = scenePath.Substring(0, scenePath.LastIndexOf('/'));
			string assetPath = string.Format("{0}/{1}_Data.asset", folderPath, scene.name);

			AssetDatabase.CreateAsset(assetObject, assetPath);
			AssetDatabase.SaveAssets();

			EditorUtility.FocusProjectWindow();

			Selection.activeObject = assetObject;
		}

		#endregion

		#region Variables

		private SceneData script;

		SerializedProperty sceneObject;
		SerializedProperty sceneName;
		SerializedProperty sceneIndex;
		SerializedProperty scenePath;

		#endregion

		#region Unity Editor GUI

		private void OnEnable()
		{
			script = (SceneData)target;

			sceneObject = serializedObject.FindProperty("scene");
			sceneName = serializedObject.FindProperty("sceneName");
			sceneIndex = serializedObject.FindProperty("sceneIndex");
			scenePath = serializedObject.FindProperty("scenePath");

			EditorBuildSettings.sceneListChanged += HandleExternalSceneListChange;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			DrawScriptField();

			EditorGUILayout.Space();

			UIScene();

			if (GUI.changed)
			{
				Undo.RecordObject(target, "Field change");
				serializedObject.ApplyModifiedProperties();
			}
		}

		private void OnDisable()
		{
			EditorBuildSettings.sceneListChanged -= HandleExternalSceneListChange;
		}

		private void HandleExternalSceneListChange()
		{
			script.OnBeforeSerialize();
			Repaint();
		}

		#endregion

		#region Parts

		/// <summary>
		/// Draws a script field similar to the regular inspector, allowing quick access to the script itself.
		/// </summary>
		protected void DrawScriptField()
		{
			bool wasEnabled = GUI.enabled;

			GUI.enabled = false;
			SerializedProperty prop = serializedObject.FindProperty("m_Script");
			EditorGUILayout.PropertyField(prop, true);
			GUI.enabled = wasEnabled;
		}

		/// <summary>
		/// Parent for drawing editor settings.
		/// </summary>
		void UIScene()
		{
			bool sceneNotNull = sceneObject.objectReferenceValue != null;

			EditorGUILayout.BeginVertical(GUI.skin.box);

			EditorGUILayout.PropertyField(sceneObject, false);

			script.OnBeforeSerialize();

			EditorGUI.indentLevel++;

			bool wasEnabled = GUI.enabled;

			GUI.enabled = false;

			string pathValue = scenePath.stringValue;
			string nameValue = sceneName.stringValue;
			int indexValue = sceneIndex.intValue;

			EditorGUILayout.LabelField("Path", sceneNotNull ? pathValue : "N/A");

			EditorGUILayout.LabelField("Name", sceneNotNull ? nameValue : "N/A");

			EditorGUILayout.LabelField("Build Index", sceneNotNull && indexValue >= -1 ? indexValue.ToString() : "N/A");

			GUI.enabled = wasEnabled;

			EditorGUI.indentLevel--;

			EditorGUILayout.EndVertical();

			if (sceneNotNull)
			{
				if (indexValue < 0)
				{
					NotInBuildSettings(pathValue);
				}
				else if (indexValue >= 0 && EditorBuildSettings.scenes.Length >= indexValue + 1 && !EditorBuildSettings.scenes[indexValue].enabled)
				{
					DisabledInBuildSettings(sceneIndex.intValue);
				}
			}
			else
			{
				EditorGUILayout.HelpBox("No scene selected.", MessageType.Error);
			}
		}

		/// <summary>
		/// Warning info label + button drawn when the selected scene isn't in the build settings.
		/// </summary>
		void NotInBuildSettings(string path)
		{
			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.HelpBox("The selected scene is not in the build settings.", MessageType.Warning);
			if (GUILayout.Button("Add to build", GUILayout.Width(100), GUILayout.Height(40)))
			{
				EditorBuildSettingsScene sceneToAdd = new EditorBuildSettingsScene(path, true);
				List<EditorBuildSettingsScene> scenesInBuild = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
				scenesInBuild.Add(sceneToAdd);
				EditorBuildSettings.scenes = scenesInBuild.ToArray();

				Debug.LogFormat(sceneObject.objectReferenceValue, "Scene '{0}' has been added to build settings at index {1}.", path, scenesInBuild.Count - 1);
			}

			EditorGUILayout.EndHorizontal();
		}

		/// <summary>
		/// Warning info label + button drawn when the selected scene is disabled in the build settings.
		/// </summary>
		void DisabledInBuildSettings(int index)
		{
			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.HelpBox("The selected scene is disabled in the build settings.", MessageType.Warning);
			if (GUILayout.Button("Enable in build", GUILayout.Width(100), GUILayout.Height(40)))
			{
				EditorBuildSettingsScene[] buildSettingsScenes = EditorBuildSettings.scenes;
				buildSettingsScenes[index].enabled = true;
				EditorBuildSettings.scenes = buildSettingsScenes;

				Debug.LogFormat(sceneObject.objectReferenceValue, "Scene #{0} has been enabled.", index);
			}

			EditorGUILayout.EndHorizontal();
		}

		#endregion
	}
}