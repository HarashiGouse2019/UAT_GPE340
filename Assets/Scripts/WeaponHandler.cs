using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public List<Weapons> weapons = new List<Weapons>();

    public Weapons equippedWeapon = null;

    public bool isEquipped;

    public int weaponIndex;

    public Pawn pawn;

    const uint reset = 0;


    public void ChangeToNextWeapon()
    {
        UnequipWeapon();
        weaponIndex++;
        MonitorWeaponIndex();
        EquipWeapon(weapons[weaponIndex]);
    }

    public void ChangeToPreviousWeapon()
    {
        UnequipWeapon();
        weaponIndex--;
        MonitorWeaponIndex();
        EquipWeapon(weapons[weaponIndex]);
    }

    public Weapons EquipWeapon(Weapons weapon)
    {
        equippedWeapon = weapon;
        isEquipped = true;
        if (!equippedWeapon.gameObject.activeInHierarchy)
        {
            equippedWeapon.enabled = true;
            equippedWeapon.gameObject.SetActive(true);

            //Check if it's a pistol or rifle
            if (!equippedWeapon.isPistol)
            {
                equippedWeapon.GetComponent<Transform>().SetParent(pawn.weaponAttachedPoint);
                equippedWeapon.GetComponent<Transform>().position = pawn.weaponAttachedPoint.position;
                equippedWeapon.GetComponent<Transform>().rotation = pawn.weaponAttachedPoint.rotation;
                return weapon;
            }

            if (equippedWeapon.isPistol)
            {
                equippedWeapon.GetComponent<Transform>().SetParent(pawn.weaponAttachedPointPistol);
                equippedWeapon.GetComponent<Transform>().position = pawn.weaponAttachedPointPistol.position;
                equippedWeapon.GetComponent<Transform>().rotation = pawn.weaponAttachedPointPistol.rotation;
                return weapon;
            }
        }
        return null;
    }

    public void UnequipWeapon()
    {
        equippedWeapon.gameObject.SetActive(false);
        equippedWeapon = null;
        isEquipped = false;
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
}
