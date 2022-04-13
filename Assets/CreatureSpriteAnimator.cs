using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSpriteAnimator : MonoBehaviour
{
    public float lookX;
    public float lookY;
    public Animator spriteAnimator;
    //His move direction is the difference between his position and his current target's position in world space.
    public Vector2 currentDestination;
    public Vector2 currentDirection;
    private void Update()
    {
        Vector2 position = transform.position;
        currentDirection = (currentDestination - (Vector2)transform.position).normalized;
        spriteAnimator.SetFloat("Look X", currentDirection.x);
        spriteAnimator.SetFloat("Look Y", currentDirection.y);
    }

    public void SetCharging(bool isCharging)
    {
        spriteAnimator.SetBool("Charging", isCharging);
    }
}
