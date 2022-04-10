using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderChase : StateMachineBehaviour
{
    public SpiderController spider;
    public CreatureStats spiderStats;
    public Transform player;
    public float meleeAttackRange = 1f;

    public float webAttackCooldown;
    public float webAttackTimer;
    public bool onCooldown = false;
    private float shootDistance;
    //public float timeUntilDash = 0f;
    public Vector2 tempPlayerPosition;
    AStarGrid grid = LevelSettings.MapData.activeAStarGrid;
    public Vector2[] currentPath;
    int nextTileIndex = 0;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        spider = animator.GetComponent<SpiderController>();
        spiderStats = animator.GetComponent<CreatureStats>();
        //Get a reference to the player's transform using Gameobject.Find()
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //Store the player's position in a temporary variable
        tempPlayerPosition = player.position;
        //Find the path from the creature's position to the player and store it in currentPath
        currentPath = LevelSettings.MapData.activeAStarGrid.FindPath(spider.transform.position, tempPlayerPosition);
        webAttackCooldown = spider.webCooldown;
        webAttackTimer = webAttackCooldown;
        shootDistance = spiderStats.detectionRange * 0.5f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //If you're on the cooldown for shooting a web, update the timer
        if (onCooldown)
        {
            webAttackTimer -= Time.deltaTime;
            if (webAttackTimer <= 0f)
            {
                webAttackTimer = webAttackCooldown;
                onCooldown = false;
            }
        }

        //Check if you're in shooting distance. If so, shoot a web, start the cooldown, and exit.
        if (isInShootingRange(animator.transform.position, player.position))
        {
            if (!onCooldown)
            {
                Debug.Log("Doing a web attack!");
                spider.WebAttack();
                onCooldown = true;
                return;
            }
        }

        //If you've reached this point in the code, then you're not in shooting range. At this point, you should be moving into position

        //Generate a path to the player and continually move towards it. This process will pause once the Spider reaches a distance to the player equal to half of their detection radius.
        currentPath = LevelSettings.MapData.activeAStarGrid.FindPath(animator.transform.position, player.position);
        nextTileIndex = 0;
        if (currentPath.Length > 0)
        {
            //Move towards the current tile on the path. If you've reached the current tile already, then increment the tile index.
            spider.MoveTowards(currentPath[nextTileIndex], spiderStats.currentSpeed);
            if (nextTileIndex + 1 < currentPath.Length) { nextTileIndex++; }
        }



        else if (!hasReached(player.position))
        {
            spider.MoveTowards(player.position, spiderStats.currentSpeed);
        }

        //If the player is in melee range, do a melee attack
        if (spider.isPlayerInMeleeAttackRange())
        {
            spider.MeleeAttack();
        }

       

        UpdatePath();

        //If the player is outside the spider's detection range, return to wander
        if(spiderStats.isPlayerDetected() == false)
        {
            animator.SetBool("isWandering", true);
            animator.SetBool("isChasing", false);
        }

    }

    public void UpdatePath()
    {
        AStarGrid grid = LevelSettings.MapData.activeAStarGrid;
        currentPath = grid.FindPath(spider.transform.position, player.position);
        nextTileIndex = 0;
        tempPlayerPosition = player.position;
    }

    public bool hasReached(Vector2Int position)
    {
        return Vector2.Distance(spider.transform.position, position) <= 0.2f;
    }

    public bool isInShootingRange(Vector2 currentPosition, Vector2 playerPosition)
    {
        return Vector2.Distance(currentPosition, playerPosition) <= shootDistance;
    }

    public bool isInMeleeRange(Vector2 currentPosition, Vector2 playerPosition)
    {
        return Vector2.Distance(currentPosition, playerPosition) <= meleeAttackRange;
    }

    public bool hasReached(Vector2 position)
    {
        return Vector2.Distance(spider.transform.position, position) <= 0.1f;
    }
}
