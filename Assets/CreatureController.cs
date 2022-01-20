using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CreatureController : MonoBehaviour
{
    [Header("References")]
    private Rigidbody2D m_rigidbody;
    [Header("Attributes")]
    public int health;
    public float speed;
    public float attack;
   
    public enum creatureState
    {
        wander,
        approach,
        attack
    }
    [Header("State")]
    private creatureState state;
    


    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("space"))
        {
            Move();
        }
    }

    private void FixedUpdate()
    {

    }


    public void Move()
    {
        Vector2 currentPos = m_rigidbody.position;
        Vector2 adjustedMovement = Vector2.left * speed;
        Vector2 newPos = currentPos + adjustedMovement * Time.fixedDeltaTime;
        m_rigidbody.MovePosition(newPos);
    }
}
