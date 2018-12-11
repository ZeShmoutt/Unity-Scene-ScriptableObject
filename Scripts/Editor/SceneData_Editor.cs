using UnityEditor;
using UnityEngine.SceneManagement;

namespace ZeShmouttsAssets.DataContainers.EditorScripts
{
	[CustomEditor(typeof(SceneData))]
	[CanEditMultipleObjects]
	public class SceneData_Editor : Editor
	{
		#region Variables

		private SceneData script;

		#endregion

		#region GUI

		public override void OnInspectorGUI()
		{
			script = (SceneData)target;

			UIScene();

			EditorUtility.SetDirty(script);
		}

		#endregion

		#region Parts

		void UIScene()
		{
			script.scene = EditorGUILayout.ObjectField("UI Scene", script.scene, typeof(SceneAsset), false) as SceneAsset;

			SerializedProperty name = serializedObject.FindProperty("SceneData.sceneName");
			SerializedProperty index = serializedObject.FindProperty("SceneData.sceneIndex");

			name.stringValue = (script.scene != null) ? script.scene.name : "";
			index.intValue = (script.scene != null) ? SceneManager.GetSceneByName(script.scene.name).buildIndex : -2;
		}

		#endregion
	}
}
