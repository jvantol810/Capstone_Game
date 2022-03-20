using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusEffectTypes
{
    Slowed,
    Speedup,
    Knockback
}
public enum StatusEffectSources
{
    Web,
    Powerup
}
[System.Serializable]
public class StatusEffect
{
    public StatusEffectTypes type;
    //public StatusEffectSources source;
    [HideInInspector]
    public GameObject source;
    [Header("Effectiveness value as a percentage")]
    public float value;
    public Vector2 vectorValue;
    public float duration;
    [HideInInspector]
    public bool stackable;
    [HideInInspector]
    public bool hasDuration { get { return duration > 0f; } }
    public StatusEffect(StatusEffectTypes type, /*StatusEffectSources source,*/ GameObject source, float value, bool stackable, float duration=-1f)
    {
        this.source = source;
        this.type = type;
        this.value = value;
        this.stackable = stackable;
        this.duration = duration;
    }

    public StatusEffect(StatusEffectTypes type, /*StatusEffectSources source,*/ GameObject source, Vector2 vectorValue, bool stackable, float duration = -1f)
    {
        this.source = source;
        this.type = type;
        this.vectorValue = vectorValue;
        this.stackable = stackable;
        this.duration = duration;
    }

    //Effects are considered to be identical if they are of the same type and derived from the same source
    public bool EffectEquals(StatusEffect effect)
    {
        return type == effect.type && source.Equals(effect.source);
    }
}