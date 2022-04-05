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
    public ParticleSystem bombParticleSystem;
    private SpriteRenderer m_renderer;
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        m_renderer = GetComponent<SpriteRenderer>();
        //m_renderer.enabled = true;
    }

    private void Start()
    {
        m_renderer.enabled = true;
    }

    public void Detonate()
    {
        StartCoroutine(Explode());
    }
    
    IEnumerator Explode()
    {
        //wait to continue with function
        yield return new WaitForSeconds(duration);

        //Set the explosion bool to be true

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
        //GetComponent<SpriteRenderer>().enabled = false;
        //GetComponent<Collider2D>().enabled = false;

        //Enable the explosion
        bombParticleSystem.Play();

        //Disable the bomb
        yield return new WaitForSeconds(bombParticleSystem.main.duration);
        m_renderer.enabled = false;
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fieldOfImpact);
    }
}
