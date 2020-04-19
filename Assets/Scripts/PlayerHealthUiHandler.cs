using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUiHandler : MonoBehaviour
{
    private static PlayerHealthUiHandler uiHandler;

    /*This will reference our player, and grab it's damageableObj script.
     This will then display the player's health on the screen*/

    static PlayerPawn playerPawn;

    //Reference our UI and damageableObj
    static Slider S_PLAYERHEALTH;
    static DamageableObj damageableObj;

    void Awake()
    {
        uiHandler = this;
    }

    // Start is called before the first frame update
    public static void RunHealthUI()
    {
        playerPawn = FindObjectOfType<PlayerPawn>();
        damageableObj = playerPawn.GetComponent<DamageableObj>();
        S_PLAYERHEALTH = uiHandler.GetComponent<Slider>();

        uiHandler.StartCoroutine(UpdateHealthUI());
    }

    static IEnumerator UpdateHealthUI()
    {
        while(true)
        {
            //Every update, change the slider value for the health
            S_PLAYERHEALTH.value = damageableObj.GetObjHealthVal() / 100f;

            yield return new WaitForEndOfFrame();
        }
    }

    public static Slider GETUI() => S_PLAYERHEALTH;
}
