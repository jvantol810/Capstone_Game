using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    //Values for damage
    public int damage;
    public float knockbackForce;
    public float attackRadius;
    public LayerMask whatCanBeHit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        Attack();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Attack()
    {
        //Create a circle at the attack position with the range given for melee attacks, and store the hit colliders in an array called hits
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius, whatCanBeHit);

        //Calculate the direction of the melee attack for knockback
        Vector2 hitDirection = Vector2.zero;
        
        //Create a knockback effect to apply on the target that's hit
        StatusEffect meleeKnockback = new StatusEffect(
            StatusEffectTypes.Knockback,
            hitDirection,
            false,
            0.2f
            );

        //Iterate through hits and check if any of them were the player
        for (int i = 0; i < hits.Length; i++)
        {
            GameObject obj = hits[i].gameObject;
            hitDirection = (hits[i].transform.position - transform.position).normalized;
            meleeKnockback.vectorValue = hitDirection * (knockbackForce / 10);
            //If the hit has a player controller, hit them.
            if (obj.GetComponent<PlayerController>() != null)
            {
                obj.GetComponent<PlayerController>().Hit(damage, meleeKnockback);
            }
            //If the hit has a creature controller, hit them.
            else if (obj.GetComponent<CreatureController>() != null)
            {
                obj.GetComponent<CreatureController>().ChangeHealth(-damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
