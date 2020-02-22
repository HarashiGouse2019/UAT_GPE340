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

    [Header("IK Targets")]
    public Transform RightHandIKTarget;
    public Transform LeftHandIKTarget;

    [Header("IK Hint Targets")]
    public Transform RightHandIKHintTarget;
    public Transform LeftHandIKHintTarget;

    public void Shoot() { }
    public void Reload() { }
    public void Use() { }
}
