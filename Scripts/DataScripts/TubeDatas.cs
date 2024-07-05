using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "New Tube", menuName = "Create New Tube")]
public class TubeDatas : ScriptableObject
{
    [Tooltip("Tube Name")] public string TubeName;
    [Tooltip("Tube Prefab to be used")] public GameObject TubePrefab;
    [Tooltip("Tube will spawn at this Y position")] public float TubeSpawnY;
    [Tooltip("Changes maximum height of green zone")][Range(-5f, 5f)] public float greenMinHeight;
    [Tooltip("Changes minimum height of green zone")][Range(-5f, 5f)] public float greenMaxHeight;
}
