using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCreatureIdle : StateMachineBehaviour
{
    [Header("Sprite To Change To")]
    public Sprite CreatureSprite;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //When the player enters this state, create a reference to their PlayerController script. Also change their sprite to be a minotaur.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Get the player's SpriteRenderer component and set its sprite to be a minotaur
        animator.GetComponent<SpriteRenderer>().sprite = CreatureSprite;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //If the player presses the space key, let them charge
        if (Input.GetKeyDown("space"))
        {
            animator.SetBool("isCharging", true);
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
