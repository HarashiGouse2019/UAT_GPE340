using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    public CharacterController charController;

    public List<Weapons> weapons = new List<Weapons>();

    public Weapons equippedWeapon = null;

    public bool isEquipped;

    public Transform weaponAttachedPoint;

    //Movement
    public float movementSpeed;
    public float rollingSpeed = 1;

    public float vSpeed = 0f;
    public float gravity = 90f;

    public virtual void Move(Vector3 worldDirectionToMove) { }

    public virtual void EquipWeapon(Weapons weapon)
    {
        equippedWeapon = Instantiate(weapon) as Weapons;
        equippedWeapon.GetComponent<Transform>().SetParent(weaponAttachedPoint);
        equippedWeapon.GetComponent<Transform>().localPosition = weaponAttachedPoint.localPosition;
        equippedWeapon.GetComponent<Transform>().localRotation = weaponAttachedPoint.localRotation;
    }
}
