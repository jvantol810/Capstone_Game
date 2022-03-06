using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float speedMultiplier = 2f;
    public float duration = 4f;
    public int healthBoost = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        //detect of the player collides with the item
        if(other.CompareTag("Player"))
        {
            //start coroutine by calling the Pickup function and passing the player object through
            StartCoroutine(Pickup(other));
        }
    }

    IEnumerator Pickup(Collider2D player)
    {
        //get player script reference
        PlayerController stats = player.GetComponent<PlayerController>();
        
        //if else statements check which kind of powerup the player has collided with by the tag
        if(this.CompareTag("speed"))
        {
            stats.speed *= speedMultiplier;
        }
        else if(this.CompareTag("health"))
        {
            stats.currentHealth += healthBoost;
        }

        //make the collectible sprite disappear
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        //wait to continue with function
        yield return new WaitForSeconds(duration);

        //undo what the powerup did
        if (this.CompareTag("speed"))
        {
            stats.speed /= speedMultiplier;
        }

        //destroy the powerup
        Destroy(gameObject);
    }
}
