using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : Controller
{
    //Animator booleans to control our animations
    public bool isLeftStrifing;
    public bool isRightStrifing;
    public bool isRolling;

    //Check if the player can shoot
    public bool canShoot;
    
    //Our timer float for dodging primarily
    float time;

    //A const reset value
    const uint reset = 0;

    //Player Exhaustible Object
    ExhaustibleObj exhaustibleObj;

    //Unity Events
    [Header("Control Events")]
    [SerializeField] private List<UnityEvent> unityEvents;

    // Start is called before the first frame update
    public override void Start()
    {
        //We have to get our reference from CharactorController, our player animation
        //and our player pawn
        pawn = GetComponent<PlayerPawn>();
        pawn.charController = GetComponent<CharacterController>();

        exhaustibleObj = GetComponent<ExhaustibleObj>();

        base.Start();
    }

    private void Update()
    {
        //If we start rolling, have it last for 1 second
        //since the whole rolling animation last that long
        if (isRolling)
            RollDuration(1f);

        //The current weapon previously on
        int weaponIndex = pawn.weaponHandler.weaponIndex;

        //Equip a weapon
        if (Input.GetKeyDown(KeyCode.Space) && pawn.weaponHandler.weapons.Count != 0)
        {
            if (pawn.weaponHandler.isEquipped == false)
            {
                pawn.weaponHandler.EquipWeapon(pawn.weaponHandler.weapons[weaponIndex]);
                pawn.weaponHandler.weapons[weaponIndex].GetComponent<Weapons>().claimed = true;
                pawn.animator.SetBool("isEquipped", pawn.weaponHandler.isEquipped);
                GameCameraControls.Instance.GoToArmedPosition();
            }
            else
            {
                pawn.weaponHandler.UnequipWeapon();
                pawn.animator.SetBool("isEquipped", pawn.weaponHandler.isEquipped);
                GameCameraControls.Instance.GoToUnArmedPosition();
            }
        }

        //Change Weapons
        if (Input.GetKeyDown(KeyCode.R) && pawn.weaponHandler.weapons.Count != 0)
        {
            /*We have to do something a big different if we want to change weapons.
             Firstly, if we were to change weapons without Equpping and UnEqupping, there's
             a likely chance of having a pistol be where a rifle weapon is suppose to spawn, etc.
             
             So we unequip as usual, then we change the weaponIndex in our WeaponHandler.
             After the index has been changed, we equip our weapon like normal. This allows checking if it's
             a pistol, a rifle, or a melee weapon without any confusion. Which is why we go through this step.*/
            if (pawn.weaponHandler.isEquipped == true)
                pawn.weaponHandler.ChangeToNextWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Q) && pawn.weaponHandler.weapons.Count != 0)
        {
            /*We have to do something a big different if we want to change weapons.
             Firstly, if we were to change weapons without Equpping and UnEqupping, there's
             a likely chance of having a pistol be where a rifle weapon is suppose to spawn, etc.
             
             So we unequip as usual, then we change the weaponIndex in our WeaponHandler.
             After the index has been changed, we equip our weapon like normal. This allows checking if it's
             a pistol, a rifle, or a melee weapon without any confusion. Which is why we go through this step.*/
            if (pawn.weaponHandler.isEquipped == true)
                pawn.weaponHandler.ChangeToPreviousWeapon();
        }
    }

    private void FixedUpdate()
    {
        //Our input will be either ASWD or Arrow Keys or Joystick
        //We get values between 1 and -1 for both horizonal and vertical movement
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * Time.fixedDeltaTime;

        //We have the player roll with the middle mouse button
        bool rollInput = Input.GetMouseButtonDown(2);

        //The shoot button
        bool shootInput = Input.GetMouseButton(0);

        if (rollInput && isRolling == false)
        {
            SetRollingTo(true);
            exhaustibleObj.UseObjStamina(10f);
            StartCoroutine(exhaustibleObj.CheckForRecovery());
        }

        if (shootInput && pawn.weaponHandler.equippedWeapon != null)
        {
            pawn.weaponHandler.equippedWeapon.OnShoot();
        }

        //This makes sure that our magnitude is not higher than 4
        input = Vector3.ClampMagnitude(input, 4f);

        //Our input is amplified by our movement speed
        //since input utimately only had values of 1, 0, and -1
        //by multiplying with our pawn movement, we can go more than 1 and still
        //account for the signs.
        input *= pawn.movementSpeed;


        //Every fixed frame, we assign our values into the player animator
        pawn.animator.SetFloat("Horizontal", input.x);
        pawn.animator.SetFloat("Vertical", input.z);
        pawn.animator.SetBool("isRolling", isRolling);

        //We create another vector that takes the x and z coordinates of our player
        //and moves every fixed frame
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
