using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    //Values for damage
    public int damage;
    public float attackCooldown;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float attackCooldownCounter;
    public float knockbackForce;
    public float attackRadius;
    public Vector2 attackOffset;
    public LayerMask whatCanBeHit;
    void Start()
    {
        attackCooldownCounter = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    private void OnEnable()
    {
        //Attack();
    }

    // Update is called once per frame
    void Update()
    {
        attackCooldownCounter -= Time.deltaTime;
    }

    public void Attack()
    {
        //If we are currently on cooldown, don't execute the attack
        if(attackCooldownCounter > 0) { return; }

        spriteRenderer.enabled = true;
        animator.enabled = true;
        animator.Play("melee_slash", 0, 0f);
        
       

        //Create a circle at the attack position with the range given for melee attacks, and store the hit colliders in an array called hits
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + (Vector3)attackOffset, attackRadius, whatCanBeHit);

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
            hitDirection = (hits[i].transform.position - transform.parent.position).normalized;
            meleeKnockback.vectorValue = hitDirection * (knockbackForce * 3);
            //If the hit has a player controller, hit them.
            if (obj.GetComponent<PlayerController>() != null)
            {
                obj.GetComponent<PlayerController>().Hit(damage, meleeKnockback);
            }
            //If the hit has a creature controller, hit them.
            else if (obj.GetComponent<CreatureStats>() != null)
            {
                obj.GetComponent<CreatureStats>().Hit(damage, meleeKnockback);
                //obj.GetComponent<CreatureStatusEffectHandler>().AddStatusEffect(meleeKnockback);
            }
        }
    }

    public void StopAttack()
    {
        Debug.Log("Stop attack!");
        spriteRenderer.enabled = false;
        animator.enabled = false;
        attackCooldownCounter = attackCooldown;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position + (Vector3)attackOffset, attackRadius);
    }
}
