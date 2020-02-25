using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannaquinTargetPawn : Pawn
{
    public override void Start()
    {
        if (weaponHandler.weapons != null)
        {
            Weapons weapon = weaponHandler.EquipWeapon(weaponHandler.weapons[0]);
            weapon.claimed = true;
        }
        else
            controller.agent.SetDestination(FindObjectOfType<SquareMight>().gameObject.transform.position);
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "bullet")
            GetComponent<DamageableObj>().TakeDamage(collision.gameObject.GetComponent<Bullet>().bulletDamage);
    }

    public override void OnSpawn()
    {
        GetComponent<MannaquinTargetController>().target = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
