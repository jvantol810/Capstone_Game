using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnExit : StateMachineBehaviour
{
    public float currentTime;
    public MeleeAttack meleeAttack;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime = 0;
        meleeAttack = animator.GetComponent<MeleeAttack>();
        Debug.Log("Enter state!");
        //animator.gameObject.SetActive(false, stateInfo.length);
        //Destroy(animator.gameObject, stateInfo.length);
    }

    //public IEnumerator DeactivateObjectAfterDelay(float delay, GameObject obj)
    //{
    //    yield return new WaitForSeconds(delay);
    //    obj.SetActive(false);
    //}
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime += Time.deltaTime;
        if (currentTime >= stateInfo.length)
        {
            //animator.gameObject.SetActive(false);
            meleeAttack.StopAttack();
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
