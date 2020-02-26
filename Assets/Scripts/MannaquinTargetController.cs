using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannaquinTargetController : Controller
{
    Weapons[] allWeaponsInWorld;

    AIState aiState;

    public override void Start()
    {
        aiState = GetComponent<AIState>();
        if (pawn.weaponHandler.weapons == null)
        {
            allWeaponsInWorld = FindObjectsOfType<Weapons>();

            //We iterate and find the closest weapon nearby
            foreach (Weapons obj in allWeaponsInWorld)
            {
                if (Vector3.Distance(pawn.transform.position, obj.transform.position) <= 10)
                    target = obj.transform;

                aiState.ChangeMovementStateTo(AIState.AIMovementState.PURSUE);
            }
        }
        else {
            target = FindObjectOfType<PlayerPawn>().transform;
            aiState.ChangeMovementStateTo(AIState.AIMovementState.IDLE);
           
        }

    }

    void Update()
    {
        agent.SetDestination(target.position);
    }

    private void FixedUpdate()
    {
        Vector3 desiredVelocity = Vector3.MoveTowards(transform.position, agent.desiredVelocity, agent.acceleration);

        Vector3 input = transform.InverseTransformDirection(desiredVelocity);

        pawn.animator.SetFloat("Horizontal", input.x);
        pawn.animator.SetFloat("Vertical", input.z);
    }

    private void OnAnimatorMove()
    {
        agent.velocity = pawn.animator.velocity;
    }
}
