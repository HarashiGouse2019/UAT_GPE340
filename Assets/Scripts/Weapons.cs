using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapons : MonoBehaviour
{
    public string weaponName;
    public enum WeaponType
    {
        Melee,
        Projectile,
        Projectile_Automatic
    }
    public WeaponType weaponType;

    [Header("IK Targets")]
    public Transform RightHandIKTarget;
    public Transform LeftHandIKTarget;

    [Header("IK Hint Targets")]
    public Transform RightHandIKHintTarget;
    public Transform LeftHandIKHintTarget;

    public virtual void OnShoot() { }
    public virtual void OnReload() { }
    public virtual void OnUse() { }
}
