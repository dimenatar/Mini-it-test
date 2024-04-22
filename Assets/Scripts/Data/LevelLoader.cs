using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Zenject;

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
        var scene = _scenesConfig.SceneByStage[sceneType];
        var loading = SceneManager.LoadSceneAsync(scene);
        await loading;
    }

    public void LoadScene(SceneType sceneType)
    {
        var scene = _scenesConfig.SceneByStage[sceneType];
        SceneManager.LoadScene(scene);
    }
}