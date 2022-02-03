using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /*
     * For possessing enemies
     *  possibly use raycast to detect when you "hit" them
     *      then change player sprite to enemy sprite for specified period of time
     *          and change abilities to match enemy type
     * 
     * 
     * 
     */
    public float speed = 3.0f;

    public int maxHealth = 5;
    public int health { get { return currentHealth; } }
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;

    float horizontal;
    float vertical;

    Vector2 lookDirection = new Vector2(1, 0);

    public GameObject projectilePrefab;
    public float launchForce;

    public GameObject player;
    

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //disable player game object when possessing enemy
        if (SampleEnemy.isPossessed == true)
        {
            player.SetActive(false);
        }
        if (SampleEnemy.isPossessed == false)
        {
            player.SetActive(true);
        }
        
        //get input from user
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

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

        //throw projectile
        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
    }

    void FixedUpdate()
    {
        //use position to move sprite
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
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
