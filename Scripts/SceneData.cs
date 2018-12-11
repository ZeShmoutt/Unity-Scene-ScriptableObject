using UnityEngine;

namespace ZeShmouttsAssets.Effects
{
	[CreateAssetMenu(fileName = "New Scene Data", menuName = "ZeShmoutt's Assets/Data Containers/Scene")]
	public class SceneData : ScriptableObject
	{
#if UNITY_EDITOR
		public UnityEditor.SceneAsset scene;
#endif
		public string sceneName;
		public int sceneInt;
	} 
}
