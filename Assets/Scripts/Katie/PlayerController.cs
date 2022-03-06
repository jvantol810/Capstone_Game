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

    private Vector2 moveInput;
    float horizontal;
    float vertical;

    public Vector2 lookDirection = new Vector2(1, 0);

    public GameObject projectilePrefab;
    public float launchForce;

    public GameObject webPrefab;
    public float webForce;

    public GameObject bombPrefab;
    public float bombForce;

    public GameObject player;
    private Animator m_animator;

    public CreatureTypes CreatureState;

    public bool chargingEnabled;
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int direction;

    [SerializeField]
    public Queue<Powers> playerPowers = new Queue<Powers>();
    private int maxNumberOfPowers = 2;

    public void AddPower(Powers power)
    {
        //Add the power if it isn't already in the power list
        if (playerPowers.Contains(power) == false)
        {
            if(playerPowers.Count >= maxNumberOfPowers)
            {
                playerPowers.Dequeue();
            }
            playerPowers.Enqueue(power);
        }
    }

    bool HasPower(Powers power)
    {
        return playerPowers.Contains(power);
    }

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

        dashTime = startDashTime;

    }

    // Update is called once per frame
    void Update()
    {
        //get input from user
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        //Vector2 move = new Vector2(moveInput.x, moveInput.y);

        //Check if mouse button is pressed. If so, execute power one.
        /*if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Do power one");
        }
        */
        //Check if you need to switch to the moving state
        //if (moveInput != Vector2.zero)
        //{
        //    m_animator.SetTrigger("PlayerMove");
        //}
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    AddPower(Powers.ShootWeb);
        //}
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    AddPower(Powers.SlashBig);
        //}
        if (moveInput != Vector2.zero)
        {
            CreatureActions.Move(rigidbody2d, moveInput, speed);
        }

        //set look direction of sprite
        if (!Mathf.Approximately(moveInput.x, 0.0f) || !Mathf.Approximately(moveInput.y, 0.0f))
        {
            lookDirection.Set(moveInput.x, moveInput.y);
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

        //check if Dash is in th Powers queue
        if (HasPower(Powers.Dash))
        {
            //If player is not currently dashing
            if (direction == 0)
            {
                //set direction based on where the player is facing
                if (Input.GetKeyDown(KeyCode.C) && lookDirection.x == -1)
                {
                    direction = 1;
                }
                else if (Input.GetKeyDown(KeyCode.C) && lookDirection.x == 1)
                {
                    direction = 2;
                }
                else if (Input.GetKeyDown(KeyCode.C) && lookDirection.y == 1)
                {
                    direction = 3;
                }
                else if (Input.GetKeyDown(KeyCode.C) && lookDirection.y == -1)
                {
                    direction = 4;
                }
            }
            else
            {
                //set everything back to normal when the player finishes dashing
                if (dashTime <= 0)
                {
                    direction = 0;
                    Debug.Log(direction);
                    dashTime = startDashTime;
                    rigidbody2d.velocity = Vector2.zero;
                }
                else
                {
                    //decrement the dash timer
                    dashTime -= Time.deltaTime;

                    //make player dash in direction they're facing
                    if (direction == 1)
                    {
                        rigidbody2d.velocity = Vector2.left * dashSpeed;
                    }
                    else if (direction == 2)
                    {
                        rigidbody2d.velocity = Vector2.right * dashSpeed;
                    }
                    else if (direction == 3)
                    {
                        rigidbody2d.velocity = Vector2.up * dashSpeed;
                    }
                    else if (direction == 4)
                    {
                        rigidbody2d.velocity = Vector2.down * dashSpeed;
                    }
                }
            }

        }
        else if (HasPower(Powers.ShootWeb))
        {
            if (Input.GetMouseButtonDown(0))
            {
                ActivatePower(Powers.ShootWeb);
            }
        }
        else if (HasPower(Powers.Explode))
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                ActivatePower(Powers.Explode);
                Debug.Log("Throwing bomb");
            }
        }


    }

    void FixedUpdate()
    {
        
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

    void ActivatePower(Powers power)
    {
        switch (power)
        {
            case Powers.Dash:
                //Do the dash
                break;
            case Powers.ShootWeb:
                //Shoot web
                GameObject web = Instantiate(webPrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

                ShootWeb shootWeb = web.GetComponent<ShootWeb>();
                
                //shoot web in direction player is facing
                shootWeb.Launch(lookDirection, webForce);
                break;
            case Powers.Explode:
                //Slash big
                GameObject bomb = Instantiate(bombPrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

                Explosion dropBomb = bomb.GetComponent<Explosion>();

                //throw bomb in direction player is facing
                dropBomb.Launch();
                break;
        }
    }
}
