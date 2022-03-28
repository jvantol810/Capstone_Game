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

}
