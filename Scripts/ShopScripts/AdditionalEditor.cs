#if UNITY_EDITOR
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(NewAdditional))]
public class AdditionalEditor : Editor
{
    string LevelDetailText;
    public override void OnInspectorGUI()
    {
        NewAdditional NAdd = (NewAdditional)target;
        if (GUILayout.Button("Add New Additional"))
        {

            if (NAdd.AddTube && NAdd.AddBall)
            {
                NAdd.AddTube = !NAdd.AddTube;
                AddNewTube();
                NAdd.AddBall = !NAdd.AddBall;
                AddNewBall();
            }
            else if (NAdd.AddTube)
            {
                NAdd.AddTube = !NAdd.AddTube;
                AddNewTube();
            }
            else if (NAdd.AddBall)
            {
                NAdd.AddBall = !NAdd.AddBall;
                AddNewBall();
            }
        }
        base.OnInspectorGUI();
    }
    public void AddNewBall()
    {
        NewAdditional NewAdd = (NewAdditional)target;
        if (NewAdd.Ball == null)
        {
            EditorUtility.DisplayDialog("Ball Can Not Be Null", "You Must Select The Ball.", "Close");
        }
        else
        {
            for (int i = 0; i < NewAdd.Ball.Length; i++)
            {
                Transform NewBall = Instantiate(NewAdd.BallPanel).transform;
                NewBall.parent = NewAdd.BallShopPanel.transform;
                NewBall.gameObject.GetComponent<AdditionalBallData>().MoneyText = NewAdd.MoneyText;
                NewBall.GetChild(1).GetChild(0).GetComponent<Image>().sprite = NewAdd.Ball[i].BallImage;
                NewBall.gameObject.GetComponent<AdditionalBallData>().Ball = NewAdd.Ball[i];
                NewBall.gameObject.GetComponent<AdditionalBallData>().Price = NewAdd.Ball[i].Price;
                NewBall.gameObject.GetComponent<AdditionalBallData>().GoldMultiplier = NewAdd.Ball[i].GoldMultiplier;
                NewBall.GetChild(2).GetChild(0).GetComponent<TMP_Text>().text = NewBall.gameObject.GetComponent<AdditionalBallData>().Ball.BallName;
                NewBall.GetChild(3).GetChild(0).GetComponent<TMP_Text>().text = "Price : " + NewBall.gameObject.GetComponent<AdditionalBallData>().Price.ToString() + "$";
                NewBall.gameObject.GetComponent<AdditionalBallData>().PriceText = NewBall.GetChild(3).GetChild(0).GetComponent<TMP_Text>();
                NewBall.GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = "Gold Multiplier : " + NewBall.gameObject.GetComponent<AdditionalBallData>().GoldMultiplier.ToString() + "X";
                NewAdd.ShopScript.BallTexts.Add(NewBall.GetChild(3).GetChild(0).GetComponent<TMP_Text>());
            }
            ResetBall(NewAdd);

        }
        
        
    }

    public void AddNewTube()
    {
        
        NewAdditional NewAdd = (NewAdditional)target;
        if (NewAdd.Tubes.Count == 0)
        {
            EditorUtility.DisplayDialog("Tubes Can Not Be Null", "You Must Select The Tubes.", "Close");
        }
        else
        {
            Transform NewTube = Instantiate(NewAdd.TubePanel).transform;
            NewTube.parent = NewAdd.TubeShopPanel.transform;
            NewTube.gameObject.GetComponent<AdditionalTubeData>().MoneyText = NewAdd.MoneyText;
            NewTube.GetChild(2).GetChild(0).GetComponent<TMP_Text>().text = NewAdd.LevelPrice.ToString();
            NewTube.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = NewAdd.LevelName;
            NewTube.gameObject.GetComponent<AdditionalTubeData>().PriceText = NewTube.GetChild(2).GetChild(0).GetComponent<TMP_Text>();
            for (int i = 0; i < NewAdd.Tubes.Count; i++)
            {
                NewTube.gameObject.GetComponent<AdditionalTubeData>().TubePrefabs.Add(NewAdd.Tubes[i]);
                NewTube.gameObject.GetComponent<AdditionalTubeData>().Price = NewAdd.LevelPrice;
                if (i == NewAdd.Tubes.Count-1)
                {
                    LevelDetailText = LevelDetailText + NewTube.gameObject.GetComponent<AdditionalTubeData>().TubePrefabs[i].TubeName;
                }
                else
                {
                    LevelDetailText = LevelDetailText + NewTube.gameObject.GetComponent<AdditionalTubeData>().TubePrefabs[i].TubeName + " - ";
                } 
            }
            NewTube.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = LevelDetailText;
            LevelDetailText = "";
            NewAdd.ShopScript.TubeTexts.Add(NewTube.GetChild(2).GetChild(0).GetComponent<TMP_Text>());
            ResetLevel(NewAdd);
        }
    }
    public void ResetBall(NewAdditional NewAdd)
    {
        NewAdd.Ball = null;
    }
    public void ResetLevel(NewAdditional NewAdd)
    {
        NewAdd.Tubes.Clear();
        NewAdd.LevelPrice = 0;
        NewAdd.LevelName = "";
    }
}
#endif