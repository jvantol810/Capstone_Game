using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class CreatureController : MonoBehaviour
{
    //References to components or other gameobjects
    [Header("References")]
    private Rigidbody2D m_rigidbody;
    //Attributes/enemy stats
    [Header("Attributes")]
    public int health;
    public float speed;
    public float damage;
    [Header("Melee Attacks")]
    public Transform meleeAttackPoint;
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
    //Attacks
    public bool swingAttacking = false;
    public enum creatureState
    {
        wander,
        approach,
        attack,
    }
    [Header("State")]
    //public TextMeshProUGUI stateText;
    public bool possessed = false;
    private creatureState state;



    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        weaponAnimator = weapon.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("space"))
        {
            Charge();
        }
        if (Input.GetMouseButtonDown(0))
        {
            RangedAttack(projectilePrefab, Vector2.right);
        }
        if (Input.GetKeyDown("f"))
        {
            swingAttacking = true;
        }
        if (possessed)
        {
            //Get input from user
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector2 moveDirection = new Vector2(horizontal, vertical);

            Move(moveDirection, speed);
        }
        //stateText.text = state.ToString();
        //stateText.transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, 0);
    }

    private void FixedUpdate()
    {
        if (charging)
        {
            float distance = MoveTowards(new Vector3(6, 0, 0), chargeSpeed);
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
        if(distance <= 0.5) { return distance;  }
        //Otherwise, the creature is a good distance from the destination. Call the move function.
        Move(direction, speed);
        //Return the distance
        return distance;
    }

    public void Charge()
    {
        charging = true;
    }

    public void MeleeAttack()
    {
        //Create a circle at the attack position with the range given for melee attacks, and store the hit colliders in an array called hits
        Collider2D[] hits = Physics2D.OverlapCircleAll(meleeAttackPoint.position, meleeAttackRange, whatCanBeHitByMelee);
        //Iterate through hits and check if any of them were the player
        for (int i = 0; i < hits.Length; i++)
        {
            switch (hits[i].gameObject.tag)
            {
                case "Player": Debug.Log("Player hit!"); break;
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

    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(meleeAttackPoint.position, meleeAttackRange);
    }
}
