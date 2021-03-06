using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombChase : StateMachineBehaviour
{
    public BombController bombDude;
    public CreatureStats bombDudeStats;
    public CreatureSpriteAnimator spriteAnimator;
    public Transform player;
    public float meleeAttackRange = 1f;

    public float bombDropCooldown;
    public float bombDropCoolCounter;
    public bool onCooldown = false;
    //public float timeUntilDash = 0f;
    public Vector2 tempPlayerPosition;
    AStarGrid grid = LevelSettings.MapData.activeAStarGrid;
    public Vector2[] currentPath;
    int nextTileIndex = 0;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bombDude = animator.GetComponent<BombController>();
        bombDudeStats = animator.GetComponent<CreatureStats>();
        spriteAnimator = animator.GetComponent<CreatureSpriteAnimator>();
        //Get a reference to the player's transform using Gameobject.Find()
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //Store the player's position in a temporary variable
        tempPlayerPosition = player.position;
        //Find the path from the creature's position to the player and store it in currentPath
        currentPath = LevelSettings.MapData.activeAStarGrid.FindPath(bombDude.transform.position, tempPlayerPosition);
        bombDropCooldown = bombDude.bombDropCooldown;
        bombDropCoolCounter = bombDropCooldown;
        
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Generate a path to the player using the astargrid
        currentPath = LevelSettings.MapData.activeAStarGrid.FindPath(animator.transform.position, player.position);
        nextTileIndex = 0;
        if (currentPath.Length > 0)
        {
            //Move towards the current tile on the path. If you've reached the current tile already, then increment the tile index.
            bombDude.MoveTowards(currentPath[nextTileIndex], bombDudeStats.currentSpeed);
            spriteAnimator.currentDestination = currentPath[nextTileIndex];
            if (nextTileIndex + 1 < currentPath.Length) { nextTileIndex++; }
        }

        else if (!hasReached(player.position))
        {
            bombDude.MoveTowards(player.position, bombDudeStats.currentSpeed);
            spriteAnimator.currentDestination = player.position;
        }

        //If the player is in melee range, do a melee attack
        if (bombDude.isPlayerInMeleeAttackRange())
        {
            bombDude.MeleeAttack();
        }

        else if (!onCooldown)
        {
            Debug.Log("Doing a bomb attack!");
            bombDude.BombAttack();
            onCooldown = true;
        }

        if (onCooldown)
        {
            bombDropCoolCounter -= Time.deltaTime;
            if (bombDropCoolCounter <= 0f)
            {
                bombDropCoolCounter = bombDropCooldown;
                onCooldown = false;
            }
        }

        UpdatePath();

        //If the player is outside the bomb dude's detection range, return to wander
        if (bombDudeStats.isPlayerDetected() == false)
        {
            animator.SetBool("isWandering", true);
            animator.SetBool("isChasing", false);
        }

    }

    public void UpdatePath()
    {
        AStarGrid grid = LevelSettings.MapData.activeAStarGrid;
        currentPath = grid.FindPath(bombDude.transform.position, player.position);
        nextTileIndex = 0;
        tempPlayerPosition = player.position;
    }

    public bool hasReached(Vector2Int position)
    {
        return Vector2.Distance(bombDude.transform.position, position) <= 0.2f;
    }

    public bool isInShootingRange(Vector2 currentPosition, Vector2 playerPosition)
    {
        return Vector2.Distance(currentPosition, playerPosition) >= meleeAttackRange;
    }

    public bool isInMeleeRange(Vector2 currentPosition, Vector2 playerPosition)
    {
        return Vector2.Distance(currentPosition, playerPosition) <= meleeAttackRange;
    }

    public bool hasReached(Vector2 position)
    {
        return Vector2.Distance(bombDude.transform.position, position) <= 0.1f;
    }
}
