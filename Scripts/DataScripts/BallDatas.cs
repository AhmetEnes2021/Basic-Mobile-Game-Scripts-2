using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Ball", menuName = "Create New Ball")]
public class BallDatas : ScriptableObject
{
    public string BallName;
    public GameObject BallPrefab;
    [Tooltip("For Shop")]public Sprite BallImage;
    [Tooltip("How much stamina we will lost in green zone")] public float StaminaDrainInGreen;
    [Tooltip("How much stamina we will lost when we close to the green zone")] public float StaminaDrainNearGreen;
    [Tooltip("How much stamina we will lost out of green zone")] public float StaminaDrainOutOfGreen;
    [Tooltip("If we want to add something more")] public float Price;
    [Tooltip("If we want to add something more")] public float GoldMultiplier;
}
