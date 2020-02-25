using System.Collections;
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

    [Header("For Projectile Weapons Only")]
    public GameObject bulletPrefab;
    public Transform ammoSource;
    public int ammoCapacity;
    public int ammoAmount;

    [Header("Automatic Weapons Only")]
    public float roundsPerSec = 10;

    [Header("Are there rounds inside?")]
    public bool areRoundsInside = true;

    [Header("Is it a Pistol")]
    public bool isPistol = true;

    [Header("IK Targets")]
    public Transform RightHandIKTarget;
    public Transform LeftHandIKTarget;

    [Header("IK Hint Targets")]
    public Transform RightHandIKHintTarget;
    public Transform LeftHandIKHintTarget;

    protected bool canShoot;

    IEnumerator recoil;

    public virtual void Start()
    {
        recoil = Recoil();

        //The gun is already loaded with bullets
        if(areRoundsInside)
            ammoAmount = ammoCapacity;
    }

    public override void Update()
    {
        /*I want to check what kind of weapon that the designer wants this to be,
         so that I can use the correct functions in order to perfectly simulate the weapon.
         */

        switch (weaponType)
        {
            case WeaponType.Projectile:
                if (Input.GetMouseButtonUp(0))
                    SetNextRound();
                break;
            case WeaponType.Projectile_Automatic:
                if (Input.GetMouseButton(0))
                {
                    canShoot = false;
                    StartCoroutine(recoil);
                } 
                break;
            default:
                break;
        }

        if (!claimed)
            base.Update();
    }

    public virtual void OnShoot() {
        if (canShoot && !OutOfAmmo())
        {
            Bullet bullet = Instantiate(bulletPrefab).GetComponent<Bullet>();
            bullet.transform.position = ammoSource.position;
            bullet.transform.rotation = ammoSource.rotation;
            bullet.Release(bullet.physicalProperty);
            canShoot = false;
            ammoAmount--;
        }
    }
    public virtual void OnReload() { }
    public virtual void OnUse() { }

    public override void OnPickUp(GameObject _source)
    {
        Pawn pawn = _source.GetComponent<Pawn>();
        pawn.weaponHandler.weapons.Add(this);
        
        claimed = true;
        
        gameObject.SetActive(false);

        if (!isPistol)
            gameObject.transform.SetParent(pawn.weaponAttachedPoint);
        else
            gameObject.transform.SetParent(pawn.weaponAttachedPointPistol);
    }

    //For Projectile Type Weapons
    public virtual void SetNextRound()
    {
        canShoot = true;
    }

    public virtual bool OutOfAmmo() => ammoAmount < 1;

    public virtual IEnumerator Recoil()
    {
        canShoot = false;
        yield return new WaitForSeconds(1/roundsPerSec);

        canShoot = true;
    }
}
