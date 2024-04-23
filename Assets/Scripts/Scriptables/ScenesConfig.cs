using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(order = 68)]
public class ScenesConfig : ScriptableObject
{
    [SerializeField] private SerializedDictionary<SceneType, string> _scenes;

    public SerializedDictionary<SceneType, string> Scenes => _scenes;
}
