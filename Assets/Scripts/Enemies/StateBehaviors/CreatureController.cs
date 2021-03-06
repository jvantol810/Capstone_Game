using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class CreatureController : MonoBehaviour
{
   
    [Header("CreatureType")]
    [SerializeField]
    public CreatureTypes CreatureType;
    //References to components or other gameobjects
    [Header("References")]
    private Rigidbody2D m_rigidbody;
    //[Header("AI")]
    private Animator m_animator;
    public Transform player;
    ////Transition to attacking whenever these events fire off
    //[Header("State Transitions")]
    //public List<UnityEvent> attackEvents;
    ////Transition to wandering whenever these events fire off
    //public List<UnityEvent> wanderEvents;
    //Attributes/enemy stats
    [Header("Attributes")]
    public float health;
    public float baseSpeed;
    [HideInInspector]
    public float currentSpeed;
    public float damage;
    [Header("Detection")]
    public float detectionRange;
    [Header("Fire Point")]
    public Transform firePoint;
    [Header("Melee Attacks")]
    public GameObject meleePrefab;
    [Header("Dash Settings")]
    public float dashSpeed;
    public float dashLength, dashCooldown;
    private float dashCounter;
    private float dashCoolCounter;
    public bool isDashing;
    [Header("Ranged Attacks")]
    public GameObject projectilePrefab;
    public Transform rangedAttackPoint;
    public float rangedAttackSpeed;
    public LayerMask whatCanBeHitByRanged;
    [Header("Charge")]
    //Special movements
    public bool charging = false;
    public float chargeSpeed;
    private Vector2 chargeDestination;
    //Attacks
    public bool swingAttacking = false;

    [Header("State")]
    public TextMeshProUGUI stateText;

    [HideInInspector]
    public Vector2 knockbackForce = Vector2.zero;
    [HideInInspector]
    public bool isBeingKnockedBack = false;




    // Start is called before the first frame update
    void Start()
    {
        //Set references
        m_rigidbody = GetComponent<Rigidbody2D>();

        //weaponAnimator = weapon.GetComponent<Animator>();
        m_animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        //Set current speed to the base speed on start
        currentSpeed = baseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //set look positions for animator
        Vector2 position = m_rigidbody.position;

        m_animator.SetFloat("Look X", position.x);
        m_animator.SetFloat("Look Y", position.y);

        //Update the aim of the firepoint to target the player
        UpdateAim();

        //Check if knockback is being applied. If it is, move in direction of knockback.
        if (isBeingKnockedBack)
        {
            m_rigidbody.MovePosition(transform.position + (Vector3)knockbackForce);
        }

        if (dashCounter > 0)
        {
            CreatureActions.Move(m_rigidbody, firePoint.right, dashSpeed);
            dashCounter -= Time.deltaTime;
            if (dashCounter <= 0)
            {
                m_animator.enabled = true;
                dashCoolCounter = dashCooldown;
                isDashing = false;
            }
        }
        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }

        //stateText.text = GetCurrentState();

        //stateText.transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, 0);
    }

    private void FixedUpdate()
    {
        if (charging)
        {
            float distance = MoveTowards(chargeDestination, chargeSpeed);
            if(distance <= 0.5)
            {
                charging = false;
            }
        }
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
        if(distance <= 0.1) { return distance;  }
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
        ChangeHealth(-damage);
    }

    public void Charge(Vector2 destination)
    {
        //Debug.Log("Charging!");
        if(!charging)
        {
            chargeDestination = destination;
            charging = true;
        }
    }

    public void Dash()
    {
        if (dashCoolCounter <= 0 && dashCounter <= 0)
        {
            dashCounter = dashLength;
            isDashing = true;
            m_animator.enabled = false;
        }
    }

    public void RangedAttack(GameObject projectilePrefab, Vector2 direction)
    {
        //Create a projectile prefab at the ranged attack position
        Projectile projectile = Instantiate(projectilePrefab, rangedAttackPoint).GetComponent<Projectile>();

        //Set the projectile's canBeHit layermask to be equal to the layermask for ranged attacks
        projectile.whatCanBeHit = whatCanBeHitByRanged;

        //Send a projectile outwards
        projectile.Shoot(direction);
    }

    public void SetAnimatorStateToAttack()
    {
        //Debug.Log("Transitioning to the attack state!");
        if (m_animator.GetBool("isAttacking") == false)
        {
            m_animator.SetBool("isAttacking", true);
            m_animator.SetBool("isChasing", false);
        }
        
    }
    public void SetAnimatorStateToChase()
    {
        //Debug.Log("Transitioning to the chase state!");
        if (m_animator.GetBool("isChasing") == false)
        {
            m_animator.SetBool("isChasing", true);
            m_animator.SetBool("isAttacking", false);
        }

    }
    public void SetAnimatorStateToWander()
    {
        //Debug.Log("Transitioning to the wander state!");
        m_animator.SetBool("isAttacking", false);
        m_animator.SetBool("isChasing", false);
    }

    public void MeleeAttack()
    {
        //Enable the melee attack prefab
        meleePrefab.SetActive(true);
        //Set its position to the firepoint (which is updated based on the player's position)
        meleePrefab.transform.position = transform.position + firePoint.right;
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
                //Debug.Log("Player in melee attack range!");
                //Since the player is in the melee attack range, return true
                return true;
            }
        }

        //If the player is not in the melee attack range, return false
        return false;
    }

    public bool isPlayerDetected()
    {
        //Create an array of colliders of all gameObjects in this enemies detection range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange);

        //Iterate through hits and check if any of them were the player
        for (int i = 0; i < hits.Length; i++)
        {
            //If one of the hits was the player, assign the player variable and return true
            if (hits[i].gameObject.CompareTag("Player"))
            {
                player = hits[i].transform;
                //Debug.Log("Player detected!"); 
                //Since the player was detected, return true
                return true;
            }
        }

        //If the player was not detected, return false
        return false;
    }
    public void ChangeHealth(float amount)
    {
        health += amount;
        Debug.Log("Creature " + gameObject.name + " has taken " + amount + " damage!");
        if (health <= 0)
        {
            Debug.Log("Creature " + gameObject.name + " has died!");
            Destroy(gameObject);
        }
    }

    float aimAngle;
    Vector2 aimDirection;
    public void UpdateAim()
    {
        aimDirection = (player.position - transform.position).normalized;
        aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, aimAngle);
    }
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        //Stop charging if you collide with an object
        if (isDashing)
        {
            dashCounter = 0;
            dashCoolCounter = dashCooldown;
            isDashing = false;
            m_animator.enabled = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
