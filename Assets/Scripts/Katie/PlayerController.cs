using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;

    public int maxHealth = 5;
    public int health { get { return currentHealth; } }
    public int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;

    float horizontal;
    float vertical;

    public Vector2 lookDirection = new Vector2(1, 0);

    public GameObject projectilePrefab;
    public float launchForce;

    public GameObject player;
    private Animator m_animator;

    public CreatureTypes CreatureState;
    // Start is called before the first frame update
    void Start()
    {
        //Create a reference to the player's rigidbody
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        //Create a reference to the player's animator controller
        m_animator = GetComponent<Animator>();

        //Set the creature state to be Ghost on start
        CreatureState = CreatureTypes.Ghost;
    }

    // Update is called once per frame
    void Update()
    {

        //get input from user
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        //Check if you need to switch to the moving state
        if (move != Vector2.zero)
        {
            m_animator.SetTrigger("PlayerMove");
        }

        //set look direction of sprite
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        //set invincible timer after hit
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown("space"))
        {
            //Fire a projectile at the enemy, possessing them on contact
            Launch();
        }
    }

    void FixedUpdate()
    {
        //use position to move sprite
        //Vector2 position = rigidbody2d.position;
        //position.x = position.x + speed * horizontal * Time.deltaTime;
        //position.y = position.y + speed * vertical * Time.deltaTime;

        //rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            //animator.SetTrigger("Hit");

            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            //PlaySound(hitClip);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth);
        //UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        GameObject possessionObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Possession possession = possessionObject.GetComponent<Possession>();

        //throw projectile in direction player is facing

        possession.Launch(lookDirection, launchForce);

        //animator.SetTrigger("Launch");
        //PlaySound(throwingClip);
    }

}
