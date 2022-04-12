using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[RequireComponent(typeof(PlayerController))]
public class PlayerStatusEffectHandler : MonoBehaviour
{
    private PlayerController player;
    public HashSet<StatusEffect> statusEffects = new HashSet<StatusEffect>();
    public SpriteRenderer spriteRenderer;
    private void Start()
    {
        player = GetComponent<PlayerController>();
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        DisplaySFX();
    }


    public void DisplaySFX()
    {
        UnityEditor.Handles.color = Color.green;
        Handles.Label(new Vector3(transform.position.x, transform.position.y + 1f, 0), GetStatusEffectsText());
    }
#endif
    public string GetStatusEffectsText()
    {
        string text = "Status EFfects: ";
        foreach (StatusEffect effect in statusEffects)
        {
            text += effect.type + ", ";
        }
        //Debug.Log("Num of effects: " + statusEffects.Count);
        return text;

    }

    //public void AddPowerUpEffect(PowerupTypes type, int value, float duration)
    //{
    //    switch(type):
    //        case: 
    //}
    public void AddPowerUp(PowerupTypes type, float effectValue)
    {
        switch (type)
        {
            case PowerupTypes.Healing:
                player.ChangeHealth((int)effectValue);
                break;
        }
    }
    public void AddStatusEffect(StatusEffect effect)
    {
        if (HasStatusEffect(effect.type))
        {
           // Debug.Log("Player already has status effect: " + effect.type);
        }
        else
        {
            //Debug.Log("Effect added: " + effect.type);
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
                if (isBeingRemoved) { player.currentSpeed += player.baseSpeed * (effect.value / 100); }
                else { player.currentSpeed -= player.baseSpeed * (effect.value / 100); };
                break;
            case StatusEffectTypes.Speedup:
                if (isBeingRemoved) { player.currentSpeed -= player.baseSpeed * (effect.value / 100); }
                else { player.currentSpeed += player.baseSpeed * (effect.value / 100); };
                break;
            case StatusEffectTypes.Invincible:
                if (isBeingRemoved) { player.isInvincible = false; }
                else { player.isInvincible = true; };
                StartCoroutine(RemoveStatusEffectAfterDelay(effect, effect.duration));
                break;
            case StatusEffectTypes.Knockback:
                if (isBeingRemoved)
                {
                    //Set the sprite to be normal again
                    spriteRenderer.color = Color.white;
                    //Reenable the animator
                    //GetComponent<Animator>().enabled = true;
                    //Disable knockback effect
                    player.isBeingKnockedBack = false;
                }
                else
                {
                    //Set the knockback force
                    player.knockbackForce = effect.vectorValue;
                    //Set the sprite to be red
                    //Debug.Log("Set sprite to be red!");
                    spriteRenderer.color = Color.red;
                    //Set isBeingKnockedBack to true
                    player.isBeingKnockedBack = true;
                    //Remove the effect
                    StartCoroutine(RemoveStatusEffectAfterDelay(effect, effect.duration));
                }
                break;
            case StatusEffectTypes.Healing:
                if (!isBeingRemoved)
                {
                    player.ChangeHealth((int)effect.value);
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
