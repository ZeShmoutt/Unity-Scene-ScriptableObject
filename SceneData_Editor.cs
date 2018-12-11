using UnityEditor;
using UnityEngine.SceneManagement;

namespace ZeShmouttsAssets.Effects.EditorScripts
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
			script.sceneName = (script.scene != null) ? script.scene.name : "";
			script.sceneInt = (script.scene != null) ? SceneManager.GetSceneByName(script.sceneName).buildIndex : -2;
		}

		#endregion
	}
}