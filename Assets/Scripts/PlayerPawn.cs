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
        PlayerAmmoTextHandler.RunAmmoTextUI();
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
}
