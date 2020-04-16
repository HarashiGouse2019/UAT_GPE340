using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponIconHandler : MonoBehaviour
{
    private static WeaponIconHandler Instance;

    //So the name and icon is all that we care about. We'll have a weapon that will be displayed,
    //and then we'll have a function SetIconInfo and update the UI passed on the equippedWeapon on the player
    public Image IMG_ICONSLOT;

    //The text mesh pro that will be the weapon name
    public TextMeshProUGUI TMP_WEAPONNAME;

    //We'll get information from pawn of what weapon they have equipped
    public static Weapons currentWeaponInfo { get; private set; }

    private Color initialColor;


    void Awake()
    {
        Instance = this;
        initialColor = IMG_ICONSLOT.color;
        SetIconInfo();
    }

    public static void SetIconInfo(Weapons _weaponInfo = null)
    {
        Instance.InvokeIconInfo(_weaponInfo);
    }

    private void InvokeIconInfo(Weapons _weaponInfo = null)
    {
        if (_weaponInfo == null)
        {
            currentWeaponInfo = null;
            TMP_WEAPONNAME.text = "";
            IMG_ICONSLOT.sprite = null;

            //This will prevent from seeing a white box
            IMG_ICONSLOT.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
            return;
        }

        currentWeaponInfo = _weaponInfo;

        //First, we update the name or the TextMeshPro
        TMP_WEAPONNAME.text = currentWeaponInfo.weaponName;

        //Then we will update the icon image that is being shown
        IMG_ICONSLOT.sprite = currentWeaponInfo.weaponIcon;

        //bring back the alpha.
        IMG_ICONSLOT.color = new Color(initialColor.r, initialColor.g, initialColor.b, 1f);
    }
}
