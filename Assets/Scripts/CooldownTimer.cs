using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CooldownTimer
{
    public float cooldownAmount = 0.5f;

    //Time cooldown should be finished
    private float cooldownCompleteTime;

    //If the cooldown is finished
    public bool cooldownComplete => Time.time > cooldownCompleteTime;

    //Set the cooldown completed time
    public void StartCooldown()
    {
        cooldownCompleteTime = Time.time + cooldownAmount;
    }
}
