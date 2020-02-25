using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour, ISpawnable
{
    public CharacterController charController;

    public Transform weaponAttachedPoint;

    public Transform weaponAttachedPointPistol;

    public Controller controller;

    public WeaponHandler weaponHandler;

    //Movement
    public float movementSpeed;
    public float rollingSpeed = 1;

    public float vSpeed = 0f;
    public float gravity = 90f;

    public Animator animator;

    protected DamageableObj health;

    public virtual void Awake()
    {
        health = gameObject.AddComponent<DamageableObj>();
        weaponHandler = gameObject.AddComponent<WeaponHandler>();
        weaponHandler.pawn = this;
    }

    public virtual void Start()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void Move(Vector3 worldDirectionToMove) { }

    public virtual void OnSpawn()
    {

        animator = GetComponent<Animator>();
    }

    protected virtual void OnAnimatorIK(int indexLayer)
    {
        if (!weaponHandler.equippedWeapon)
            return;

        SetRightArmIK();

        SetLeftArmIK();

    }

    public virtual void SetRightArmIK()
    {
        if (weaponHandler.equippedWeapon.RightHandIKTarget)
        {
            animator.SetIKPosition(AvatarIKGoal.RightHand, weaponHandler.equippedWeapon.RightHandIKTarget.position);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKRotation(AvatarIKGoal.RightHand, weaponHandler.equippedWeapon.RightHandIKTarget.rotation);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);

            animator.SetIKHintPosition(AvatarIKHint.RightElbow, weaponHandler.equippedWeapon.RightHandIKHintTarget.position);
            animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 1f);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);

            animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 0f);
        }
    }

    public virtual void SetLeftArmIK()
    {
        if (weaponHandler.equippedWeapon.LeftHandIKTarget)
        {
            animator.SetIKPosition(AvatarIKGoal.LeftHand, weaponHandler.equippedWeapon.LeftHandIKTarget.position);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, weaponHandler.equippedWeapon.LeftHandIKTarget.rotation);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);

            animator.SetIKHintPosition(AvatarIKHint.LeftElbow, weaponHandler.equippedWeapon.LeftHandIKHintTarget.position);
            animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 1f);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);

            animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 0f);
        }
    }

    public virtual GameObject GetGameObject() { return null; }

    public virtual GameObject FindGameObjectOnLayer(string _layerName)
    {
        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allGameObjects)
        {
            if (obj.layer == SortingLayer.GetLayerValueFromName(_layerName))
                return obj;
        }
        return null;
    }
}
