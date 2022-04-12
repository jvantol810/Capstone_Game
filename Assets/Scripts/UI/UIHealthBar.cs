using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar instance { get; private set; }

    public Image mask;
    float originalSize;

    //public Image dashIcon, webIcon, bombIcon;


    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
        //dashIcon.gameObject.SetActive(false);
        //webIcon.gameObject.SetActive(false);
        //bombIcon.gameObject.SetActive(false);
    }

    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }
    /*
    public void ActivePowerIcon(Powers power)
    {
        if(power == Powers.Dash)
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
    public void DeactivePowerIcon(Powers power)
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
    */
}
