using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUiHandler : MonoBehaviour
{
    /*This will reference our player, and grab it's damageableObj script.
     This will then display the player's health on the screen*/

    PlayerPawn playerPawn;

    //Reference our UI and damageableObj
    Slider S_PLAYERHEALTH;
    DamageableObj damageableObj;

    // Start is called before the first frame update
    void Start()
    {
        playerPawn = FindObjectOfType<PlayerPawn>();
        damageableObj = playerPawn.GetComponent<DamageableObj>();
        S_PLAYERHEALTH = GetComponent<Slider>();

        StartCoroutine(UpdateHealthUI());
    }

    IEnumerator UpdateHealthUI()
    {
        while(true)
        {
            S_PLAYERHEALTH.value = damageableObj.GetObjHealthVal() / 100f;

            yield return new WaitForEndOfFrame();
        }
    }
}
