using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAdditional : MonoBehaviour
{
    [Header("Select Additional")]
    public bool AddTube;
    public bool AddBall;


    [Header("Tube Values")]
    public float LevelPrice;
    public string LevelName;
    public List<TubeDatas> Tubes;
    
    
    [Space]

    [Header("Ball Values")]
    public BallDatas[] Ball;
    [Space]

    [Tooltip("Don't touch this things")]
    [Header("Other")]
    public TMPro.TMP_Text MoneyText;
    public GameObject BallShopPanel;
    public GameObject TubeShopPanel;
    public GameObject BallPanel;
    public GameObject TubePanel;
    public ShopScript ShopScript;
}
