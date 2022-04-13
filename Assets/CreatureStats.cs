using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureStats : MonoBehaviour
{
    public float health;
    public float baseSpeed;
    [HideInInspector]
    public float currentSpeed;
    public float attackDamage;
    public CreatureTypes creatureType;
    [Header("Detection")]
    public float detectionRange;
    private void Start()
    {
        currentSpeed = baseSpeed;
    }

    public void Hit(float damage, StatusEffect knockback)
    {
        GetComponent<CreatureStatusEffectHandler>().AddStatusEffect(knockback);
        ChangeHealth(-damage);
    }

    public void ChangeHealth(float amount)
    {
        health += amount;
        Debug.Log("Creature " + gameObject.name + " has taken " + amount + " damage!");
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Creature " + gameObject.name + " has died!");
        Destroy(gameObject);
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
                //Since the player was detected, return true
                return true;
            }
        }

        //If the player was not detected, return false
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //m_animator.enabled = true;
        if (collision.gameObject.CompareTag("Map"))
        {
            Debug.Log("Touched the map!");
            GetComponent<Rigidbody2D>().mass = 100000000f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {
            Debug.Log("Touched the map!");
            GetComponent<Rigidbody2D>().mass = 1f;
        }
    }


}
