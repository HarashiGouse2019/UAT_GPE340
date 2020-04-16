using System;
using System.Collections;
using UnityEngine;

public class AIState : MonoBehaviour
{
    //This will be responsible for AI Related Stuff
    //There will be two enumerators; one based on movement
    //and one based on rather he's shooting or not.
    public enum AIMovementState
    {
        IDLE,
        PURSUE,
        EVADE
    }

    public enum AIAggroState
    {
        NOTAGGRO,
        AGGRO
    }

    /*We'll have to change the agents move speed if the AI is at aggro distance.
     As it's walking, we limit the velocity of the agent, putting it to a stop.
     At least that's what I hope.*/
    [SerializeField]
    private float pursuitDistance;

    /*At shooting distance, the AI will stand idle, until
     the player is no longer withing shooting distance.*/
    [SerializeField]
    private float shootingDistance;

    public AIMovementState AI_Movement_State { get; set; }
    public AIAggroState AI_Aggro_State { get; set; }

    //We'll need to reference our controller, so that in case
    //we need to access the NavMeshAgent
    public Controller AI_Controller { get; set; }

    //A boolean that we can use to change the animator in the pawn, which we can grab from the controller
    public bool IsAggro { get; set; }
    public bool IsChasing { get; set; }
    public bool IsEvading { get; set; }

    //And an IEnumerator that checks the 2 Ai States
    IEnumerator manageStates;

    void Start()
    {
        //Get our controller
        AI_Controller = GetComponent<Controller>();

        manageStates = ManageAIStates();
        StartCoroutine(manageStates);
    }

    public void ChangeMovementStateTo(AIMovementState _AiMovementState)
    {
        AI_Movement_State = _AiMovementState;

        //Check on what was passed through the function
        switch (AI_Movement_State)
        {
            case AIMovementState.IDLE:
                IsChasing = IsEvading = false;
                break;

            case AIMovementState.PURSUE:
                IsChasing = true;
                IsEvading = false;
                break;

            case AIMovementState.EVADE:
                IsChasing = false;
                IsEvading = true;
                break;

            default:
                break;
        }

        //Update the animator values
        UpdateAnimatorValues<bool>("isChasing", IsChasing);
        UpdateAnimatorValues<bool>("isEvading", IsEvading);

        return;
    }

    public void ChangeAggroStateTo(AIAggroState _AiAggroState)
    {
        AI_Aggro_State = _AiAggroState;

        //Check on what was passed through the function
        switch (AI_Aggro_State)
        {
            case AIAggroState.NOTAGGRO:
                IsAggro = false;
                break;
            case AIAggroState.AGGRO:
                IsAggro = true;
                break;
            default:
                break;
        }

        //Update the animator values
        UpdateAnimatorValues<bool>("isAggro", IsAggro);

        return;
    }

    void UpdateAnimatorValues<T>(string _param, object _paramVal)
    {
        Type paramType = typeof(T);

        try
        {
            if (paramType == typeof(int))
                AI_Controller.pawn.animator.SetInteger(_param, (int)_paramVal);

            if (paramType == typeof(float))
                AI_Controller.pawn.animator.SetFloat(_param, (float)_paramVal);

            if (paramType == typeof(bool))
                AI_Controller.pawn.animator.SetBool(_param, (bool)_paramVal);
        }
        catch { }
    }

    IEnumerator ManageAIStates()
    {
        while (true)
        {
            if (!AI_Controller.pawn.IsDead())
            {
                if (AI_Controller.pawn.weaponHandler.weapons != null)
                {
                    Transform target = AI_Controller.target;


                    if (target != null)
                    {

                        switch (AI_Movement_State)
                        {
                            case AIMovementState.IDLE:
                                /*We'll do nothing. However,  we have to be sure that we are in
                                 our idle position. You may need to do some changes to the pawn's respective
                                 animator.*/

                                AI_Controller.agent.speed = 0f;

                                //Let's check if player is in distance.
                                if (Vector3.Distance(AI_Controller.pawn.transform.position, target.position) < pursuitDistance &&
                                    Vector3.Distance(AI_Controller.pawn.transform.position, target.position) > shootingDistance)
                                {
                                    ChangeMovementStateTo(AIMovementState.PURSUE);
                                    ChangeAggroStateTo(AIAggroState.AGGRO);
                                }

                                break;

                            case AIMovementState.PURSUE:
                                /*We want the pursue the given target if the state happens to be
                                 this. This is what the controller reference is handy for.*/
                                AI_Controller.agent.SetDestination(target.position);

                                AI_Controller.agent.speed = 10f;

                                //While pursuing, we want to check if we are at shooting distance.
                                if (Vector3.Distance(AI_Controller.pawn.transform.position, target.position) < pursuitDistance)
                                    ChangeAggroStateTo(AIAggroState.AGGRO);
                                else
                                    ChangeAggroStateTo(AIAggroState.NOTAGGRO);

                                //What if we're close to death? We'll check the pawn's damageableObj and check if objHealth is
                                //less than 20
                                if (AI_Controller.pawn.GetDamageableObj().GetObjHealthVal() < 21)
                                    ChangeMovementStateTo(AIMovementState.EVADE);

                                break;

                            case AIMovementState.EVADE:
                                /*This will be a unique kind of state. Honestly, we're just doing the same thing
                                 for pursuing, but just the inverse. Keep that in mind as you do this.*/
                                AI_Controller.agent.SetDestination(target.position);

                                AI_Controller.agent.speed = -10f;

                                //While pursuing, we want to check if we are at shooting distance.
                                if (Vector3.Distance(AI_Controller.pawn.transform.position, target.position) < pursuitDistance)
                                    ChangeAggroStateTo(AIAggroState.AGGRO);
                                else
                                    ChangeAggroStateTo(AIAggroState.NOTAGGRO);

                                //What if we're close to death? We'll check the pawn's damageableObj and check if objHealth is
                                //less than 20
                                if (AI_Controller.pawn.GetDamageableObj().GetObjHealthVal() > 19)
                                    ChangeMovementStateTo(AIMovementState.PURSUE);
                                break;

                            default:
                                //Do nothing
                                break;
                        }

                        switch (AI_Aggro_State)
                        {
                            case AIAggroState.NOTAGGRO:
                                /*We will use this to make sure that the AI is not
                                 having his gun up (again, this will require some modifications
                                 to the ai pawn's respective animator). While in this state, we check if the player's nearby
                                 or not.*/

                                //For now, we know that we are not doing anything, except change the velocity
                                break;

                            case AIAggroState.AGGRO:
                                /*In this state, the enemy ai will have his gun up, aiming at his target. There's a reason
                                 why we have two finite state machines. Mainly because as we're aggro, we can either pursue
                                 the target, or evading from the target. In the special case that we're evading, let's say the AI's
                                 health is very low. This is when we can switch the movement state to evade, while still being aggro.*/
                                if (Vector3.Distance(AI_Controller.pawn.transform.position, target.position) < shootingDistance)
                                    AI_Controller.pawn.weaponHandler.equippedWeapon.Shoot();
                                break;

                            default:
                                //Do nothing
                                break;
                        } 
                    }

                   
                } 
            }
            else
            {
                AI_Controller.agent.SetDestination(Vector3.zero);
                AI_Controller.pawn.itemDrop.DropAllItems();
                yield return false;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
