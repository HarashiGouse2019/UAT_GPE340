using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    CharacterController playerCharController;
    PlayerPawn pawn;
    public float movementSpeed;

    public float vSpeed = 0f;
    public float gravity = 90f;

    //Animator
    Animator playerAnimator;
    bool isMoving;
    public bool isLeftStrifing;
    public bool isRightStrifing;



    // Start is called before the first frame update
    void Start()
    {
        playerCharController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        pawn = GetComponent<PlayerPawn>();
        base.Start();
    }

    private void FixedUpdate()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * Time.fixedDeltaTime;

        input = Vector3.ClampMagnitude(input, 4f);

        input *= movementSpeed;

        playerAnimator.SetFloat("Horizontal", input.x);
        playerAnimator.SetFloat("Vertical", input.z);


        Vector3 directionToMove = new Vector3(input.x, 0f, input.z);
        Move(directionToMove);
    }

    void Move(Vector3 worldDirectionToMove)
    {
        //Calculate our direction based on our rotation (so 0,0,1 becomes our forward)
        Vector3 directionToMove = pawn.transform.TransformDirection(worldDirectionToMove);

        //Actually move
        if (playerCharController.isGrounded) vSpeed = 0;
        else
        {
            vSpeed -= gravity * Time.deltaTime;
            directionToMove.y = vSpeed;
        }
        playerCharController.Move(directionToMove * Time.deltaTime);
    }
}
