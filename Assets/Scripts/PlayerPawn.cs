using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPawn : Pawn
{
    //This is unique to the player only, where the camera will rotate if they dies.
    public ObjectPooler deathSpotPooler;

    /// <summary>
    /// Move the pawn in a direction in World Space
    /// </summary>
    /// <param name="worldDirectionToMove"></param>
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

    /// <summary>
    /// Execute an action the moment an object spawns.
    /// </summary>
    public override void OnSpawn()
    {
        GameManager.LogPlayer(this);
        PlayerStaminaUiHandler.RunStaminaUI();
        PlayerHealthUiHandler.RunHealthUI();
        base.OnSpawn();
    }

    /// <summary>
    /// Enable Ragdoll physics
    /// </summary>
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

            //Lose player lives
            LoseLives();
        }
    }

    /// <summary>
    /// Lose 1 Player Life Stock
    /// </summary>
    void LoseLives()
    {
        //Decrease player lives
        GameManager.Instance.DecrementPlayerLives();

        //Check if player lives is 0
        if (GameManager.GetPlayerLives() <= 0)
        {
            GetComponentInParent<SpawnerHandler>().End();
            GameManager.EndGame();
        }
    }

    /// <summary>
    /// Check if a flag is within capture range
    /// </summary>
    /// <param name="other"></param>
    void OnFlagInRange(Collider other)
    {
        #region Capture Flag
        //Detect a flag if close to it.
        FlagID currentFlag = other.GetComponent<FlagID>();

        if (currentFlag != null)
            FlagTracker.DetectFlag(currentFlag).UpdatePercentage();
        else
            FlagTracker.GetCaptureMeter().gameObject.SetActive(false);
        #endregion
    }

    /// <summary>
    /// Check if flag case is within range
    /// </summary>
    /// <param name="other"></param>
    void OnCaseInRange(Collider other)
    {
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
            foreach (FlagID _flagsToRemove in toBeRemoved)
            {
                FlagTracker.GetCapturedFlags().Remove(_flagsToRemove);
            }
        }
        #endregion
    }

    private void OnTriggerStay(Collider collider)
    {
        OnFlagInRange(collider);
        OnCaseInRange(collider);
    }
}
