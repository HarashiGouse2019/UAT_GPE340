using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    public CharacterController charController;

    //Movement
    public float movementSpeed;
    public float rollingSpeed = 1;

    public float vSpeed = 0f;
    public float gravity = 90f;

    public virtual void Move(Vector3 worldDirectionToMove) { }
}
