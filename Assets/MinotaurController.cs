using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurController : MonoBehaviour
{
    //References to components or other gameobjects
    [Header("References")]
    private Rigidbody2D m_rigidbody;
    private Animator m_animator;
    private Transform player;
    //[Header("Attributes")]
    //public float health;
    //public float baseSpeed;
    private CreatureStats stats;
    [Header("Fire Point")]
    public Transform firePoint;
    [Header("Melee Attacks")]
    public GameObject meleePrefab;
    [Header("Dashing")]
    public float dashSpeed;
    public float dashLength, dashCooldown;
    private float dashCounter;
    private float dashCoolCounter;
    public bool isDashing;
    Vector2 dashDirection;

    // Start is called before the first frame update
    void Start()
    {
        //Set references
        m_rigidbody = GetComponent<Rigidbody2D>();

        m_animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        //Set current speed to the base speed on start
        //currentSpeed = baseSpeed;
        stats = GetComponent<CreatureStats>();
    }

    // Update is called once per frame
    void Update()
    {
        //set look positions for animator
        Vector2 position = m_rigidbody.position;

        //m_animator.SetFloat("Look X", position.x);
        //m_animator.SetFloat("Look Y", position.y);

        //Update the aim of the firepoint to target the player
        UpdateAim();

        if (dashCounter > 0)
        {
            Move(dashDirection, dashSpeed);
            dashCounter -= Time.deltaTime;
            if (dashCounter <= 0)
            {
                dashCoolCounter = dashCooldown;
                isDashing = false;
            }
        }

        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
    }

    /// <summary>This method changes the gameobject's location by
    /// moving it in the given direction at the given speed.
    /// <example>For example:
    /// <code>
    /// Move(Vector2.right, 1.5f);
    /// </code>
    /// results in the gameobject moving in the right direction by an amount of 1.5f.
    /// </example>
    /// </summary>
    public void Move(Vector2 direction, float speed)
    {
        //Debug.Log("Move function called.");
        //Get the current position based on the rigidbody
        Vector2 currentPos = m_rigidbody.position;
        //Multiply direction and speed to get the adjustedMovement value, which will modify the current position
        Vector2 adjustedMovement = direction * speed;
        //Calculate the new position by adding the adjustedMovement to the current position, and multiplying by fixedDeltaTime to correct for different framerates
        Vector2 newPos = currentPos + adjustedMovement * Time.fixedDeltaTime;
        //Set the rigidbody's location to the new position
        m_rigidbody.MovePosition(newPos);
    }

    /// <summary>This method changes the gameobject's location by
    /// moving it towards the given destination at the given speed.
    /// <example>For example:
    /// <code>
    /// Move(new Vector2(6, 0, 0), 1.5f));
    /// </code>
    /// results in the gameobject moving towards (6, 0, 0) by an amount of 1.5f.
    /// </example>
    /// </summary>
    public float MoveTowards(Vector2 destination, float speed)
    {
        //Invoke the enemyMove event
        GameEvents.OnEnemyMove.Invoke(LevelSettings.MapData.activeAStarGrid.ConvertWorldPositionToTilePosition(transform.position));
        //Calculate direction and store it
        Vector2 direction = ((Vector3)destination - (Vector3)m_rigidbody.position).normalized;
        //Calculate distance between creature and destination
        float distance = Vector2.Distance(m_rigidbody.position, destination);
        //If the distance is extremely small, return distance and don't move.
        if (distance <= 0) { return distance; }
        //Otherwise, the creature is a good distance from the destination. Call the move function.
        Move(direction, speed);
        //Return the distance
        return distance;
    }

    public void Hit(int damage, StatusEffect knockback)
    {
        //Apply knockback in the direction of the hit
        GetComponent<CreatureStatusEffectHandler>().AddStatusEffect(knockback);
        //Update your health amount
        stats.ChangeHealth(-damage);
    }

    public void SetStateToWander()
    {
        m_animator.SetBool("isWandering", true);
        m_animator.SetBool("isChasing", false);
    }

    public void SetStateToChase()
    {
        m_animator.SetBool("isWandering", false);
        m_animator.SetBool("isChasing", true);
    }
    public void SetStateToRangedAttack()
    {
        m_animator.SetTrigger("RangedAttack");
    }

    public void MeleeAttack()
    {
        //Enable the melee attack prefab
        meleePrefab.SetActive(true);
        //Set its position to the firepoint (which is updated based on the player's position)
        meleePrefab.transform.position = transform.position + firePoint.right * 0.4f;
    }

    public void StartDash()
    {
        if (dashCoolCounter <= 0 && dashCounter <= 0)
        {
            dashDirection = firePoint.right;
            dashCounter = dashLength;
            isDashing = true;
        }
    }

    public void StopDash()
    {
        dashCounter = 0;
        dashCoolCounter = dashCooldown;
        isDashing = false;
    }

    public bool isPlayerInMeleeAttackRange()
    {
        //Create an array of colliders of all gameObjects in this enemies detection range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, meleePrefab.GetComponent<MeleeAttack>().attackRadius);

        //Iterate through hits and check if any of them were the player
        for (int i = 0; i < hits.Length; i++)
        {
            //If one of the hits was the player, assign the player variable and return true
            if (hits[i].gameObject.CompareTag("Player"))
            {
                player = hits[i].transform;
                //Since the player is in the melee attack range, return true
                return true;
            }
        }

        //If the player is not in the melee attack range, return false
        return false;
    }


    float aimAngle;
    Vector2 aimDirection;
    public void UpdateAim()
    {
        aimDirection = (player.position - transform.position).normalized;
        aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, aimAngle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_animator.GetBehaviour<MinotaurChase>().StopDash();
    }
}
