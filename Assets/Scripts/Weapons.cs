using System.Collections;
using UnityEngine;

public abstract class Weapons : PickUps, IPickable
{
    public string weaponName;
    public bool claimed = false;
    public Pawn claimedBy;
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

    protected bool canShoot = true;

    protected float time;

    protected ObjectPooler weaponBulletPooler;

    public virtual void Start()
    {
        //The gun is already loaded with bullets
        if (areRoundsInside)
            ammoAmount = ammoCapacity;

        weaponBulletPooler = GetComponent<ObjectPooler>();
    }

    public override void Update()
    {
        /*I want to check what kind of weapon that the designer wants this to be,
         so that I can use the correct functions in order to perfectly simulate the weapon.
         */

        if (Input.GetMouseButtonUp(0))
            SetNextRound();

        if (!canShoot && weaponType == WeaponType.Projectile_Automatic)
            Recoil();

        if (!claimed)
            base.Update();
    }

    public virtual void Shoot()
    {
        if (canShoot && !OutOfAmmo() && claimedBy != null)
        {
            
            Bullet bullet = weaponBulletPooler.GetMember(claimedBy.weaponHandler.ammoKind).GetComponent<Bullet>();
            if (!bullet.gameObject.activeInHierarchy)
            {
                bullet.gameObject.SetActive(true);
                bullet.transform.position = ammoSource.position;
                bullet.transform.rotation = ammoSource.rotation;
                bullet.Release(bullet.physicalProperty, claimedBy);
                canShoot = false;
                ammoAmount--;
                claimedBy.weaponHandler.UpdateAmmoProperties();
            }
        }
        else
            claimedBy.weaponHandler.CallForReload();

    }
    public virtual void Reload()
    {
        //If someone actually has this weapon
        if (claimedBy != null)
        {
            //We want to check how much ammmo we need to put in first.
            //It'll basically be a transfer between the weaponHandler's packOfAmmo to ammoLeft
            //which then that value will be set back to it's ammoCapacity
            int ammoToFill = ammoCapacity - ammoAmount; //This will get use the value of how much we need to into our weapon

            //Now we "Send the packOfAmmo" to the "ammoLeft"
            ammoAmount += ammoToFill;
            claimedBy.weaponHandler.packOfAmmoLeft -= ammoToFill;

            claimedBy.weaponHandler.UpdateAmmoProperties();
        }
    }
    public virtual void OnUse() { }

    public virtual void Drop()
    {
        claimed = false;
        claimedBy = null;
    }

    public override void OnPickUp(GameObject _source)
    {
        try
        {
            //Getting our pawn based on value parameter
            Pawn pawn = _source.GetComponent<Pawn>();

            //Add this weapon to pawn's weaponHandler
            pawn.weaponHandler.weapons.Add(this);

            //We're contantly picking up our weapon
            claimed = true;

            //For the player, we can use this to update UI and change values
            //Specific to who pick it up
            claimedBy = pawn;

            //Add the weapon to the pawn's ItemDrop list
            claimedBy.itemDrop.AddItem(gameObject);

            //Particularly for the player; updating Ui;
            claimedBy.weaponHandler.UpdateAmmoProperties();

            //Passed name to shoot from object pooler
            claimedBy.weaponHandler.ammoKind = bulletPrefab.name;

            //We don't want a full mess of errors, so set inactive
            gameObject.SetActive(false);

            //We know that this is our weapon.
            //And we can then use inverse kinematics by changing the parent of 
            //the weapon object.
            if (!isPistol)
                gameObject.transform.SetParent(pawn.weaponAttachedPoint);
            else
                gameObject.transform.SetParent(pawn.weaponAttachedPointPistol);

            //Now! If it happens to be an AI, we'll have the equip this weapon immediately.
            CheckIfAI(claimedBy, this);
        }
        catch { }
    }

    public virtual void CheckIfAI(Pawn _obj, Weapons _weapon)
    {
        if (_obj.GetComponent<AIState>() != null)
        {
            _obj.weaponHandler.EquipWeapon(_weapon);

            //Change the target destination for it, because we assume that it was
            //after a weapon.
            Transform newTarget = FindObjectOfType<PlayerPawn>().transform;
            _obj.controller.target = newTarget;
        }
    }

    //For Projectile Type Weapons
    public virtual void SetNextRound()
    {
        time = (int)reset;
        canShoot = true;
    }

    public virtual bool OutOfAmmo() => ammoAmount < 1;

    public virtual void Recoil()
    {
        time += Time.deltaTime;
        if(time >= (1 / roundsPerSec))
            SetNextRound();
            
    }
}
