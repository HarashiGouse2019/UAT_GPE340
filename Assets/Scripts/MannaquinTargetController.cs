using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannaquinTargetController : Controller
{
    public override void Start()
    {
        target = FindObjectOfType<PlayerPawn>().transform;
        agent.SetDestination(target.position);

    }

    void Update()
    {
        
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
