using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public bool CanAddStamina;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("TubeBottom") && BlowScript.instance.currentStamina < BlowScript.instance.maxStamina && CanAddStamina)
        {
            BlowScript.instance.AddStamina();
        }
    }

}
