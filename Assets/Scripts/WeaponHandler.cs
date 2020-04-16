using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public List<Weapons> weapons = new List<Weapons>();

    public Weapons equippedWeapon = null;

    public int ammoLeft { get; set; }
    public int packOfAmmoLeft { get; set; } = 1000;

    public string ammoKind;

    public bool isEquipped;

    public int weaponIndex;

    public Pawn pawn;

    public bool needReload = false;

    const uint reset = 0;


    public void ChangeToNextWeapon()
    {
        /*Because equipping a weapon validates the kind of weapon we're equipping to,
         this is why we unequip first, change our index, check rather the value exceeds the min 
         and max total of weapons on the player, and THEN we equip to that weapon.*/
        UnequipWeapon();
        weaponIndex++;

        MonitorWeaponIndex();
        EquipWeapon(weapons[weaponIndex]);

        UpdateAmmoProperties();
    }

    public void ChangeToPreviousWeapon()
    {
        /*Because equipping a weapon validates the kind of weapon we're equipping to,
         this is why we unequip first, change our index, check rather the value exceeds the min 
         and max total of weapons on the player, and THEN we equip to that weapon.*/
        UnequipWeapon();
        weaponIndex--;
        
        MonitorWeaponIndex();
        EquipWeapon(weapons[weaponIndex]);

        UpdateAmmoProperties();
    }

    public Weapons EquipWeapon(Weapons weapon)
    {
        equippedWeapon = weapon;
        isEquipped = true;

        if (!equippedWeapon.gameObject.activeInHierarchy)
        {
            equippedWeapon.enabled = true;
            equippedWeapon.gameObject.SetActive(true);

            UpdateAmmoProperties();

            //Check if it's a pistol or rifle
            if (!equippedWeapon.isPistol)
            {
                equippedWeapon.GetComponent<Transform>().SetParent(pawn.weaponAttachedPoint);
                equippedWeapon.GetComponent<Transform>().position = pawn.weaponAttachedPoint.position;
                equippedWeapon.GetComponent<Transform>().rotation = pawn.weaponAttachedPoint.rotation;
                equippedWeapon.OnEquip();
                return weapon;
            }

            if (equippedWeapon.isPistol)
            {
                equippedWeapon.GetComponent<Transform>().SetParent(pawn.weaponAttachedPointPistol);
                equippedWeapon.GetComponent<Transform>().position = pawn.weaponAttachedPointPistol.position;
                equippedWeapon.GetComponent<Transform>().rotation = pawn.weaponAttachedPointPistol.rotation;
                equippedWeapon.OnEquip();
                return weapon;
            }
            PlayerAmmoTextHandler.UpdateAmmoTextUI();

        }
        return null;
    }

    public void UnequipWeapon()
    {
        equippedWeapon.gameObject.SetActive(false);
        equippedWeapon = null;
        isEquipped = false;

        PlayerAmmoTextHandler.UpdateAmmoTextUI();
    }

    public void MonitorWeaponIndex()
    {
        if (weaponIndex > weapons.Count - 1)
        {
            weaponIndex = (int)reset;
            return;
        }

        if (weaponIndex < (int)reset)
        {
            weaponIndex = weapons.Count - 1;
            return;
        }
    }

    public void UpdateAmmoProperties()
    {
        if (equippedWeapon != null)
            ammoLeft = equippedWeapon.ammoAmount;
        else
            ammoLeft = (int)reset;

        PlayerAmmoTextHandler.UpdateAmmoTextUI();
    }

    public void CallForReload()
    {
        needReload = true;
    }
}
