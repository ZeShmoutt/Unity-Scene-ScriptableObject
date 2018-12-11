using UnityEngine;

namespace ZeShmouttsAssets.Effects
{
	[CreateAssetMenu(fileName = "New Scene Data", menuName = "ZeShmoutt's Assets/Data Containers/Scene")]
	public class SceneData : ScriptableObject
	{
		#if UNITY_EDITOR
		public UnityEditor.SceneAsset scene;
		#endif
		
		public string SceneName { get { return sceneName; } }
		public int SceneIndex { get { return sceneIndex; } }

		[SerializeField] private string sceneName;
		[SerializeField] private int sceneIndex;
	} 
}
