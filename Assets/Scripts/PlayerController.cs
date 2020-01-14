using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    CharacterController playerCharController;
    Transform playerTransform;
    PlayerPawn pawn;
    public float movementSpeed, rotationSpeed;

    public float vSpeed = 0f;
    public float gravity = 90f;

    //Animator
    Animator playerAnimator;
    bool isMoving;


    // Start is called before the first frame update
    void Start()
    {
        playerCharController = GetComponent<CharacterController>();
        playerTransform = GetComponent<Transform>();
        playerAnimator = GetComponent<Animator>();
        pawn = GetComponent<PlayerPawn>();
        base.Start();
    }

    private void FixedUpdate()
    {
        playerAnimator.SetBool("isMoving", isMoving);

        Vector3 directionToMove = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            directionToMove += Vector3.forward * movementSpeed * Time.deltaTime;
            isMoving = true;
        }

        if (Input.GetKey(KeyCode.S))
        {
            directionToMove -= Vector3.forward * movementSpeed * Time.deltaTime;
            isMoving = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            directionToMove += Vector3.left * movementSpeed * Time.deltaTime;
            isMoving = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            directionToMove -= Vector3.left * movementSpeed * Time.deltaTime;
            isMoving = true;
        }

        if (Input.GetKeyUp(KeyCode.W) && Input.GetKeyUp(KeyCode.S) && Input.GetKeyUp(KeyCode.A) && Input.GetKeyUp(KeyCode.D))
            isMoving = false;

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
