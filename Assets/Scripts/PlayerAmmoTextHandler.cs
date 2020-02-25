using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerAmmoTextHandler : MonoBehaviour
{
    /*This will handle the blue bar that is the Stamina.
   It'll just simply take the stamina up or down. That's about it.*/
    private PlayerPawn playerPawn;
    private TextMeshProUGUI TMP_AMMO;

    private WeaponHandler weaponHandler;
    // Start is called before the first frame update
    void Start()
    {
        playerPawn = FindObjectOfType<PlayerPawn>();
        TMP_AMMO = GetComponent<TextMeshProUGUI>();
        weaponHandler = playerPawn.GetComponent<WeaponHandler>();

        StartCoroutine(UpdateAmmoText());
    }


    IEnumerator UpdateAmmoText()
    {
        while (true)
        {
            TMP_AMMO.text = weaponHandler.ammoLeft + " / " + weaponHandler.packOfAmmoLeft;

            yield return new WaitForEndOfFrame();
        }
    }
}
