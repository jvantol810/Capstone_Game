using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CreatureStats))]
[RequireComponent(typeof(Rigidbody2D))]
public class CreatureStatusEffectHandler : MonoBehaviour
{
    public HashSet<StatusEffect> statusEffects = new HashSet<StatusEffect>();
    public SpriteRenderer spriteRenderer;
    private CreatureStats stats;
    private Vector2 knockbackForce;
    public bool isBeingKnockedBack = false;
    private Rigidbody2D m_rigidbody;
    private void Start()
    {
        stats = GetComponent<CreatureStats>();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isBeingKnockedBack)
        {
            m_rigidbody.MovePosition(transform.position + (Vector3)knockbackForce * Time.deltaTime);
        }
    }

    //public void Knockback(Vector2 force)
    //{
    //    m_rigidbody.AddForce(/*transform.position + */force, ForceMode2D.Force);
    //    //isBeingKnockedBack = false;
    //}
    public void AddStatusEffect(StatusEffect effect)
    {
        if (HasStatusEffect(effect.type))
        {
            Debug.Log("Player already has status effect: " + effect.type);
        }
        else
        {
            Debug.Log("Effect added: " + effect.type);
            statusEffects.Add(effect);
            ProcessStatusEffect(effect);
        }
    }

    public void RemoveStatusEffect(StatusEffect effect)
    {
        foreach (StatusEffect statusEffect in statusEffects)
        {
            if (statusEffect.type == effect.type)
            {
                ProcessStatusEffect(effect, true);
            }
        }
        statusEffects.RemoveWhere((item) => item.type == effect.type);
    }

    public void ProcessStatusEffect(StatusEffect effect, bool isBeingRemoved = false)
    {
        if (effect.hasDuration)
        {

        }
        switch (effect.type)
        {
            case StatusEffectTypes.Slowed:
                if (isBeingRemoved)
                {
                    Debug.Log("Slowed status effect being removed from enemy!");
                    stats.currentSpeed += stats.baseSpeed * (effect.value / 100);
                }
                else { stats.currentSpeed -= stats.baseSpeed * (effect.value / 100); };
                break;
            case StatusEffectTypes.Speedup:
                if (isBeingRemoved) { stats.currentSpeed -= stats.baseSpeed * (effect.value / 100); }
                else { stats.currentSpeed += stats.baseSpeed * (effect.value / 100); };
                break;
            case StatusEffectTypes.Knockback:
                //if (!isBeingRemoved)
                //{
                //    Knockback(effect.vectorValue);
                //}
                if (isBeingRemoved)
                {
                    //Reenable the animator
                    GetComponent<Animator>().enabled = true;
                    //Set the sprite to be normal again
                    spriteRenderer.color = Color.white;
                    //Disable knockback effect
                    isBeingKnockedBack = false;
                    //Destroy the bomb gameObject associated with the knockback effect
                    Destroy(effect.source);
                }
                else
                {
                    //Temporarily enable the BoxCollider so that the creature collides with walls
                    //Disable the animator temporarily 
                    GetComponent<Animator>().enabled = false;
                    //Set the sprite to be red
                    //Debug.Log("Set sprite to be red!");
                    spriteRenderer.color = Color.red;
                    //Set the knockback force
                    knockbackForce = effect.vectorValue;
                    //Set isBeingKnockedBack to true
                    isBeingKnockedBack = true;
                    //Remove the effect
                    StartCoroutine(RemoveStatusEffectAfterDelay(effect, effect.duration));
                }
                break;
        }
    }



    public IEnumerator RemoveStatusEffectAfterDelay(StatusEffect effect, float delay)
    {
        //Wait for the passed in time (in seconds)
        yield return new WaitForSeconds(delay);
        //Remove the effect
        RemoveStatusEffect(effect);
    }

    public bool HasStatusEffect(StatusEffectTypes type)
    {
        foreach (StatusEffect effect in statusEffects)
        {
            if (effect.type == type) { return true; }
        }
        return false;
    }

    public StatusEffect GetStatusEffect(StatusEffectTypes type)
    {
        foreach (StatusEffect effect in statusEffects)
        {
            if (effect.type == type) { return effect; }
        }
        return null;
    }
}
