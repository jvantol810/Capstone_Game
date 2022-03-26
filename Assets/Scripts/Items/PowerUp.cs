using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float speedMultiplier = 2f;
    public float duration = 4f;
    public int healthBoost = 1;
    [SerializeField]
    public StatusEffect powerupEffect;
    void OnTriggerEnter2D(Collider2D other)
    {
        //detect of the player collides with the item
        if(other.CompareTag("Player"))
        {
            //start coroutine by calling the Pickup function and passing the player object through
            StartCoroutine(Pickup(other));
        }
    }

    IEnumerator Pickup(Collider2D playerCollider)
    {
        //get player script reference
        PlayerStatusEffectHandler player = playerCollider.GetComponent<PlayerStatusEffectHandler>();

        player.AddStatusEffect(powerupEffect);
        //if else statements check which kind of powerup the player has collided with by the tag
        //if(this.CompareTag("speed"))
        //{

        //}
        //else if(this.CompareTag("health"))
        //{
        //    stats.currentHealth += healthBoost;
        //}

        //make the collectible sprite disappear
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        //wait to continue with function
        //yield return new WaitForSeconds(duration);

        ////undo what the powerup did
        ////if (this.CompareTag("speed"))
        ////{
        ////    stats.currentSpeed /= speedMultiplier;
        ////}

        ////destroy the powerup
        //Destroy(gameObject);
        yield return null;
    }
}
