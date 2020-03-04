using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannaquinTargetController : Controller
{
    //Find all weapons in the world
    //So that we can look for the closest one
    Weapons[] allWeaponsInWorld;

    //This is a reference to our finite state machine
    AIState aiState;

    public override void Start()
    {
        //We reference our finite state machine
        aiState = GetComponent<AIState>();

        //Check if we have no weapon
        if (pawn.weaponHandler.weapons == null)
        {
            //We're going to find all instances of type Weapon
            //so that we can get the closest one with our foreach loop
            allWeaponsInWorld = FindObjectsOfType<Weapons>();

            //We iterate and find the closest weapon nearby
            foreach (Weapons obj in allWeaponsInWorld)
            {
                //We find out which one is the closest
                if (Vector3.Distance(pawn.transform.position, obj.transform.position) <= 10)
                    target = obj.transform;

                //Changes our state to pursue our new target
                aiState.ChangeMovementStateTo(AIState.AIMovementState.PURSUE);
            }
        }
        else {
            //We'll pursue the player
            target = FindObjectOfType<PlayerPawn>().transform;
            aiState.ChangeMovementStateTo(AIState.AIMovementState.IDLE);
           
        }

    }

    void Update()
    {
        //Every frame, go for our target
        if(target != null)
            agent.SetDestination(target.position);
    }

    private void FixedUpdate()
    {
        //We create a vector that is from this to our target/trasnform.
        Vector3 desiredVelocity = Vector3.MoveTowards(transform.position, agent.desiredVelocity, agent.acceleration);

        //Innstead of going with the x, go with the z.
        Vector3 input = transform.InverseTransformDirection(desiredVelocity);

        //Pass in our values for our x and z for horizontal and vertical movement
        pawn.animator.SetFloat("Horizontal", input.x);
        pawn.animator.SetFloat("Vertical", input.z);
    }

    private void OnAnimatorMove()
    {
        //We want our agent to go the speed of our animator
        agent.velocity = pawn.animator.velocity;
    }
}
