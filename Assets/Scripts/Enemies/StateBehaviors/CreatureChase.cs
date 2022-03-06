using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureChase : StateMachineBehaviour
{
    public CreatureController creatureController;
    public Transform player;
    public float attackRange = 5f;
    public Vector2 tempPlayerPosition;
    AStarGrid grid = LevelSettings.MapData.activeAStarGrid;
    public Vector2[] currentPath;
    int nextTileIndex = 0;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        creatureController = animator.GetComponent<CreatureController>();
        //Get a reference to the player's transform using Gameobject.Find()
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //Store the player's position in a temporary variable
        tempPlayerPosition = player.position;
        //Find the path from the creature's position to the player and store it in currentPath
        currentPath = LevelSettings.MapData.activeAStarGrid.FindPath(creatureController.transform.position, tempPlayerPosition);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Generate a path to the player using the astargrid
        currentPath = LevelSettings.MapData.activeAStarGrid.FindPath(animator.transform.position, player.position);
        nextTileIndex = 0;
        if(currentPath.Length > 0)
        {
            //Move towards the current tile on the path. If you've reached the current tile already, then increment the tile index.
            creatureController.MoveTowards(currentPath[nextTileIndex], creatureController.speed);
            if(nextTileIndex + 1 < currentPath.Length) { nextTileIndex++; }
        }
        //nextTileIndex = 1;


        //If the player has changed their position (meaning that player.position is different significantly than tempPlayerPosition), then update the path
        //if (tempPlayerPosition != (Vector2)player.position)
        //{
        //    UpdatePath();
        //}
        //if(currentPath.Length > 0) {
        //    creatureController.MoveTowards(currentPath[nextTileIndex], creatureController.speed);
        //    if (hasReached(currentPath[nextTileIndex]))
        //    {
        //        nextTileIndex++;
        //    }
        //}
        //Otherwise, there is no path to the player. Assume that you must enter the attack state.

        //else
        //{
        //    animator.SetBool("isAttacking", true);
        //    animator.SetBool("isChasing", false);
        //}
        //creatureController.MoveTowards(currentPath[nextTileIndex], creatureController.speed);
        //if (hasReached(currentPath[nextTileIndex]))
        //{
        //    nextTileIndex++;
        //}
        if (creatureController.isPlayerInMeleeAttackRange())
        {
            animator.SetBool("isAttacking", true);
            animator.SetBool("isChasing", false);
        }
    }

    public void UpdateStateToAttack()
    {

    }
    public void UpdatePath()
    {
        AStarGrid grid = LevelSettings.MapData.activeAStarGrid;
        currentPath = grid.FindPath(creatureController.transform.position, player.position);
        nextTileIndex = 0;
        tempPlayerPosition = player.position;
    }

    public bool hasReached(Vector2Int position)
    {
        return Vector2.Distance(creatureController.transform.position, position) <= 0.1f;
    }

    public bool isInAttackRange(Vector2 currentPosition, Vector2 playerPosition)
    {
        return Vector2.Distance(currentPosition, playerPosition) <= attackRange;
    }

    public bool hasReached(Vector2 position)
    {
        return Vector2.Distance(creatureController.transform.position, position) <= 0.1f;
    }
}
