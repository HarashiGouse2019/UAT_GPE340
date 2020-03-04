using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerAmmoTextHandler : MonoBehaviour
{
    private static PlayerAmmoTextHandler uiHandler;

    /*This will handle the blue bar that is the Stamina.
   It'll just simply take the stamina up or down. That's about it.*/
    private static PlayerPawn playerPawn;
    private static TextMeshProUGUI TMP_AMMO;

    private static WeaponHandler weaponHandler;
    void Awake()
    {
        uiHandler = this;
    }


    // Start is called before the first frame update
    public static void RunAmmoTextUI()
    {
        playerPawn = FindObjectOfType<PlayerPawn>();
        TMP_AMMO = uiHandler.GetComponent<TextMeshProUGUI>();
        weaponHandler = playerPawn.GetComponent<WeaponHandler>();

        uiHandler.StartCoroutine(UpdateAmmoText());
    }


    static IEnumerator UpdateAmmoText()
    {
        while (true)
        {
            TMP_AMMO.text = weaponHandler.ammoLeft + " / " + weaponHandler.packOfAmmoLeft;

            yield return new WaitForEndOfFrame();
        }
    }
}
