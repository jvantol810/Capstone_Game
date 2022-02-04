using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.ChangeSpeed();

                //controller.ChangeHealth(1);
            Destroy(gameObject);

                //controller.PlaySound(collectedClip);
            
        }
    }
}
