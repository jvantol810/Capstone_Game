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

    public GameObject player;
    private Animator m_animator;

    public CreatureTypes CreatureState;

    public bool chargingEnabled;
    public float dashLength = 5f;
    public float dashCooldown = 1f;
    public float dashCoolCounter;
    private float activeMoveSpeed;
    public float dashSpeed = 14;

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
        activeMoveSpeed = speed;

        dashCoolCounter = dashLength;
    }

    Vector2 lastDirectionFaced = Vector2.zero;
    bool isDashing = false;
    // Update is called once per frame
    void Update()
    {
        //get input from user
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        //Vector2 move = new Vector2(moveInput.x, moveInput.y);
        
        //Check if you need to switch to the moving state
        if (moveInput != Vector2.zero)
        {
            m_animator.SetTrigger("PlayerMove");
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

        //if (Input.GetKeyDown("space"))
        //{
        //    //Fire a projectile at the enemy, possessing them on contact
        //    Launch();
        //}
        
        //rigidbody2d.velocity = moveInput * activeMoveSpeed;
        //charching doesn't work when I set keycode to c, but it does work when I set it to space

        Debug.Log("I'm moving in this direction: " + "( " + moveInput.x + ", " + moveInput.y + ")");
        
        if (Input.GetKeyDown("space"))
        {

            isDashing = true;
           
        }

        if (isDashing && dashCoolCounter > 0)
        {

            //Cache the direction they were in when they executed the dash
            if (lastDirectionFaced == Vector2.zero)
            {

                lastDirectionFaced = moveInput;
                Debug.Log("Ima dashin in this direction: " + lastDirectionFaced);
            }

            //Move player in that direction
            rigidbody2d.MovePosition(rigidbody2d.position + Vector2.right * speed * 20f);

            //Inc counter
            dashCoolCounter -= Time.deltaTime;
        }
        else if (dashCoolCounter <= 0 && isDashing)
        {
            isDashing = false;
            dashCoolCounter = dashLength;
            lastDirectionFaced = Vector2.zero;
        }
        //if(Input.GetKeyDown("space"))
        //{
        //    if(dashCoolCounter <= 0 && dashCounter <= 0)
        //    {
        //        activeMoveSpeed = dashSpeed;
        //        dashCounter = dashLength;
        //    }
        //}

        //if(dashCounter > 0)
        //{
        //    dashCounter -= Time.deltaTime;

        //    if(dashCounter <= 0)
        //    {
        //        activeMoveSpeed = speed;
        //        dashCoolCounter = dashCooldown;
        //    }
        //}

        //if(dashCoolCounter > 0)
        //{
        //    dashCoolCounter -= Time.deltaTime;
        //}

        /*
        if (chargingEnabled)
        {
            //charge left
            if(Input.GetKeyDown(KeyCode.A)) {
                if(doubleTapTime > Time.time && lastKeyCode == KeyCode.A){
                    StartCoroutine(Dash(-1f));
                }else{
                    doubleTapTime = Time.time + 0.5f;
                }

                lastKeyCode = KeyCode.A;
            }
            //charge right
            if (Input.GetKeyDown(KeyCode.D)) {
                if (doubleTapTime > Time.time && lastKeyCode == KeyCode.D){
                    StartCoroutine(Dash(1f));
                } else {
                    doubleTapTime = Time.time + 0.5f;
                }

                lastKeyCode = KeyCode.D;
            }
            /*
            //charge up
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (doubleTapTime > Time.time && lastKeyCode == KeyCode.W)
                {
                    StartCoroutine(Dash(1f));
                }
                else
                {
                    doubleTapTime = Time.time + 0.5f;
                }

                lastKeyCode = KeyCode.W;
            }
            //charge down
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (doubleTapTime > Time.time && lastKeyCode == KeyCode.S)
                {
                    StartCoroutine(Dash(-1f));
                }
                else
                {
                    doubleTapTime = Time.time + 0.5f;
                }

                lastKeyCode = KeyCode.S;
            }
            */

    }

    void FixedUpdate()
    {
    /*
    //use position to move sprite
    if(!isCharging)
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }*/
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

    public void Charge(Vector2 direction, float force)
    {
        Debug.Log("charging");
        rigidbody2d.AddForce(direction * force, ForceMode2D.Impulse);
        //rigidbody2d.AddForce(direction * force);
    }


}
