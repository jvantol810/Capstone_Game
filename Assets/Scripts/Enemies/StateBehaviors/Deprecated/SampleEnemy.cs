using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleEnemy : MonoBehaviour
{
    public static bool isPossessed;

    public float speed = 3.0f;

    float horizontal;
    float vertical;

    Vector2 lookDirection = new Vector2(1, 0);

    // Start is called before the first frame update
    void Start()
    {
        isPossessed = false;
    }

    // Update is called once per frame
    void Update()
    {
        //call Possess function when enemy is possessed
        if(isPossessed == true)
        {
            Possess();
        }
        else if(isPossessed == false)
        {

        }
    }

    public void Possess()
    {
        //take input from keyboard -- NOT WORKING
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        //set look direction of sprite
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
    }
}
