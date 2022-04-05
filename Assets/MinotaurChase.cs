using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurChase : StateMachineBehaviour
{
    public MinotaurController minotaur;
    public CreatureStats minotaurStats;
    public Transform player;
    public float meleeAttackRange = 1f;

    public float chargeAttackCooldown = 1f;
    public float chargeAttackTimer;
    public bool onCooldown = false;
    //public float timeUntilDash = 0f;
    public Vector2 tempPlayerPosition;
    AStarGrid grid = LevelSettings.MapData.activeAStarGrid;
    public Vector2[] currentPath;
    int nextTileIndex = 0;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        minotaur = animator.GetComponent<MinotaurController>();
        minotaurStats = animator.GetComponent<CreatureStats>();
        //Get a reference to the player's transform using Gameobject.Find()
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //Store the player's position in a temporary variable
        tempPlayerPosition = player.position;
        //Find the path from the creature's position to the player and store it in currentPath
        currentPath = LevelSettings.MapData.activeAStarGrid.FindPath(minotaur.transform.position, tempPlayerPosition);

        chargeAttackTimer = chargeAttackCooldown;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Generate a path to the player using the astargrid
        currentPath = LevelSettings.MapData.activeAStarGrid.FindPath(animator.transform.position, player.position);
        nextTileIndex = 0;
        if (currentPath.Length > 0)
        {
            //Move towards the current tile on the path. If you've reached the current tile already, then increment the tile index.
            minotaur.MoveTowards(currentPath[nextTileIndex], minotaurStats.currentSpeed);
            if (nextTileIndex + 1 < currentPath.Length) { nextTileIndex++; }
        }

        else if (!hasReached(player.position))
        {
            minotaur.MoveTowards(player.position, minotaurStats.currentSpeed);
        }

        //If the player is in melee range, do a melee attack
        if (minotaur.isPlayerInMeleeAttackRange())
        {
            minotaur.MeleeAttack();
        }

        else if (!onCooldown)
        {
            Debug.Log("Doing a charge attack!");
            //bombDude.BombAttack();
            onCooldown = true;
        }

        if (onCooldown)
        {
            chargeAttackTimer -= Time.deltaTime;
            if (chargeAttackTimer <= 0f)
            {
                chargeAttackTimer = chargeAttackCooldown;
                onCooldown = false;
            }
        }

        UpdatePath();

        //If the player is outside the bomb dude's detection range, return to wander
        if (minotaurStats.isPlayerDetected() == false)
        {
            animator.SetBool("isWandering", true);
            animator.SetBool("isChasing", false);
        }

    }

    public void UpdatePath()
    {
        AStarGrid grid = LevelSettings.MapData.activeAStarGrid;
        currentPath = grid.FindPath(minotaur.transform.position, player.position);
        nextTileIndex = 0;
        tempPlayerPosition = player.position;
    }

    public bool hasReached(Vector2Int position)
    {
        return Vector2.Distance(minotaur.transform.position, position) <= 0.2f;
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
        return Vector2.Distance(minotaur.transform.position, position) <= 0.1f;
    }
}
