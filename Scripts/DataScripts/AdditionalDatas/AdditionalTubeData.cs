using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdditionalTubeData : MonoBehaviour
{
    public List<TubeDatas> TubePrefabs = new List<TubeDatas>();
    public float Price;
    public TMP_Text PriceText;
    public TMP_Text MoneyText;

    public void ChangeTubes(int ID)
    {
        if (!ShopScript.instance.OwnLevels[ID] && ShopScript.instance.OwnMoney > Price)
        {
            ShopScript.instance.OwnMoney -= Price;
            MoneyText.text = ShopScript.instance.OwnMoney.ToString() + "$";
            PriceText.text = "Own";
            ShopScript.instance.OwnLevels[ID] = true;
        }
        else if (ShopScript.instance.OwnLevels[ID])
        {
            LevelManager.Instance.ChangeTubes(TubePrefabs);
        }
    }
}
