using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIPowers : MonoBehaviour
{
    public static UIPowers instance { get; private set; }
    public Image dashIcon, webIcon, bombIcon;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        dashIcon.gameObject.SetActive(false);
        webIcon.gameObject.SetActive(false);
        bombIcon.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivatePowerIcon(Powers power)
    {
        if (power == Powers.Dash)
        {
            dashIcon.gameObject.SetActive(true);
        }
        else if (power == Powers.ShootWeb)
        {
            webIcon.gameObject.SetActive(true);
        }
        else if (power == Powers.Explode)
        {
            bombIcon.gameObject.SetActive(true);
        }
    }
    public void DeactivatePowerIcon(Powers power)
    {
        if (power == Powers.Dash)
        {
            dashIcon.gameObject.SetActive(false);
        }
        else if (power == Powers.ShootWeb)
        {
            webIcon.gameObject.SetActive(false);
        }
        else if (power == Powers.Explode)
        {
            bombIcon.gameObject.SetActive(false);
        }
    }
}
