using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannaquinTargetPawn : Pawn
{
    public override void Start()
    {
        if (weaponHandler.weapons.Count != 0)
        {
            Weapons weapon = weaponHandler.EquipWeapon(weaponHandler.weapons[0]);
            weapon.claimed = true;
            weapon.claimedBy = this;
            weaponHandler.ammoKind = weapon.bulletPrefab.name;
        }
    }

    public override void OnSpawn()
    {
        GetComponent<MannaquinTargetController>().target = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
