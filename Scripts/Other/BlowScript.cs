using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BlowScript : MonoBehaviour
{
    [Header("Stamina")]
    public TMP_Text staminaText;
    public float maxStamina;
    public float currentStamina;


    [Header("Other")]
    [SerializeField] [Range(0,2f)] float[] JumpForce;
    LevelManager levelManager;
    float distance;

    public static BlowScript instance;

    private void Awake()
    {
        instance = this;
        currentStamina = maxStamina;
        staminaText.text = "Stamina : " + currentStamina.ToString();
        levelManager = gameObject.GetComponent<LevelManager>();
        for (int i = 0; i < levelManager.BallObjects.Count; i++)
        {
            levelManager.BallObjects[i].gameObject.GetComponent<BallScript>().CanAddStamina = false;
        }
        
    }

    private void FixedUpdate()
    {
        if (levelManager.currentLevel < levelManager.Tubes.Count && levelManager.GreenZones.Count > 0)
        {
            distance = -((levelManager.BallObjects[levelManager.currentLevel].position.y - levelManager.GreenZones[levelManager.currentLevel].position.y));
        }
    }

    public void Blow()
    {
        if (currentStamina > 0)
        {
            for (int i = 0 ; i < levelManager.BallObjects.Count; i++)
            {
                levelManager.BallObjects[i].GetComponent<Rigidbody>().velocity = new Vector3(0, JumpForce[i], 0);
            }
            for (int i = 0; i <= levelManager.currentLevel; i++)
            {
                _ControlDistance(2, 3, 4);
            }
        }
    }

    private void _ControlDistance(float _Value1, float _Value2, float _Value3)
    {
        for (int i = 0; i < levelManager.BallObjects.Count; i++)
        {
            if (distance <= 0 && distance >= -0.15 || distance >= 0 && distance <= 0.15)
            {
                Stamina(_Value1);
            }
            else if (distance > -0.15 && distance <= -0.30 || distance > 0.15 && distance <= 0.30)
            {
                Stamina(_Value2);
            }
            else
            {
                Stamina(_Value3);
            }
        }
    }
    public void ControlDistance()
    {
        for (int i = 0; i < levelManager.BallObjects.Count; i++)
        {
            if (distance <= 0 && distance >= -0.25 || distance >= 0 && distance <= 0.25)
            {
                levelManager.isInGreen = true;
            }
            else if (distance > -0.25 && distance <= -0.50 || distance > 0.25 && distance <= 0.50)
            {
                levelManager.isInGreen = false;
            }
            else
            {
                levelManager.isInGreen = false;
            }
        }
    }
    public void Stamina(float _value)
    {
        currentStamina -= _value;
        ControlStamina();
        staminaText.text = "Stamina : " + currentStamina;
    }
    void ControlStamina()
    {
        if (currentStamina < 0) currentStamina = 0;
        else if (currentStamina > maxStamina) currentStamina = maxStamina;
    }

    public void AddStamina()
    {
        currentStamina++;
        staminaText.text = "Stamina : " + BlowScript.instance.currentStamina;
    }
}
