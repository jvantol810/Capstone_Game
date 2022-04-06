using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurChase : StateMachineBehaviour
{
    public MinotaurController minotaur;
    public CreatureStats minotaurStats;
    public Transform player;
    public float meleeAttackRange = 1f;

    private float dashSpeed;
    private float dashLength, dashCooldown;
    private float dashCounter;
    private float dashCoolCounter;
    public bool isDashing;
    Vector2 dashDirection;
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
        dashSpeed = minotaur.dashSpeed;
        dashLength = minotaur.dashLength;
        dashCounter = dashLength;
        dashCooldown = minotaur.dashCooldown;
    }


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //If the cooldown has completed and the dash has completed, set the dash direction and dash counter. Set dashing to true.
        if (dashCoolCounter <= 0 && dashCounter <= 0)
        {
            dashDirection = (player.position - animator.transform.position).normalized;
            dashCounter = dashLength;
            isDashing = true;
        }

        //If dash counter is greater than 0--in other words, if we are dashing.
        if (dashCounter > 0)
        {
            //Move in the direction of the dash by a speed of 20
            minotaur.Move(dashDirection, dashSpeed);
            //Decrement the dash counter
            dashCounter -= Time.deltaTime;
            //If the dash counter has hit 0, start the cooldown counter and set isDashing to false
            if (dashCounter <= 0)
            {
                dashCoolCounter = dashCooldown;
                isDashing = false;
            }
        }

        //If the dash cooldown timer is in effect, decrement it.
        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }

        //If we are not dashing, update the path.
        if (isDashing == false)
        {
            UpdatePath();

            //Generate a path to the player using the astargrid if not dashing
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
        }
        

        //If the player is outside the bomb dude's detection range, return to wander
        if (minotaurStats.isPlayerDetected() == false)
        {
            animator.SetBool("isWandering", true);
            animator.SetBool("isChasing", false);
        }

    }

    public void StopDash()
    {
        dashCounter = 0;
        dashCoolCounter = dashCooldown;
        isDashing = false;
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