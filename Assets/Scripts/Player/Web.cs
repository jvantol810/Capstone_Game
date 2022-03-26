using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Web : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite explodedWebSprite;
    Rigidbody2D rigidbody2d;
    Collider2D collider;
    SpriteRenderer renderer;
    Vector2 startingPosition;
    float distanceTraveled = 0f;
    bool move = false;
    Vector2 moveDirection;
    float speed;
    [Header("Settings")]
    public float travelDistance;
    public float speedReduction;
    public float webRadius = 0.5f;
    StatusEffect slowEffect;
    bool slowdown;
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        startingPosition = transform.position;
        slowEffect = new StatusEffect(StatusEffectTypes.Slowed, gameObject, speedReduction, false);
    }

    public void Launch(Vector2 direction, float speed)
    {
        move = true;
        moveDirection = direction;
        this.speed = speed;
    }

    
    void FixedUpdate()
    {
        if (transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }

        if (move)
        {
            rigidbody2d.MovePosition(transform.position + (Vector3)(moveDirection * speed * Time.fixedDeltaTime));
            distanceTraveled += Vector2.Distance(startingPosition, transform.position);

            if (distanceTraveled >= travelDistance)
            {
                //Debug.Log("MAX DISTANCE REACHED!");
                rigidbody2d.angularVelocity = 0;
                ExplodeWeb();
            }
        }

        else
        {
            CheckContacts();
        }
       
        
    }

    //Transform the web from a projectile into a static obstacle in the scene
    void ExplodeWeb()
    {
        //Set the sprite to be the exploded version of the web, rather than the projectile version
        renderer.sprite = explodedWebSprite;
        //Make it so the sprite slows the player when they walk over it
        slowdown = true;
        move = false;
        collider.isTrigger = true;
    }
    bool isTouchingPlayer = false;
    List<GameObject> lastTouchedObjects = new List<GameObject>();
    PlayerStatusEffectHandler player;
    public void CheckContacts()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, webRadius);
        foreach (Collider2D hit in hits)
        {
            GameObject obj = hit.gameObject;
            if (obj.CompareTag("Player"))
            {
                isTouchingPlayer = true;
                //Debug.Log("Player touched the web!");
                player = obj.GetComponent<PlayerStatusEffectHandler>();
                player.AddStatusEffect(slowEffect);
                break;
            }
            //If the player is not in contact with the web, but the Web was previously touching the player, then remove the slowdown status effect from the player.
            else if(isTouchingPlayer == true) {
                player.RemoveStatusEffect(slowEffect);
                isTouchingPlayer = false;
            }
            if (obj.CompareTag("Enemy"))
            {
                Debug.Log("Enemy touched the web!");
                CreatureController enemy = obj.GetComponent<CreatureController>();
                enemy.AddStatusEffect(slowEffect);
                lastTouchedObjects.Add(obj);
                break;
            }
            //If the enemy is not in contact with the web, but the Web was previously touching the enemy, then remove the slowdown status effect from the enemy.
            else if (lastTouchedObjects.Contains(obj))
            {
                obj.GetComponent<CreatureController>().RemoveStatusEffect(slowEffect);
                lastTouchedObjects.Remove(obj);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, webRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.CompareTag("Player"))
        {
            isTouchingPlayer = true;
            //Debug.Log("Player touched the web!");
            player = collision.GetComponent<PlayerStatusEffectHandler>();
            player.AddStatusEffect(slowEffect);
        }
        else if (collision.CompareTag("Enemy"))
        {
            CreatureController enemy = collision.GetComponent<CreatureController>();
            enemy.AddStatusEffect(slowEffect);
        }
        //if (slowdown)
        //{

        //    GameObject obj = collision.gameObject;
        //    switch (obj.tag)
        //    {
        //        case "Player":
        //            //Debug.Log("Player touched the web!");
        //            obj.GetComponent<PlayerController>().AddStatusEffect(slowEffect);
        //            break;
        //    }
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            isTouchingPlayer = true;
            //Debug.Log("Player touched the web!");
            player = collision.GetComponent<PlayerStatusEffectHandler>();
            player.RemoveStatusEffect(slowEffect);
        }
        else if (collision.CompareTag("Enemy"))
        {
            CreatureController enemy = collision.GetComponent<CreatureController>();
            enemy.RemoveStatusEffect(slowEffect);
        }
    }

}
