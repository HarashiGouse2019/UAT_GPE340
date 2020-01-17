using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    //Animator
    Animator playerAnimator;
    public bool isLeftStrifing;
    public bool isRightStrifing;



    // Start is called before the first frame update
    public override void Start()
    {
        pawn.charController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        pawn = GetComponent<PlayerPawn>();
        base.Start();
    }

    private void FixedUpdate()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * Time.fixedDeltaTime;

        input = Vector3.ClampMagnitude(input, 4f);

        input *= pawn.movementSpeed;

        playerAnimator.SetFloat("Horizontal", input.x);
        playerAnimator.SetFloat("Vertical", input.z);


        Vector3 directionToMove = new Vector3(input.x, 0f, input.z);
        pawn.Move(directionToMove);
    }


}
