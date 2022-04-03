using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float duration = 3f;
    public float explosionDamage = 2f;
    Rigidbody2D rigidbody2d;
    public float fieldOfImpact;
    public float explosionForce;

    public LayerMask layerToHit;
    public GameObject explosion;
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Detonate()
    {
        StartCoroutine(Explode());
    }
    
    IEnumerator Explode()
    {
        //wait to continue with function
        yield return new WaitForSeconds(duration);

        //Enable the explosion gameobject
        explosion.SetActive(true);

        //Construct a list of objects overlapping the explosion.
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, fieldOfImpact, layerToHit);
        
        foreach (Collider2D obj in objects)
        {
            Vector2 direction = (obj.transform.position - transform.position).normalized;
            //Construct knockback status effect based on the explosionForce and direction
            StatusEffect knockback = new StatusEffect(StatusEffectTypes.Knockback, gameObject, direction * (explosionForce / 100), false, 1f);
            if (obj.CompareTag("Player"))
            {
                obj.GetComponent<PlayerStatusEffectHandler>().AddStatusEffect(knockback);
                obj.GetComponent<PlayerController>().ChangeHealth((int)explosionDamage);
            }
            else if (obj.CompareTag("Enemy"))
            {
                obj.GetComponent<CreatureStatusEffectHandler>().AddStatusEffect(knockback);
                obj.GetComponent<CreatureStats>().ChangeHealth(-explosionDamage);
            }
            
        }

        //make the bomb sprite disappear
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        //Disable the bomb
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, fieldOfImpact);
    }
}