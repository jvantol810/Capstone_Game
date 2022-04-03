using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureWander : StateMachineBehaviour
{
    public CreatureStats creature;
    private Rigidbody2D m_rb;
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
        if (animator.GetComponent<CreatureStats>() == null) { return; }

        creature = animator.GetComponent<CreatureStats>();
        m_rb = animator.GetComponent<Rigidbody2D>();
        currentDestination = GenerateNewDestination();
        AStarGrid grid = LevelSettings.MapData.activeAStarGrid;
        currentPath = grid.FindPath(animator.transform.position, currentDestination);
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Check if the creatureController has been destroyed. If it has, destroy yourself.
        if(creature == null)
        {
            Destroy(animator);
        }
        ////Calculate the path to the destination
        //Debug.Log("Reached destination: " + hasReached(currentDestination));
        Debug.Log(currentDestination);
        //While you haven't reached the current destination, continue moving towards it along the path
        if (!hasReached(currentDestination) && nextTileIndex < currentPath.Length)
        {
            //if(nextTileIndex >= currentPath.Length) { return; }
            //Move towards the next tile in the path that you have generated
            CreatureActions.MoveTowards(m_rb, currentPath[nextTileIndex], creature.currentSpeed);
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
            //Debug.Log("Cooldown amount: " + wanderCooldown.cooldownAmount);
            //Debug.Log("Cooldown complete: " + wanderCooldown.cooldownComplete);
            //Update the current destination
            UpdatePath();
        }
        //If the creature has detected the player, switch the state to chase
        if (creature.isPlayerDetected() == true)
        {
            animator.SetBool("isChasing", true);
            animator.SetBool("isWandering", false);
        }
    }

    
    public Vector2 GenerateNewDestination()
    {
        AStarGrid grid = LevelSettings.MapData.activeAStarGrid;
        Vector2Int tilePosition = grid.ConvertWorldPositionToTilePosition(creature.transform.position);
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
        currentPath = grid.FindPath(creature.transform.position, currentDestination);
        nextTileIndex = 1;
    }
   

    public bool hasReached(Vector2Int position)
    {
        return Vector2.Distance(creature.transform.position, position) <= 0.1f;
    }

    public bool hasReached(Vector2 position)
    {
        return Vector2.Distance(creature.transform.position, position) <= 0.1f;
    }


    public void SetStateToChase(Animator anim)
    {
        //Debug.Log("Switching to chase state!");
        anim.SetBool("isChasing", true);
        anim.SetBool("isWandering", false);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(creature.transform.position, 10);
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
