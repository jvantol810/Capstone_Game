using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float duration = 3f;
    Rigidbody2D rigidbody2d;
    public float fieldOfImpact;
    public float explosionForce;

    public LayerMask layerToHit;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();

        //waitTime = startWaitTime;
    }

    public void Launch()
    {
        StartCoroutine(Explode());
    }
    
    IEnumerator Explode()
    {
        //wait to continue with function
        yield return new WaitForSeconds(duration);

        //code section for bomb exploding
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, fieldOfImpact, layerToHit);

        foreach (Collider2D obj in objects)
        {
            Vector2 direction = obj.transform.position - transform.position;

            obj.GetComponent<Rigidbody2D>().AddForce(direction * explosionForce);
        }

        //make the bomb sprite disappear
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        //destroy the bomb
        Destroy(gameObject);
    }
}
