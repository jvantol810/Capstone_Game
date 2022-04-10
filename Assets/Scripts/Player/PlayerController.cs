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
    [HideInInspector]
    public bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    Animator animator;
    PlayerAim playerAim;

    private Vector2 moveInput;
    float horizontal;
    float vertical;

    public Vector2 lookDirection = new Vector2(1, 0);
    [Header("Possession")]
    public GameObject projectilePrefab;
    public float launchForce;
    [Header("Web Power")]
    public ObjectPool webPool;
    public float webForce;
    [Header("Bomb Power")]
    public ObjectPool bombPool;
    //public GameObject bombPrefab;
    //public float bombForce;
    [Header("Melee Attack")]
    public MeleeAttack meleeAttack;
    public float meleeOffset;
    [Header("Invincibility")]
    public float hitInvincibilityLength;
    public GameObject player;
    private Animator m_animator;
    [Header("Default Dash Stats")]
    public float defaultDashSpeed;
    public float defaultDashLength, defaultDashCooldown;
    [Header("Minotaur Dash Stats")]
    public float minotaurDashSpeed;
    public float minotaurDashLength;
    public float minotaurDashKnockbackForce;
    public float minotaurDashDamage;
    [Header("Post Dash Collision Stun Duration")]
    public float dashStunLength;
    private float dashStunCounter;
    private float currentDashLength, currentDashSpeed, currentDashCooldown;
    private float dashCounter = 0;
    private float dashCoolCounter;
    public bool isDashing;
    public enum DashTypes
    {
        Default,
        Minotaur
    }
    private DashTypes DashType = DashTypes.Default;
    private Vector2 dashDirection;
    
    [SerializeField]
    public Queue<Powers> playerPowers = new Queue<Powers>();
    public TextMeshProUGUI powersDisplay;
    public TextMeshProUGUI healthDisplay;
    private int maxNumberOfPowers = 2;
    public string powersText { get { return GetPowersText(); } }
    public Transform firePoint;
    //public ObjectPool bombPool;

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
            //If the dash power is being added, update the dash settings
            if(power == Powers.Dash)
            {
                Debug.Log("update dash speed");
                SetDashStats(minotaurDashLength, minotaurDashSpeed, defaultDashCooldown);
            }
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
        SetDashStats(defaultDashLength, defaultDashSpeed, defaultDashCooldown);
        //Create a reference to the player's rigidbody
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        //Create a reference to the player's animator controller
        m_animator = GetComponent<Animator>();

        currentSpeed = baseSpeed;

        animator = GetComponent<Animator>();

        playerAim = GetComponent<PlayerAim>();

        AddPower(Powers.Dash);
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
        //UpdateAim();
        healthDisplay.text = "Health: " + currentHealth;

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
            if (!isDashing && dashStunCounter <= 0)
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
            Debug.Log("Dashing!");
            CreatureActions.Move(rigidbody2d, lookDirection, currentDashSpeed);
            dashCounter -= Time.deltaTime;
            if(dashCounter <= 0)
            {
                dashCoolCounter = currentDashCooldown;
                dashStunCounter = 0;
                isDashing = false;
            }
        }

        if(dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }

        if(dashStunCounter > 0)
        {
            dashStunCounter -= Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0))
        {
            MeleeAttack();
        }

        //if (HasPower(Powers.Dash))
        //{
        //    if (Input.GetKeyDown(KeyCode.C))
        //    {
        //        ActivatePower(Powers.Dash);
        //    }
        //}
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (HasPower(Powers.Dash))
            {
                ActivatePower(Powers.Dash);
            }
            else
            {
                StartDash(defaultDashLength, defaultDashSpeed, defaultDashCooldown);
            }
        }
        if (HasPower(Powers.ShootWeb))
        {
            if (Input.GetMouseButtonDown(0))
            {
                ActivatePower(Powers.ShootWeb);
            }
        }
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
        if (moveInput != Vector2.zero && isDashing == false && dashStunCounter <= 0)
        {
            CreatureActions.Move(rigidbody2d, moveInput, currentSpeed);
        }
    }

    private void OnDrawGizmos()
    {
        //DisplayPowers();
    }

    public void StartDodge()
    {
        dashDirection = playerAim.aimDirection;
        if (dashCoolCounter <= 0 && dashCounter <= 0)
        {
            dashCounter = currentDashLength;
            isDashing = true;
        }
    }
    public void MeleeAttack()
    {
        //meleePrefab.SetActive(true);
        meleeAttack.transform.rotation = Quaternion.Euler(0, 0, playerAim.aimAngle);
        meleeAttack.transform.position = transform.position + firePoint.right * meleeOffset;
        meleeAttack.Attack();
    }
