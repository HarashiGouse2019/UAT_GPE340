using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    CharacterController playerCharController;
    PlayerPawn pawn;
    public float movementSpeed, rotationSpeed;

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
        playerAnimator.SetBool("isMoving", isMoving);
        playerAnimator.SetBool("isLeftStrifing", isLeftStrifing);
        playerAnimator.SetBool("isRightStrifing", isRightStrifing);

        Vector3 directionToMove = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            directionToMove += Vector3.forward * movementSpeed * Time.deltaTime;
            isMoving = true;
        }

        if (Input.GetKey(KeyCode.S))
        {
            directionToMove += Vector3.back* movementSpeed * Time.deltaTime;
            isMoving = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            directionToMove += Vector3.left * movementSpeed * Time.deltaTime;
            isLeftStrifing = true;
            isRightStrifing = !isLeftStrifing;
        }

        if (Input.GetKey(KeyCode.D))
        {
            directionToMove += Vector3.right * movementSpeed * Time.deltaTime;
            isRightStrifing = true;
            isLeftStrifing = !isRightStrifing;
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            isMoving = false;

            if (Input.GetKeyUp(KeyCode.A))
                isLeftStrifing = false;
            if (Input.GetKeyUp(KeyCode.D))
                isRightStrifing = false;
        }

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
        playerCharController.SimpleMove(directionToMove * Time.deltaTime);
    }
}
