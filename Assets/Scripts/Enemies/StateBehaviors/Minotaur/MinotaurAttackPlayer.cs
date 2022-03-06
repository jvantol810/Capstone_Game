using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurAttackPlayer : StateMachineBehaviour
{
    public float minimumChargeDistance;
    private CreatureController minotaurController;
    Vector3 playerPos;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        minotaurController = animator.GetComponent<CreatureController>();
        playerPos = minotaurController.player.position;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Call the attack function
        minotaurController.MeleeAttack();

        //If player is ever not detected, switch back to wander
        if ( minotaurController.isPlayerDetected() == false )
        {
            minotaurController.SetAnimatorStateToWander();
        }

        //If the player is ever not in the melee attack range, switch back to chase
        if (minotaurController.isPlayerInMeleeAttackRange() == false)
        {
            minotaurController.SetAnimatorStateToChase();
        }

        //Update player pos
        playerPos = minotaurController.player.position;

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
