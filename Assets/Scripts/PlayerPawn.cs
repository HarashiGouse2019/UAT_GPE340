using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPawn : Pawn
{
    //This is unique to the player only, where the camera will rotate if they dies.
    public ObjectPooler deathSpotPooler;

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

    public override void OnSpawn()
    {
        PlayerStaminaUiHandler.RunStaminaUI();
        PlayerHealthUiHandler.RunHealthUI();
        base.OnSpawn();
    }

    public override void EnableRagDoll()
    {
        if (!isDead)
        {
            isDead = true;

            if (weaponHandler.equippedWeapon != null)
            {
                weaponHandler.equippedWeapon.Drop();
                weaponHandler.UnequipWeapon();
                WeaponIconHandler.SetIconInfo();
            }

            var deathSpot = deathSpotPooler.GetMember("DeathSpot");

            deathSpot.SetActive(true);

            animator.enabled = false;

            charController.enabled = false;

            GetComponent<Rigidbody>().isKinematic = true;

            GameManager.Instance.DecrementPlayerLives();

            if (GameManager.GetPlayerLives() <= 0)
            {
                GetComponentInParent<SpawnerHandler>().End();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        #region Capture Flag
        //Detect a flag if close to it.
        FlagID currentFlag = other.GetComponent<FlagID>();
        if (currentFlag != null)
            FlagTracker.DetectFlag(currentFlag).UpdatePercentage();
        else
            FlagTracker.GetCaptureMeter().gameObject.SetActive(false);
        #endregion

        #region Base Flag Case
        //Check if player is at Flag Case
        BaseFlagCase flagCase = other.GetComponent<BaseFlagCase>();
        if (flagCase != null && FlagTracker.GetCapturedFlags().Count != 0)
        {
            List<FlagID> toBeRemoved = new List<FlagID>();
            foreach (FlagID _flags in FlagTracker.GetCapturedFlags())
            {
                BaseFlagCase.AddFlagIntoCase(_flags);
                toBeRemoved.Add(_flags);
            }

            //After moving them to the case, we clear the list
            foreach(FlagID _flagsToRemove in toBeRemoved)
            {
                FlagTracker.GetCapturedFlags().Remove(_flagsToRemove);
            }
        } 
        #endregion
    }
}
