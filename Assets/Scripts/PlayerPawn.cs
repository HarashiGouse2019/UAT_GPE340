using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPawn : Pawn
{

    //Our movement function for our player pawn
    public override void Move(Vector3 worldDirectionToMove)
    {
        //Calculate our direction based on our rotation (so 0,0,1 becomes our forward)
        Vector3 directionToMove = transform.TransformDirection(worldDirectionToMove);

        //Actually move
        if (charController.isGrounded) vSpeed = 0;
        else
        {
            vSpeed -= gravity * Time.deltaTime;
            directionToMove.y = vSpeed;
        }
        charController.Move(directionToMove * Time.deltaTime);
    }
}
