using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    private void Start()
    {
        GameEvents.OnEnterTeleporter.AddListener(SaySomething);
    }

    public void SaySomething()
    {
        Debug.Log("I'm giving up on you");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameEvents.OnEnterTeleporter.Invoke();
        }
    }
}
