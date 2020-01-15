using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    CharacterController playerCharController;
    PlayerPawn pawn;
    public float horMoveSpeed, verMoveSpeed;

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
        float horSpeed = horMoveSpeed * Input.GetAxis("Horizontal");
        float verSpeed = verMoveSpeed * Input.GetAxis("Vertical");

        playerAnimator.SetFloat("Horizontal", horSpeed);
        playerAnimator.SetFloat("Vertical", verSpeed);

        Vector3 directionToMove = new Vector3(horSpeed, 0f, verSpeed);
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
