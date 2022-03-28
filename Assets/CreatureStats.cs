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

    public void ChangeHealth(float amount)
    {
        health += amount;
        Debug.Log("Creature " + gameObject.name + " has taken " + amount + " damage!");
        if (health <= 0)
        {
            Debug.Log("Creature " + gameObject.name + " has died!");
            Destroy(gameObject);
        }
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



}
