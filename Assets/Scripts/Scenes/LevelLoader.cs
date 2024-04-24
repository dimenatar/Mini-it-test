using Cysharp.Threading.Tasks;
using Scriptables;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Zenject;

namespace Scenes
{
	public class LevelLoader
	{
		private ScenesConfig _scenesConfig;

		[Inject]
		public LevelLoader(ScenesConfig scenesConfig)
		{
			_scenesConfig = scenesConfig;
		}

		public async Task LoadSceneAsync(SceneType sceneType)
		{
			var scene = _scenesConfig.Scenes[sceneType];
			var loading = SceneManager.LoadSceneAsync(scene);
			await loading;
		}

		public void LoadScene(SceneType sceneType)
		{
			var scene = _scenesConfig.Scenes[sceneType];
			SceneManager.LoadScene(scene);
		}
	}
}