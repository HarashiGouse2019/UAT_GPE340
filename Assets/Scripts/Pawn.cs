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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public virtual void Move(Vector3 worldDirectionToMove) { }
}
