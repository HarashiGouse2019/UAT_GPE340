using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaUiHandler : MonoBehaviour
{
    private static PlayerStaminaUiHandler uiHandler;

    /*This will handle the blue bar that is the Stamina.
     It'll just simply take the stamina up or down. That's about it.*/
    private static PlayerPawn playerPawn;
    private static Slider S_PLAYERSTAMINA;

    private static ExhaustibleObj exhaustibleObj;

    void Awake()
    {
        uiHandler = this;
    }


    // Start is called before the first frame update
    public static void RunStaminaUI()
    {
        playerPawn = FindObjectOfType<PlayerPawn>();
        S_PLAYERSTAMINA = uiHandler.GetComponent<Slider>();
        exhaustibleObj = playerPawn.GetComponent<ExhaustibleObj>();

        uiHandler.StartCoroutine(UpdateStaminaUI());
    }


    static IEnumerator UpdateStaminaUI()
    {
        while (true)
        {
            S_PLAYERSTAMINA.value = exhaustibleObj.GetObjStamina() / 100f;

            yield return new WaitForEndOfFrame();
        }
    }
}
