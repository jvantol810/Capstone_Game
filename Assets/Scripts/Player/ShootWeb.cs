using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootWeb : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public float distanceTraveled = 0f;
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force)
    {
        
        rigidbody2d.AddForce(direction * force);
    }

    void Update()
    {
        Vector2 previousPos = transform.position;
        if (transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
        distanceTraveled += Vector2.Distance(previousPos, transform.position);
        if (distanceTraveled >= 10f)
        {
            Debug.Log("MAX DISTANCE REACHED!");
            rigidbody2d.isKinematic = true;
        }
    }

}
