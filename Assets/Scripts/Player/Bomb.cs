using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float duration = 3f;
    public int explosionDamage = 3;
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
        
    }

    public void Detonate()
    {
        StartCoroutine(Explode());
    }
    
    IEnumerator Explode()
    {
        m_renderer.enabled = true;
        //wait to continue with function
        yield return new WaitForSeconds(duration);

        //Set the bomb's sprite to vanish as it explodes
        m_renderer.enabled = false;

        //Construct a list of objects overlapping the explosion.
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, fieldOfImpact, layerToHit);
        
        foreach (Collider2D obj in objects)
        {
            Vector2 direction = (obj.transform.position - transform.position).normalized;
            //Construct knockback status effect based on the explosionForce and direction
            StatusEffect knockback = new StatusEffect(StatusEffectTypes.Knockback, gameObject, direction * explosionForce, false, 1f);
            if (obj.CompareTag("Player"))
            {
                //obj.GetComponent<PlayerStatusEffectHandler>().AddStatusEffect(knockback);
                //obj.GetComponent<PlayerController>().ChangeHealth((int)explosionDamage);
                obj.GetComponent<PlayerController>().Hit(explosionDamage, knockback);
            }
            else if (obj.CompareTag("Enemy"))
            {
                //obj.GetComponent<CreatureStatusEffectHandler>().Hit(knockback);
                obj.GetComponent<CreatureStats>().Hit(explosionDamage, knockback);
            }
            else if (obj.CompareTag("Web"))
            {
                obj.gameObject.SetActive(false);
            }
            
        }

        //make the bomb sprite disappear
        //GetComponent<SpriteRenderer>().enabled = false;
        //GetComponent<Collider2D>().enabled = false;

        //Enable the explosion
        bombParticleSystem.Play();

        //Disable the bomb
        yield return new WaitForSeconds(bombParticleSystem.main.duration);

        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fieldOfImpact);
    }
}
