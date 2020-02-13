using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public string weaponName;
    public enum WeaponType
    {
        Melee,
        Projectile
    }
    public WeaponType weaponType;

    public void Shoot() { }
    public void Reload() { }
    public void Use() { }

}
