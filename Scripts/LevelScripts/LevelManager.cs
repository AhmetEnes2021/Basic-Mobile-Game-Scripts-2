using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.VFX;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    #region PublicValues
    [Header("Values")]
    [Tooltip("How much will green zone goes up")]
    [SerializeField] [Range(0, 1)] float MoveOffset;
    [Tooltip("Smooth movement duration value")]
    [SerializeField] [Range(0, 5)] float MoveSmoothness;
    [Tooltip("How long can we stay out of green zone. If we stay more than this value, green will move to back \n(50 = 1sec)")]
    [SerializeField] [Range(50 , 6000)] float OutOfGreenTime;
    [SerializeField] Vector3 TubeSpawnOffset;
    [SerializeField] Vector3 TubeSpawnAngleOffset;
    [SerializeField] Vector3 BallSpawnOffset;
    [SerializeField] float SpawnXOffset;
    #endregion
    #region PublicLists
    [Space]
    [Header("Beginning")]
    public BallDatas[] DefaultSCBalls;
    public TubeDatas[] DefaultSCTubes;
    public GameObject[] DefaultTubes;
    public GameObject[] DefaultBalls;
    [Space]
    [Header("Lists")]
    public List<BallDatas> Balls = new List<BallDatas>();
    public List<Transform> BallObjects = new List<Transform>();
    public List<TubeDatas> Tubes = new List<TubeDatas>();
    public List<Transform> TubeObjects = new List<Transform>();
    public List<Transform> GreenZones = new List<Transform>();
    #endregion
    #region PublicOthers
    [Space]
    [Header("Other")]
    public ShopScript _ShopScript;
    public SaveSystem _SaveSystem;

    #endregion
    #region Private
    private List<Vector3> LastPosition = new List<Vector3>();
    [HideInInspector] public bool isInGreen;
    BlowScript blowScript;
    [HideInInspector] public int currentLevel;
    int Timer, Timer2, greenTimer;
    bool End,IsReturning;
    float _BallSpawnOffset;
    float _TubeSpawnXOffset;
    [HideInInspector]public SaveDatas _SaveDatas = new SaveDatas();
    bool MoveBack;

    #endregion
    private void OnApplicationQuit()
    {
        _SaveDatas.MaxStamina = blowScript.maxStamina;
        _SaveDatas.OwnLevels = _ShopScript.OwnLevels;
        _SaveDatas.OwnBall = _ShopScript.OwnBall;
        _SaveDatas.OwnMoney = _ShopScript.OwnMoney;
        _SaveDatas.StaminaCost = _ShopScript.StaminaCost;
        _SaveDatas.Balls = Balls;
        _SaveDatas.Tubes = Tubes;
        _SaveSystem.SaveJSON("GameDatas", _SaveDatas);

    }
    private void Awake()
    {
        Instance = this;
        blowScript = GetComponent<BlowScript>();
        if (_SaveSystem.LoadJSON("GameDatas", ref _SaveDatas))
        {
            _SaveSystem.LoadJSON("GameDatas", ref _SaveDatas);
            if (_SaveDatas.Balls[0].BallPrefab != Balls[0].BallPrefab)
            {
                Balls = _SaveDatas.Balls;
                SpawnBalls();
            }
            if (_SaveDatas.Tubes[0].TubePrefab != Tubes[0].TubePrefab)
            {
                Tubes = _SaveDatas.Tubes;
                SpawnTubes();
            }
            blowScript.maxStamina = _SaveDatas.MaxStamina;
        }
        
        _ShopScript.CurrentMoneyMultiplier = Balls[0].GoldMultiplier;
        BallObjects[currentLevel].gameObject.GetComponent<BallScript>().CanAddStamina = true;
        currentLevel = 0;
        for (int i = 0; i < GreenZones.Count; i++)
        {
            GreenZones[i].transform.gameObject.SetActive(false);
        }
        GreenZones[currentLevel].gameObject.SetActive(true);
        GreenZones[currentLevel].localPosition =
            new Vector3(GreenZones[currentLevel].localPosition.x,
            Tubes[currentLevel].greenMinHeight,
            GreenZones[currentLevel].localPosition.z);
    }

    private void SpawnTubes()
    {
        for (int i = 0; i < DefaultTubes.Length; i++)
        {
            Destroy(DefaultTubes[i]);
        }
        _TubeSpawnXOffset = TubeSpawnOffset.x;
        TubeObjects.Clear();
        GreenZones.Clear();
        for (int i = 0; i < Tubes.Count; i++)
        {
            Transform NewTube = Instantiate(Tubes[i].TubePrefab, new Vector3(_TubeSpawnXOffset, TubeSpawnOffset.y + Tubes[i].TubeSpawnY, TubeSpawnOffset.z), Quaternion.Euler(TubeSpawnAngleOffset)).transform;
            GreenZones.Add(NewTube.GetChild(0));
            GreenZones[i].transform.localPosition = new Vector3(GreenZones[i].transform.localPosition.x, Tubes[i].greenMinHeight, GreenZones[i].transform.localPosition.z);
            TubeObjects.Add(NewTube);
            _TubeSpawnXOffset += SpawnXOffset;
        }
        for (int i = 0; i < GreenZones.Count; i++)
        {
            GreenZones[i].gameObject.SetActive(false);
        }
        GreenZones[currentLevel].gameObject.SetActive(true);
       
    }
    void SpawnBalls()
    {
        for (int i = 0; i < DefaultBalls.Length; i++)
        {
            Destroy(DefaultBalls
                [i]);
        }
        _BallSpawnOffset = BallSpawnOffset.x;
        BallObjects.Clear();
        for (int i = 0; i < Tubes.Count; i++)
        {
            Transform NewBall = Instantiate(Balls[i].BallPrefab, new Vector3(_BallSpawnOffset, BallSpawnOffset.y, BallSpawnOffset.z), Quaternion.Euler(-90f, 0f, 0f)).transform;
            BallObjects.Add(NewBall);
            _BallSpawnOffset += SpawnXOffset;
        }
        BallObjects[0].gameObject.GetComponent<BallScript>().CanAddStamina = true;
        ShopScript.instance.CurrentMoneyMultiplier = Balls[0].GoldMultiplier;
    }

    public void ChangeBalls(BallDatas _Ball)
    {
        for (int i = 0; i < BallObjects.Count; i++)
        {
            Destroy(BallObjects[i].gameObject);
        }
        Balls.Clear();
        BallObjects.Clear();
        _BallSpawnOffset = BallSpawnOffset.x;
        for (int i = 0; i < Tubes.Count; i++)
        {
            Balls.Add(_Ball);
            Transform NewBall = Instantiate(Balls[i].BallPrefab, new Vector3(_BallSpawnOffset , BallSpawnOffset.y , BallSpawnOffset.z), Quaternion.Euler(-90f, 0f, 0f)).transform;
            BallObjects.Add(NewBall);
            _BallSpawnOffset += SpawnXOffset;
        }
        BallObjects[0].gameObject.GetComponent<BallScript>().CanAddStamina = true;
        ShopScript.instance.CurrentMoneyMultiplier = _Ball.GoldMultiplier;
    }
    public void ChangeTubes(List<TubeDatas> _Tubes)
    {
        for (int i = 0; i < TubeObjects.Count; i++)
        {
            Destroy(TubeObjects[i].gameObject);
        }
        Tubes.Clear();
        TubeObjects.Clear();
        GreenZones.Clear();
        _TubeSpawnXOffset = TubeSpawnOffset.x;
        for (int i = 0; i < _Tubes.Count; i++)
        {
            Tubes.Add(_Tubes[i]);
            Transform NewTube = Instantiate(Tubes[i].TubePrefab, new Vector3(_TubeSpawnXOffset, TubeSpawnOffset.y + _Tubes[i].TubeSpawnY, TubeSpawnOffset.z), Quaternion.Euler(TubeSpawnAngleOffset)).transform;
            GreenZones.Add(NewTube.GetChild(0));
            GreenZones[i].transform.localPosition= new Vector3(GreenZones[i].transform.localPosition.x, _Tubes[i].greenMinHeight, GreenZones[i].transform.localPosition.z);
            TubeObjects.Add(NewTube);
            _TubeSpawnXOffset += SpawnXOffset;
        }
        for (int i = 0; i < GreenZones.Count; i++)
        {
            GreenZones[i].gameObject.SetActive(false);
        }
        GreenZones[currentLevel].gameObject.SetActive(true);
    }
    private void FixedUpdate()
    {
        if (blowScript.currentStamina > 0 && !IsReturning)
        {
            Timer++;
            blowScript.ControlDistance();
            if (!End && currentLevel + 1 == Tubes.Count && GreenZones[currentLevel].localPosition.y >= Tubes[currentLevel].greenMaxHeight)
            {
                End = true;
            }
            if (!MoveBack && !End && GreenZones[currentLevel].localPosition.y >= Tubes[currentLevel].greenMaxHeight && Timer >= 50 && currentLevel < Tubes.Count)
            {
                NextTube();
            }
            else if (!End && isInGreen && Timer >= 50 && currentLevel < Tubes.Count && GreenZones[currentLevel].localPosition.y <= Tubes[currentLevel].greenMaxHeight)
            {
                MoveBack = false;
                greenTimer = 0;
                Timer = 0;
                LastPosition.Add(GreenZones[currentLevel].localPosition);
                GreenZones[currentLevel].DOLocalMoveY(GreenZones[currentLevel].localPosition.y + MoveOffset, MoveSmoothness);
                AddMoney(1f);
            }
            else if (End && isInGreen && GreenZones[currentLevel].localPosition.y > Tubes[currentLevel].greenMinHeight && Timer >= 50)
            {
                MoveBack = false;
                Timer = 0;
                AddMoney(10f);
            }
            else if (!isInGreen && GreenZones[currentLevel].localPosition.y > Tubes[currentLevel].greenMinHeight)
            {
                if (greenTimer < OutOfGreenTime)
                {
                    greenTimer++;
                }
                if (greenTimer >= OutOfGreenTime)
                {
                    Timer2++;
                    if (currentLevel >= 0 && Timer2 >= 50)
                    {
                        Timer2 = 0;
                        StartCoroutine("BackMove");

                    }
                    if (End)
                    {
                        End = false;
                    }

                }
            }
        }
        else if(!IsReturning)
        {
            End = false;
            IsReturning = true;
            StartCoroutine("ReturnToBeginning");
        }
    }
    void AddMoney(float _Value)
    {
        ShopScript.instance.OwnMoney += _Value * ShopScript.instance.CurrentMoneyMultiplier;
        ShopScript.instance.MoneyText.text = ShopScript.instance.OwnMoney.ToString() + "$";
    }
    IEnumerator BackMove()
    {
        MoveBack = true;
        GreenZones[currentLevel].DOLocalMove(LastPosition[LastPosition.Count - 1], MoveSmoothness);
        LastPosition.RemoveAt(LastPosition.Count - 1);
        yield return new WaitForSeconds(MoveSmoothness);
        if (GreenZones[currentLevel].localPosition.y == Tubes[currentLevel].greenMinHeight && currentLevel > 0)
        {
            GreenZones[currentLevel].gameObject.SetActive(false);
            currentLevel--;
            yield return new WaitForSeconds(0.25f);
            GreenZones[currentLevel].gameObject.SetActive(true);
            GreenZones[currentLevel].position = new Vector3(GreenZones[currentLevel].position.x, GreenZones[currentLevel].position.y - 0.2f , GreenZones[currentLevel].position.z);
        }
    }
    IEnumerator ReturnToBeginning()
    {

        for (int i = GreenZones.Count-1; i >= 0; i--)
        {
            LastPosition.Clear();
            GreenZones[currentLevel].DOLocalMoveY(Tubes[currentLevel].greenMinHeight, 0.5f);
            yield return new WaitForSeconds(0.5f);
            if (GreenZones[currentLevel].localPosition.y <= Tubes[currentLevel].greenMinHeight && currentLevel > 0)
            {
                GreenZones[currentLevel].gameObject.SetActive(false);
                currentLevel--;
                GreenZones[currentLevel].gameObject.SetActive(true);
            }
        }
        IsReturning = false;
        
    }
    void NextTube()
    {
        currentLevel++;
        if (currentLevel < Tubes.Count && !IsReturning)
        {
            GreenZones[currentLevel - 1].gameObject.SetActive(false);
            GreenZones[currentLevel].gameObject.SetActive(true);
            GreenZones[currentLevel].localPosition =
                new Vector3(GreenZones[currentLevel].localPosition.x,
                Tubes[currentLevel].greenMinHeight,
                GreenZones[currentLevel].localPosition.z);
            BallObjects[currentLevel - 1].GetComponent<BallScript>().CanAddStamina = false;
            BallObjects[currentLevel].GetComponent<BallScript>().CanAddStamina = true;
        }
       
    }
    
}