using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour, ISpawnable, IDropable
{
    public CharacterController charController;

    public Transform weaponAttachedPoint;

    public Transform weaponAttachedPointPistol;

    public Controller controller;

    public WeaponHandler weaponHandler;

    public ItemDrop itemDrop;

    public bool droppedSomething = false;

    //Movement
    public float movementSpeed;
    public float rollingSpeed = 1;

    public float vSpeed = 0f;
    public float gravity = 90f;

    public Animator animator;

    [SerializeField]
    protected DamageableObj health;

    protected bool isDead = false;

    public virtual void Awake()
    {
        if (health == null) health = gameObject.AddComponent<DamageableObj>();
        if (weaponHandler == null) weaponHandler = gameObject.AddComponent<WeaponHandler>();
        if (itemDrop == null) itemDrop = gameObject.AddComponent<ItemDrop>();
        weaponHandler.pawn = this;
    }

    public virtual void Start()
    {

    }

    public virtual void Update()
    {

    }

    public virtual DamageableObj GetDamageableObj() => health;

    public virtual void Move(Vector3 worldDirectionToMove) { }

    public virtual void OnSpawn()
    {

        animator = GetComponent<Animator>();
        isDead = false;
    }

    public virtual void OnDrop()
    {
        
        itemDrop.DropAllItems();
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

    public virtual void EnableRagDoll()
    {
        try
        {
            if (weaponHandler.equippedWeapon != null)
            {
                weaponHandler.equippedWeapon.Drop();
                weaponHandler.UnequipWeapon();
            }
            
            animator.enabled = false;
            charController.enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            isDead = true;
        }
        catch { }
    }

    public virtual bool IsDead() => isDead;

    public virtual void DestroySelf()
    {
        Destroy(gameObject);
    }
}
