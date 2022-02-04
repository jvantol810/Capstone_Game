using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CreatureActions 
{
    //Special actions specific to certain creatures are stored here, such as the minotaur's charge ability.
    //Some generic actions such as Move() are also stored here and are mainly used in the special actions themselves.

    public static class MinotaurActions
    {
        
    }

    /// <summary>This method changes the gameobject's location by
    /// moving it in the given direction at the given speed.
    /// <example>For example:
    /// <code>
    /// Move(Vector2.right, 1.5f);
    /// </code>
    /// results in the gameobject moving in the right direction by an amount of 1.5f.
    /// </example>
    /// </summary>
    public static void Move(Rigidbody2D creatureRigidbody, Vector2 direction, float speed)
    {
        //Debug.Log("Player is moving!");
        //Debug.Log("Move function called.");
        //Get the current position based on the rigidbody
        Vector2 currentPos = creatureRigidbody.position;
        //Multiply direction and speed to get the adjustedMovement value, which will modify the current position
        Vector2 adjustedMovement = direction * speed;
        //Calculate the new position by adding the adjustedMovement to the current position, and multiplying by fixedDeltaTime to correct for different framerates
        Vector2 newPos = currentPos + adjustedMovement * Time.fixedDeltaTime;
        //Set the rigidbody's location to the new position
        creatureRigidbody.MovePosition(newPos);
    }

    /// <summary>This method changes the gameobject's location by
    /// moving it towards the given destination at the given speed.
    /// <example>For example:
    /// <code>
    /// Move(new Vector2(6, 0, 0), 1.5f));
    /// </code>
    /// results in the gameobject moving towards (6, 0, 0) by an amount of 1.5f.
    /// </example>
    /// </summary>
    public static float MoveTowards(Rigidbody2D creatureRigidbody, Vector2 destination, float speed)
    {
        //Debug.Log("Player is moving!");
        //Calculate direction and store it
        Vector2 direction = ((Vector3)destination - (Vector3)creatureRigidbody.position).normalized;
        //Calculate distance between creature and destination
        float distance = Vector2.Distance(creatureRigidbody.position, destination);
        //If the distance is extremely small, return distance and don't move.
        if (distance <= 0.1) { return distance; }
        //Otherwise, the creature is a good distance from the destination. Call the move function.
        Move(creatureRigidbody, direction, speed);
        //Return the distance
        return distance;
    }
}
