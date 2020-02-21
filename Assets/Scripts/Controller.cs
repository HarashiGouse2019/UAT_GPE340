using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Controller : MonoBehaviour
{
    public Pawn pawn; //The pawn in which we'll control

    [Header("AI")]
    public NavMeshAgent agent;

    public Transform target;

    // Start is called before the first frame update
    public virtual void Start()
    {
        pawn = GetComponent<Pawn>(); //Grab anything that is of type Pawn
    }
}