#if UNITY_EDITOR
    public void DisplayPowers()
    {
        UnityEditor.Handles.color = Color.green;
        Handles.Label(new Vector3(transform.position.x, transform.position.y + 1f, 0), powersText);
    }
#endif
    public void DisplayHealth()
    {
        UnityEditor.Handles.color = Color.green;
        Handles.Label(new Vector3(transform.position.x, transform.position.y + 1f, 0), powersText);
    }


    public void Hit(int damage, StatusEffect knockback)
    {
        if (isInvincible) { return; }
        if(isDashing) { return; }

        //Update your health amount
        ChangeHealth(-damage);

        //Apply knockback in the direction of the hit
        GetComponent<PlayerStatusEffectHandler>().AddStatusEffect(knockback);

        //Add invincibility
        GetComponent<PlayerStatusEffectHandler>().AddStatusEffect(new StatusEffect(StatusEffectTypes.Invincible, hitInvincibilityLength));
    }

    public void ChangeHealth(int amount)
    {
        //Debug.Log("Health decreased by amount: " + amount);

        if (amount <= 0)
        {
            //animator.SetTrigger("Hit");

            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            //PlaySound(hitClip);
        }
        //Debug.Log("Health decreased by amount: " + amount);
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        //Debug.Log(currentHealth);
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
                //if(dashCoolCounter <= 0 && dashCounter <= 0)
                //{
                //    Debug.Log("do the thing");
                //    dashDirection = playerAim.aimDirection;
                //    dashCounter = currentDashLength;
                //    isDashing = true;
                //}
                StartDash(minotaurDashLength, minotaurDashSpeed, defaultDashCooldown);
                break;
            case Powers.ShootWeb:
                //Shoot web
                Vector3 webSpawnPosition = new Vector3(firePoint.position.x + 0.25f, firePoint.position.y, 0);
                GameObject webObj = webPool.GetPooledObject(webSpawnPosition);
                Web web = webObj.GetComponent<Web>();
                web.speed = webForce;
                //shoot web in direction player is facing
                web.Launch(playerAim.aimDirection);
                break;
            case Powers.Explode:
                Vector3 bombSpawnPosition = new Vector3(firePoint.position.x + 0.25f, firePoint.position.y, 0);
                GameObject bombObj = bombPool.GetPooledObject(bombSpawnPosition);
                Bomb bomb = bombObj.GetComponent<Bomb>();

                //throw bomb in direction player is facing
                bomb.Detonate();
                break;
        }
    }


    public void StopDash()
    {
        dashCounter = 0;
        dashCoolCounter = currentDashCooldown;
        isDashing = false;
    }

    public void StartDash(float dashLength, float dashSpeed, float dashCooldown)
    {
        if (dashCoolCounter <= 0 && dashCounter <= 0)
        {
            SetDashStats(dashLength, dashSpeed, dashCooldown);
            Debug.Log("do the thing");
            dashDirection = playerAim.aimDirection;
            dashCounter = currentDashLength;
            isDashing = true;
        }
    }

    public void SetDashStats(float dashLength, float dashSpeed, float dashCooldown)
    {
        currentDashLength = dashLength;
        currentDashSpeed = dashSpeed;
        currentDashCooldown = dashCooldown;
        this.dashCounter = currentDashLength;
        this.dashCoolCounter = currentDashCooldown;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isDashing)
        {
            //dashCounter = 0;
            //dashCoolCounter = dashCooldown;
            //isDashing = false;
            if (collision.gameObject.CompareTag("Enemy") && HasPower(Powers.Dash))
            {
                Debug.Log("Dashed into an enemy!");
                StatusEffect dashKnockback = new StatusEffect(StatusEffectTypes.Knockback, (collision.transform.position - transform.position).normalized * minotaurDashKnockbackForce, false, 0.2f);
                collision.gameObject.GetComponent<CreatureStats>().Hit(minotaurDashDamage, dashKnockback);
            }
            dashCounter = 0;
            dashCoolCounter = currentDashCooldown;
            dashStunCounter = dashStunLength;
            isDashing = false;
        }
    }
}
