using TMPro;
using UnityEngine;

public class AdditionalBallData : MonoBehaviour
{
    public BallDatas Ball;
    public float Price;
    public float GoldMultiplier;
    public TMP_Text PriceText;
    public TMP_Text MoneyText;

    public void ChangeBalls(int ID)
    {
        if (!ShopScript.instance.OwnBall[ID] && ShopScript.instance.OwnMoney >= Price)
        {
            ShopScript.instance.OwnMoney -= Price;
            MoneyText.text = ShopScript.instance.OwnMoney.ToString() + "$";
            PriceText.text = "Own";
            ShopScript.instance.OwnBall[ID] = true;
        }
        else if(ShopScript.instance.OwnBall[ID])
        {
            LevelManager.Instance.ChangeBalls(Ball);
        }
    }
}
