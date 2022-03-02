using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //SampleEnemy e = other.collider.GetComponent<SampleEnemy>();


        //if (e != null)
        //{
        //    SampleEnemy.isPossessed = true;
        //}

        //Get the PlayerController script from the player in the scene
        PlayerController Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Animator PlayerStateMachine = Player.GetComponent<Animator>();
        CreatureController Creature = other.collider.gameObject.GetComponent<CreatureController>();

        //Get the type of the creature that the player hit
        switch (Creature.CreatureType)
        {
            case CreatureTypes.Minotaur:
                //Player.chargingEnabled = true;
                //add dash power to the queue
                Player.AddPower(Powers.Dash);
                Debug.Log("Charging enabled");
                //PlayerStateMachine.SetTrigger("PossessMinotaur");
                break;
            case CreatureTypes.Spider:
                Player.AddPower(Powers.ShootWeb);
                Debug.Log(Powers.ShootWeb);
                break;
            case CreatureTypes.Bomb:
                //PlayerStateMachine.SetTrigger("PossessOgre");
                Player.AddPower(Powers.Explode);
                Debug.Log(Powers.Explode);
                break;
        }

        Destroy(gameObject);
    }
}
