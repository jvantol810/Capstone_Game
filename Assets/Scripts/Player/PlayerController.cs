using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEditor;
using TMPro;
public class PlayerController : MonoBehaviour
{
    public float baseSpeed = 7f;
    public float currentSpeed;
    public int maxHealth = 5;
    public int health { get { return currentHealth; } }
    public int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    Animator animator;

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

    public GameObject meleePrefab;

    public GameObject player;
    private Animator m_animator;

    public float dashSpeed;
    public float dashLength, dashCooldown;
    private float dashCounter;
    private float dashCoolCounter;
    public bool isDashing;
    private Vector2 direction;

    [SerializeField]
    public Queue<Powers> playerPowers = new Queue<Powers>();
    public TextMeshProUGUI powersDisplay;
    private int maxNumberOfPowers = 2;
    public string powersText { get { return GetPowersText(); } }
    public Transform firePoint;
    public ObjectPool bombPool;

    //Knockback settings
    //[HideInInspector]
    public bool isBeingKnockedBack;
    [HideInInspector]
    public Vector2 knockbackForce;
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
            powersDisplay.text = powersText;
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

        currentSpeed = baseSpeed;

        animator = GetComponent<Animator>();

        //AddPower(Powers.Dash);
        //AddPower(Powers.ShootWeb);
        AddPower(Powers.Explode);
    }

    public string GetPowersText()
    {
        string text = "Powers: ";
        foreach (Powers power in playerPowers)
        {
            text += power.ToString() + ", ";
        }
        return text;
    }

  
    // Update is called once per frame
    void Update()
    {
        //Update the aim
        UpdateAim();

        //get input from user
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");


        if (isBeingKnockedBack)
        {
            rigidbody2d.MovePosition(transform.position + (Vector3)knockbackForce);
        }

        //Update the look direction but only if the player is not dashing
        if (!Mathf.Approximately(moveInput.x, 0.0f) || !Mathf.Approximately(moveInput.y, 0.0f))
        {
            if (!isDashing && dashCoolCounter <= 0)
            {
                lookDirection.Set(moveInput.x, moveInput.y);
                lookDirection.Normalize();

                animator.SetFloat("Look X", lookDirection.x);
                animator.SetFloat("Look Y", lookDirection.y);
            }
        }

        //Set invincible timer after hit
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown("space"))
        {
            //Fire a projectile at the enemy, possessing them on contact
            FirePossession();
        }

        if (dashCounter > 0)
        {
            CreatureActions.Move(rigidbody2d, lookDirection, dashSpeed);
            dashCounter -= Time.deltaTime;
            if(dashCounter <= 0)
            {
                dashCoolCounter = dashCooldown;
                isDashing = false;
            }
        }
        if(dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0))
        {
            meleePrefab.SetActive(true);
            meleePrefab.transform.position = transform.position + firePoint.right;
        }
        if (HasPower(Powers.Dash))
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                ActivatePower(Powers.Dash);
            }
        }
        //if (HasPower(Powers.ShootWeb))
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        ActivatePower(Powers.ShootWeb);
        //    }
        //}
        if (HasPower(Powers.Explode))
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                ActivatePower(Powers.Explode);
                //Debug.Log("Throwing bomb");
            }
        }
        //Debug.Log("Dash counter: " + dashCounter);

    }

    void FixedUpdate()
    {
        if (moveInput != Vector2.zero && isDashing == false && dashCoolCounter <= 0)
        {
            CreatureActions.Move(rigidbody2d, moveInput, currentSpeed);
        }
    }

    private void OnDrawGizmos()
    {
        //DisplayPowers();
    }

    
    public void DisplayPowers()
    {
        UnityEditor.Handles.color = Color.green;
        Handles.Label(new Vector3(transform.position.x, transform.position.y + 1f, 0), powersText);
    }

    public void Hit(int damage, StatusEffect knockback)
    {
        //Apply knockback in the direction of the hit
        GetComponent<PlayerStatusEffectHandler>().AddStatusEffect(knockback);
        //Update your health amount
        ChangeHealth(-damage);
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

    void FirePossession()
    {
        GameObject possessionObject = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Possession possession = possessionObject.GetComponent<Possession>();

        //throw projectile in direction player is facing

        possession.Launch(firePoint.right, launchForce);

        //animator.SetTrigger("Launch");
        //PlaySound(throwingClip);
    }

    void ActivatePower(Powers power)
    {
        switch (power)
        {
            case Powers.Dash:
                //Do the dash
                if(dashCoolCounter <= 0 && dashCounter <= 0)
                {
                    dashCounter = dashLength;
                    isDashing = true;
                }
                break;
            case Powers.ShootWeb:
                //Shoot web
                GameObject webObject = Instantiate(webPrefab, transform.position, Quaternion.identity);
                Web web = webObject.GetComponent<Web>();
                web.speed = webForce;
                //shoot web in direction player is facing
                web.Launch(aimDirection);
                break;
            case Powers.Explode:
                //Slash big
                GameObject bomb = bombPool.GetPooledObject();
                bomb.transform.position = new Vector3(firePoint.position.x + 0.25f, firePoint.position.y, 0);
                Explosion dropBomb = bomb.GetComponent<Explosion>();

                //throw bomb in direction player is facing
                dropBomb.Launch();
                break;
        }
    }

    float aimAngle;
    Vector2 mousePosition;
    Vector2 aimDirection;
    public void UpdateAim()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimDirection = (mousePosition - (Vector2)transform.position).normalized;
        aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, aimAngle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing)
        {
            dashCounter = 0;
            dashCoolCounter = dashCooldown;
            isDashing = false;
        }
    }
}
