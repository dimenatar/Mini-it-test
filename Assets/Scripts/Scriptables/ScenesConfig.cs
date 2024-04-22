using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(order = 68)]
public class ScenesConfig : ScriptableObject
{
    [SerializeField] private SerializedDictionary<SceneType, string> _sceneByStage;

    public SerializedDictionary<SceneType, string> SceneByStage => _sceneByStage;
}
