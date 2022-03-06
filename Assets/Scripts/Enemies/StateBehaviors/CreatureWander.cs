using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureWander : StateMachineBehaviour
{
    public CreatureController creatureController;
    public Vector2 creatureColliderPosition;
    
    public CooldownTimer wanderCooldown;
    [Header("Pathfinding")]
    public Vector2 currentDestination;
    int nextTileIndex = 1;
    Vector2[] currentPath;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Teleport the creature to a walkable tile
        creatureController = animator.GetComponent<CreatureController>();
        currentDestination = GenerateNewDestination();
        AStarGrid grid = LevelSettings.MapData.activeAStarGrid;
        currentPath = grid.FindPath(animator.transform.position, currentDestination);
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ////Calculate the path to the destination
        //Debug.Log("Reached destination: " + hasReached(currentDestination));
        //While you haven't reached the current destination, continue moving towards it along the path
        if (!hasReached(currentDestination) && nextTileIndex < currentPath.Length)
        {
            //if(nextTileIndex >= currentPath.Length) { return; }
            //Move towards the next tile in the path that you have generated
            creatureController.MoveTowards(currentPath[nextTileIndex], creatureController.speed);
            if (hasReached(currentPath[nextTileIndex]))
            {
                nextTileIndex++;
            }
        }
        //Otherwise, if you've reached the destination, generate a new one.
        else
        {
            //Debug.Log("Reached the destination!");
            //If the wander cooldown is not complete, skip execution
            if (wanderCooldown.cooldownComplete == false) { return; }

            ////Start the cooldown
            wanderCooldown.StartCooldown();

            //Log the current cooldown amount?
            Debug.Log("Cooldown amount: " + wanderCooldown.cooldownAmount);
            Debug.Log("Cooldown complete: " + wanderCooldown.cooldownComplete);
            //Update the current destination
            UpdatePath();
        }
        //If the creature has detected the player, switch the state to chase
        if (creatureController.isPlayerDetected())
        {
            SetStateToChase(animator);
        }
    }

    
    public Vector2 GenerateNewDestination()
    {
        AStarGrid grid = LevelSettings.MapData.activeAStarGrid;
        Vector2Int tilePosition = grid.ConvertWorldPositionToTilePosition(creatureController.transform.position);
        //Debug.Log("Starting tile position: " + tilePosition);
        //Get a random nearby walkable tile in the aStarGrid
        WorldTile newDestination = grid.GetWalkableTileWithinRange(tilePosition, 10f, 20f);

        //Place a yellow marker on your current destination
        //grid.PlaceMarker(newDestination.gridPosition, Color.yellow);

        //return the new destination's location
        Debug.Log("New destination: " + newDestination.centerWorldPosition);
        return newDestination.centerWorldPosition;
    }

    public void UpdatePath()
    {
        currentDestination = GenerateNewDestination();
        AStarGrid grid = LevelSettings.MapData.activeAStarGrid;
        currentPath = grid.FindPath(creatureController.transform.position, currentDestination);
        nextTileIndex = 1;
    }
   

    public bool hasReached(Vector2Int position)
    {
        return Vector2.Distance(creatureController.transform.position, position) <= 0.1f;
    }

    public bool hasReached(Vector2 position)
    {
        return Vector2.Distance(creatureController.transform.position, position) <= 0.1f;
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
