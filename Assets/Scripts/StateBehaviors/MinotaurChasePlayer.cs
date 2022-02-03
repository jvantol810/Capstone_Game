using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurChasePlayer : StateMachineBehaviour
{
    private CreatureController minotaurController;
    Vector3 playerPos;
    public float distanceBeforeSwitchingToAttackState;
 
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        minotaurController = animator.GetComponent<CreatureController>();
        playerPos = minotaurController.player.position;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //If player is ever not detected, switch back to wander
        if (minotaurController.isPlayerDetected() == false)
        {
            minotaurController.SetAnimatorStateToWander();
        }

        //Update player pos
        playerPos = minotaurController.player.position;

        //Move towards player
        minotaurController.MoveTowards(playerPos, minotaurController.speed);

        //Check if the player is in the melee attack range. If so, switch the state to attacking.
        if (minotaurController.isPlayerInMeleeAttackRange())
        {
            minotaurController.SetAnimatorStateToAttack();
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
