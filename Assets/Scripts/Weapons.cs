using UnityEngine;

public abstract class Weapons : PickUps, IPickable
{
    public string weaponName;
    public bool claimed = false;
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

    protected bool canShoot;

    public virtual void OnShoot() { }
    public virtual void OnReload() { }
    public virtual void OnUse() { }

    public override void OnPickUp(GameObject _source) { }
}
