using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    public float speed = 25f;
    public Vector2 direction;
    public LayerMask whatCanBeHit;
    private Rigidbody2D m_rigidbody;
    private bool shot;
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (shot)
        {
            Move(direction);
        }
    }

    public void Move(Vector2 direction)
    {
        //Get the current position based on the rigidbody
        Vector2 currentPos = m_rigidbody.position;
        //Multiply direction and speed to get the adjustedMovement value, which will modify the current position
        Vector2 adjustedMovement = direction * speed;
        //Calculate the new position by adding the adjustedMovement to the current position, and multiplying by fixedDeltaTime to correct for different framerates
        Vector2 newPos = currentPos + adjustedMovement * Time.fixedDeltaTime;
        //Set the rigidbody's location to the new position
        m_rigidbody.MovePosition(newPos);
    }

    public void Shoot(Vector2 direction)
    {
        this.direction = direction;
        shot = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the collision is not touching the whatCanBeHit layers, return.
        if (!collision.IsTouchingLayers(whatCanBeHit)) { return; }

        //Otherwise, check the tag of the gameobjects that it collided with.
        switch (collision.gameObject.tag)
        {
            case "Player": Debug.Log("Player hit!"); break;
            default: Debug.Log("Something hit!"); break;
        }

        //After responding to the collision, destroy the projectile.
        Destroy(gameObject);
    }
}
