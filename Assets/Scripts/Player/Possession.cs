using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
public class Possession : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    void Update()
    {
        if (transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }

    
    void OnCollisionEnter2D(Collision2D other)
    {

        //Get the PlayerController script from the player in the scene
        PlayerController Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (!other.collider.gameObject.TryGetComponent(out CreatureStats Creature))
        {
            Destroy(gameObject);
            return;
        }
        //Get the type of the creature that the player hit
        switch (Creature.creatureType)
        {
            case CreatureTypes.Minotaur:
                //add dash power to the queue
                Player.AddPower(Powers.Dash);
                Debug.Log("Charging enabled");
                break;
            case CreatureTypes.Spider:
                Player.AddPower(Powers.ShootWeb);
                Debug.Log("Shoot web enabled!"/*Powers.ShootWeb*/);
                break;
            case CreatureTypes.Bomb:
                Player.AddPower(Powers.Explode);
                Debug.Log("Explosion enabled!"/*Powers.Explode*/);
                break;
            default:
                Destroy(gameObject);
                break;
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        //Get the PlayerController script from the player in the scene
        PlayerController Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        if (!other.gameObject.TryGetComponent(out CreatureStats Creature))
        {
            Destroy(gameObject);
            return;
        }
        //Get the type of the creature that the player hit
        switch (Creature.creatureType)
        {
            case CreatureTypes.Minotaur:
                //add dash power to the queue
                Player.AddPower(Powers.Dash);
                Debug.Log("Charging enabled");
                break;
            case CreatureTypes.Spider:
                Player.AddPower(Powers.ShootWeb);
                Debug.Log("Shoot web enabled!");
                break;
            case CreatureTypes.Bomb:
                Player.AddPower(Powers.Explode);
                Debug.Log("Explosion enabled!");
                break;
            default:
                Destroy(gameObject);
                break;
        }
        Destroy(gameObject);
    }
}
