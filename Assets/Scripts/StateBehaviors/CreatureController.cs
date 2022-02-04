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
    public int health;
    public float speed;
    public float damage;
    [Header("Detection")]
    public float detectionRange;
    [Header("Melee Attacks")]
    //public Transform meleeAttackPoint;
    public float meleeAttackRange;
    public LayerMask whatCanBeHitByMelee;
    [Header("Swing Attacks")]
    public Transform weapon;
    public float swingSpeed;
    private Animator weaponAnimator;
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

    //[Header("Possession")]
    //public bool possessed = false;




    // Start is called before the first frame update
    void Start()
    {
        //Set references
        m_rigidbody = GetComponent<Rigidbody2D>();
        //weaponAnimator = weapon.GetComponent<Animator>();
        m_animator = GetComponent<Animator>();

        //Go through each event assigned in the inspector window and add listeners 
        //foreach (UnityEvent attackEvent in attackEvents)
        //{
        //    attackEvent.AddListener(SetAnimatorStateToAttack);
        //    Debug.Log("Adding listener to event: " + attackEvent.GetPersistentMethodName(0));
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("space"))
        {
            

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

        if (swingAttacking)
        {
            weaponAnimator.SetBool("Swing", true);
            swingAttacking = false;
        }
    }

    //public string GetCurrentState()
    //{
    //    AnimatorStateInfo currentStateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
    //    if (currentStateInfo.IsName("Wander"))
    //    {
    //        return "Wander";
    //    }
    //    else if (currentStateInfo.IsName("AttackPlayer"))
    //    {
    //        return "Attack";
    //    }
    //    else if (currentStateInfo.IsName("ChasePlayer"))
    //    {
    //        return "Chase";
    //    }
    //    else if (currentStateInfo.IsName("Possessed"))
    //    {
    //        return "Possessed";
    //    }
    //    else return "Not found";
    //}

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

    public void Charge(Vector2 destination)
    {
        //Debug.Log("Charging!");
        if(!charging)
        {
            chargeDestination = destination;
            charging = true;
        }
    }

    public void MeleeAttack()
    {
        //Create a circle at the attack position with the range given for melee attacks, and store the hit colliders in an array called hits
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, meleeAttackRange, whatCanBeHitByMelee);


        //Iterate through hits and check if any of them were the player
        for (int i = 0; i < hits.Length; i++)
        {
            switch (hits[i].gameObject.tag)
            {
                case "Player":/* Debug.Log("Player hit!");*/ break;
                default: Debug.Log("Something hit!"); break;
            }
        }
    }

    public Vector3 SwingAttack()
    {
        //Rotate weapon
        weapon.Rotate(Vector3.forward, swingSpeed * Time.fixedDeltaTime);
        //Return weapon's rotation
        return weapon.rotation.eulerAngles;
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
        m_animator.SetBool("isPossessed", false);
        if (m_animator.GetBool("isChasing") == false)
        {
            m_animator.SetBool("isChasing", true);
            m_animator.SetBool("isAttacking", false);
        }

    }
    public void SetAnimatorStateToWander()
    {
        //Debug.Log("Transitioning to the wander state!");
        m_animator.SetBool("isPossessed", false);
        m_animator.SetBool("isAttacking", false);
        m_animator.SetBool("isChasing", false);
    }

    public bool isPlayerInMeleeAttackRange()
    {
        //Create an array of colliders of all gameObjects in this enemies detection range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, meleeAttackRange);

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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //Stop charging if you collide with an object
        if (charging) { charging = false;  }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
