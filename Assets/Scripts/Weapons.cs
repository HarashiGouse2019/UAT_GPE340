using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;

public abstract class Weapons : PickUps, IPickable
{
    public string weaponName;
    public bool claimed = false;
    public Pawn claimedBy;
    public Vector3 startInThisPosition;
    public Transform ammoSource;
    public enum WeaponType
    {
        Melee,
        Projectile,
        Projectile_Automatic
    }
    public WeaponType weaponType;

    [Header("Weapon Stats")] public WeaponStats weaponStats;

    [Header("Weapon Sound")] public string weaponSound;

    [Header("How Much Ammo is Left???")]
    public int ammoAmount;

    public GameObject bulletPrefab;
    
    [Header("Are there rounds inside?")]
    public bool areRoundsInside = true;

    [Header("Is it a Pistol")]
    public bool isPistol = true;

    [Header("IK Targets")]
    public Transform RightHandIKTarget;
    public Transform LeftHandIKTarget;
    public List<Transform> OtherIKTargets = new List<Transform>();

    [Header("IK Hint Targets")]
    public Transform RightHandIKHintTarget;
    public Transform LeftHandIKHintTarget;
    public List<Transform> OtherIKHintTargets = new List<Transform>();

    [Header("Weapon Icon")] public Sprite weaponIcon;

    protected bool canShoot = true;

    protected float time;

    protected ObjectPooler weaponObjectPooler;

    public virtual void Awake()
    {
        weaponObjectPooler = GetComponent<ObjectPooler>();
    }

    public virtual void Start()
    {
        //The gun is already loaded with bullets
        if (areRoundsInside && weaponStats != null)
            ammoAmount = weaponStats.weaponAmmoCapacity;
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
            //We'll be now using a raycast to shoot! Luckily, this is simple to do.

            Ray ray = new Ray(ammoSource.position, ammoSource.right);

            //Then we need to create a RaycastHit object
            RaycastHit hitInfo;

            //This will be a test whether our ray is showing/working
            Debug.DrawRay(ammoSource.position, ammoSource.right * weaponStats.weaponRange, Color.red, 2f);

            //We'll now create a muzzle effect
            GameObject muzzelFlash = weaponObjectPooler.GetMember("MuzzleFlash");

            if (!muzzelFlash.gameObject.activeInHierarchy)

            {
                muzzelFlash.gameObject.SetActive(true);

                muzzelFlash.transform.position = ammoSource.position;

                muzzelFlash.transform.rotation = ammoSource.rotation;

                muzzelFlash.GetComponent<ParticleSystem>().Play();

                canShoot = false;

                ammoAmount -= 1;

                claimedBy.weaponHandler.UpdateAmmoProperties();

                AudioManager.Instance.PlayAudio(weaponSound, _oneShot: true);

            }

            //When we show, we want to update the ammo info;
            PlayerAmmoTextHandler.UpdateAmmoTextUI();

            //Now if we hit something....
            if (Physics.Raycast(ray, out hitInfo, weaponStats.weaponRange) && hitInfo.collider.gameObject != claimedBy.gameObject)
                OnHit(hitInfo);
        }
        else
            claimedBy.weaponHandler.CallForReload();

    }

    public virtual void OnHit(RaycastHit _hitInfo)
    {
        if (_hitInfo.collider.gameObject != gameObject)
        {
            //Randomize between the weapons min and max damage
            var damageInflicted = Rand.Range(weaponStats.weaponMinDamage, weaponStats.weaponMaxDamage);

            //Then we had crit damage (if we ever get one). 
            damageInflicted += ApplyCritDamage();

            //Find the health component
            try
            {
                _hitInfo.collider.GetComponentInParent<Pawn>().GetDamageableObj().TakeDamage(damageInflicted);
            }
            catch { }
            
        }
    }

    public virtual float ApplyCritDamage()
    {
        //If a crit has been successful, it'll return
        //the weapon's critical damage value

        //chances of Critical will be converted to a percent.
        var chancesForCritical = (weaponStats.weaponCritChance / 100);

        //Now we get a value between 0 and one, since
        //chances of Critical is in a percentage.
        var getChance = Rand.Range(0f, 1f);

        /*If we are smaller than our equal to the chancesForCritical,
         we return the weaponCritValue.*/
        if (getChance <= chancesForCritical)
        {
            //We get a value between our critical min and max
            var criticalDamageInflicted = Rand.Range(weaponStats.weaponCritMinDamage, weaponStats.weaponCritMaxDamage);

            //And then we return it.
            return criticalDamageInflicted;
        }

        //Otherwise, we get no critical bonus
        return 0f;
    }

    public virtual void OnEquip()
    {
        transform.localPosition = startInThisPosition;
        PlayerAmmoTextHandler.UpdateAmmoTextUI();
    }


    public virtual void Reload()
    {
        //If someone actually has this weapon
        if (claimedBy != null)
        {
            //We want to check how much ammmo we need to put in first.
            //It'll basically be a transfer between the weaponHandler's packOfAmmo to ammoLeft
            //which then that value will be set back to it's ammoCapacity
            int ammoToFill = weaponStats.weaponAmmoCapacity - ammoAmount; //This will get use the value of how much we need to into our weapon

            //Now we get how much packs of ammo we have left
            int packsOfAmmoLeft = claimedBy.weaponHandler.packOfAmmoLeft;

            //Now we "Send the packOfAmmo" to the "ammoLeft"
            //I'll iterate it this time.
            for (int ammoAdded = 0; ammoAdded < ammoToFill; ammoAdded++)
            {
                if (packsOfAmmoLeft > 0)
                {
                    packsOfAmmoLeft -= 1;
                    claimedBy.weaponHandler.packOfAmmoLeft = packsOfAmmoLeft;
                    ammoAmount += 1;
                    claimedBy.weaponHandler.UpdateAmmoProperties();
                }
                else
                {
                    claimedBy.weaponHandler.UpdateAmmoProperties();
                    return;
                }
            }

            // We also want to update the ammo info when we reload
            PlayerAmmoTextHandler.UpdateAmmoTextUI();
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
        if(time >= (1 / weaponStats.weaponRoundsPer))
            SetNextRound();
            
    }
}
