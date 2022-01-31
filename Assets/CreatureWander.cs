using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureWander : StateMachineBehaviour
{
    public CreatureController creatureController;
    public Vector2 currentDestination;
    public CooldownTimer wanderCooldown;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        creatureController = animator.GetComponent<CreatureController>();
        currentDestination = GenerateNewDestination();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        //Move towards the current destination
        creatureController.MoveTowards(currentDestination, creatureController.speed);
        
        //If the creature has detected the player, switch the state to chase
        if( creatureController.CheckIfPlayerIsDetected() )
        {
            SetStateToChase(animator);
        }

        //If the creature has reached the current destination, calculate a new one
        if (ReachedDestination())
        {
            //If the wander cooldown is not complete, skip execution
            if (wanderCooldown.cooldownComplete == false) { return; }

            //Start the cooldown
            wanderCooldown.StartCooldown();

            //Update the current destination
            currentDestination = GenerateNewDestination();
        }

        
    }

    public Vector2 GenerateNewDestination()
    {
        //Generate a random direction
        Vector2 randomDirection = new Vector2(
            Mathf.RoundToInt( Random.Range(0f, 1f) ),
            Mathf.RoundToInt( Random.Range(0f, 1f) ) 
        );

        //Pick a distance -- picking 5f for now
        float distance = Random.Range(2f, 4f);

        //Cast a ray to that destination
        Debug.DrawRay(creatureController.transform.position, randomDirection * distance, Color.red, 20, true);
        RaycastHit2D hit = Physics2D.Raycast(creatureController.transform.position, randomDirection, distance);
       
        //If the hit intersects with an object, calculate the distance and move up to that point (so the destination is never set into an impossible to reach area)
        if (hit.collider)
        {
            Debug.Log("Collided with: " + hit.collider.gameObject.tag);
            //Call the function again
            return GenerateNewDestination();
        }
        else
        {
            return randomDirection * distance;
        }
        
    }

    public bool ReachedDestination()
    {
        return Vector2.Distance(creatureController.transform.position, currentDestination) <= 0.1f;
    }

    public void SetStateToChase(Animator anim)
    {
        Debug.Log("Switching to chase state!");
        anim.SetBool("isChasing", true);
        anim.SetBool("isAttacking", false);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(creatureController.transform.position, 10);
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
