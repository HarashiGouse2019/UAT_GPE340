using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaUiHandler : MonoBehaviour
{
    /*This will handle the blue bar that is the Stamina.
     It'll just simply take the stamina up or down. That's about it.*/
    private PlayerPawn playerPawn;
    private Slider S_PLAYERSTAMINA;

    private ExhaustibleObj exhaustibleObj;
    // Start is called before the first frame update
    void Start()
    {
        playerPawn = FindObjectOfType<PlayerPawn>();
        S_PLAYERSTAMINA = GetComponent<Slider>();
        exhaustibleObj = playerPawn.GetComponent<ExhaustibleObj>();

        StartCoroutine(UpdateStaminaUI());
    }


    IEnumerator UpdateStaminaUI()
    {
        while (true)
        {
            S_PLAYERSTAMINA.value = exhaustibleObj.GetObjStamina() / 100f;

            yield return new WaitForEndOfFrame();
        }
    }
}
