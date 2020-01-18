using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    //Animator
    Animator playerAnimator;

    //Animator booleans to control our animations
    public bool isLeftStrifing;
    public bool isRightStrifing;
    public bool isRolling;

    //Our timer float for dodging primarily
    float time;

    //A const reset value
    const uint reset = 0;

    // Start is called before the first frame update
    public override void Start()
    {
        //We have to get our reference from CharactorController, our player animation
        //and our player pawn
        pawn.charController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        pawn = GetComponent<PlayerPawn>();
        base.Start();
    }

    private void Update()
    {
        //If we start rolling, have it last for 1 second
        //since the whole rolling animation last that long
        if (isRolling)
            RollDuration(1f);
    }

    private void FixedUpdate()
    {
        //Our input will be either ASWD or Arrow Keys or Joystick
        //We get values between 1 and -1 for both horizonal and vertical movement
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * Time.fixedDeltaTime;

        //We have the player roll with the middle mouse button
        bool rollInput = Input.GetMouseButtonDown(2);

        if (rollInput)
            SetRollingTo(true);

        //This makes sure that our magnitude is not higher than 4
        input = Vector3.ClampMagnitude(input, 4f);

        //input = transform.InverseTransformDirection(input);

        //Our input is amplified by our movement speed
        //since input utimately only had values of 1, 0, and -1
        //by multiplying with our pawn movement, we can go more than 1 and still
        //account for the signs.
        input *= pawn.movementSpeed;
       

        //Every frame, we assign our values into the player animator
        playerAnimator.SetFloat("Horizontal", input.x);
        playerAnimator.SetFloat("Vertical", input.z);
        playerAnimator.SetBool("isRolling", isRolling);

        //We create another vector that takes the x and z coordinates of our player
        //and moves every frame
        Vector3 directionToMove = new Vector3(input.x, 0f, input.z);
        pawn.Move(directionToMove);
    }

    //Set if our player is rolling or not
    public void SetRollingTo(bool state)
    {
        isRolling = state;
    }

    //How long it takes before we get out of our rolling animation
    public void RollDuration(float duration)
    {
        time += Time.deltaTime;
        if (time >= duration)
        {
            SetRollingTo(false);
            time = reset;
        }
    }
}
