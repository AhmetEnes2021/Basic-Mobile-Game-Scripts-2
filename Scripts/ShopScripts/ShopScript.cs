using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopScript : MonoBehaviour
{
    public static ShopScript instance;

    [Header("GameObjects")]
    [SerializeField] GameObject ShopPanel;
    [SerializeField] TMP_Text StaminaCostText;
    [SerializeField] Button StaminaButton;
    public TMP_Text MoneyText;
    [SerializeField] GameObject BallPanel;
    [SerializeField] GameObject TubePanel;
    public List<TMP_Text> BallTexts = new List<TMP_Text>();
    public List<TMP_Text> TubeTexts = new List<TMP_Text>();

    [Space]
    [Header("Values")]
    public float OwnMoney;
    public float StaminaCost;
    [SerializeField] float IncreaseStaminaCost;
    [SerializeField] float AddMaxStamina;

    [HideInInspector] public float CurrentMoneyMultiplier;
    public bool[] OwnLevels;
    public bool[] OwnBall;
    public SaveSystem _SaveSystem;
    private void Awake()
    {
        instance = this;
        if (_SaveSystem.LoadJSON("GameDatas", ref LevelManager.Instance._SaveDatas))
        {
            _SaveSystem.LoadJSON("GameDatas", ref LevelManager.Instance._SaveDatas);
            OwnLevels = LevelManager.Instance._SaveDatas.OwnLevels;
            OwnBall = LevelManager.Instance._SaveDatas.OwnBall;
            OwnMoney = LevelManager.Instance._SaveDatas.OwnMoney;
            StaminaCost = LevelManager.Instance._SaveDatas.StaminaCost;
            for (int i = 0; i < OwnBall.Length; i++)
            {
                if (OwnBall[i])
                {
                    BallTexts[i].text = "Own";
                }
            }
            for (int i = 0; i < OwnLevels.Length; i++)
            {
                if (OwnLevels[i])
                {
                    TubeTexts[i].text = "Own";
                }
            }
        }
       

        MoneyText.text = OwnMoney.ToString() + "$";
        StaminaCostText.text = StaminaCost.ToString() + "$";
    }
    public void OpenShop()
    {
        BlowScript.instance.enabled = !BlowScript.instance.enabled;
        ShopPanel.SetActive(true);
    }
    public void CloseShop()
    {
        BlowScript.instance.enabled = !BlowScript.instance.enabled;
        ShopPanel.SetActive(false);
    }
    public void SellStamina()
    {
        if (OwnMoney > StaminaCost)
        {
            OwnMoney -= StaminaCost;
            BlowScript.instance.maxStamina += AddMaxStamina;
            StaminaCost += IncreaseStaminaCost;
            StaminaCostText.text = StaminaCost.ToString() + "$";
            MoneyText.text = OwnMoney.ToString() + "$";
        }
    }
    private void Update()
    {
        CheckMoneyForStamina();
    }
    public void CheckMoneyForStamina()
    {
        if (OwnMoney > StaminaCost)
        {
            StaminaButton.GetComponent<Button>().enabled = true;
            StaminaButton.GetComponent<Image>().color = Color.white;
        }
        else if(StaminaButton.GetComponent<Button>().enabled)
        {
            StaminaButton.GetComponent<Button>().enabled = false;
            StaminaButton.GetComponent<Image>().color = Color.gray;
        }
    }
    public void OpenBallPanel()
    {
        BallPanel.SetActive(true);
        TubePanel.SetActive(false);
    }
    public void OpenTubePanel()
    {
        TubePanel.SetActive(true);
        BallPanel.SetActive(false);
    }
}
