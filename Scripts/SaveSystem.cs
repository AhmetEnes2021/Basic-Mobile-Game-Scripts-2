using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{

    public void SaveJSON(string fileName, object recordObj)
    {
        string strOutput = JsonUtility.ToJson(recordObj);
        File.WriteAllText(Application.persistentDataPath + "/" + fileName + ".json", strOutput);
    }
    public bool LoadJSON<T>(string fileName, ref T recordObj)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        if (File.Exists(path))
        {
            string contents = File.ReadAllText(path);

            Type recordType = recordObj.GetType();

            recordObj = (T)JsonUtility.FromJson(contents, recordType);

            return true;
        }
        else
        {
            return false;
        }
    }
}
[System.Serializable]
public class SaveDatas
{
    public float MaxStamina;
    public bool[] OwnLevels;
    public bool[] OwnBall;
    public float OwnMoney;
    public float StaminaCost;
    public List<BallDatas> Balls = new List<BallDatas>();
    public List<TubeDatas> Tubes = new List<TubeDatas>();
}
