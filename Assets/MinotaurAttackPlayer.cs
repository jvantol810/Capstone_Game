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
        //If player is ever not detected, switch back to wander
        if ( minotaurController.CheckIfPlayerIsDetected() == false )
        {
            minotaurController.SetAnimatorStateToWander();
        }

        //Update player pos
        playerPos = minotaurController.player.position;

        //If the distance between enemy and player is larger than the min charge distance, charge
        if (Vector3.Distance(minotaurController.transform.position, playerPos) >= minimumChargeDistance)
        {
            minotaurController.Charge(playerPos);
        }
        //Otherwise the minotaur must be close to the player. Execute a melee attack.
        else
        {
            minotaurController.MeleeAttack();
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
