using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMinotaurCharge : StateMachineBehaviour
{
    //Reference to the player's PlayerController script
    public PlayerController player;

    public Vector2 chargeDirection;

    public Rigidbody2D m_rigidobdy;
    public Vector2 destination;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Set the reference by getting the PlayerController component from the Animator gameobject (AKA the player)
        player = animator.GetComponent<PlayerController>();
        m_rigidobdy = animator.GetComponent<Rigidbody2D>();
        chargeDirection = player.lookDirection.normalized;
        destination = (Vector2)animator.transform.position + (chargeDirection * 10f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distanceToDestination = CreatureActions.MoveTowards(m_rigidobdy, destination, 20f);
        if(distanceToDestination <= 0.2f)
        {
            Debug.Log("Reached destination!");
            animator.SetBool("isCharging", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
