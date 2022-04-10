using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HatItem : MonoBehaviour
{
    
    [SerializeField]
    private string hatID;
    [SerializeField]
    private int hatPrice;
    [SerializeField]
    private Vector3 offsetD;
    [SerializeField]
    private Vector3 offsetU;
    [SerializeField]
    private Vector3 offsetL;
    [SerializeField]
    private Vector3 offsetR;
    public Sprite forwardSprite;
    public Sprite backwardSprite;
    SpriteRenderer playerSR;
    SpriteRenderer hatSR;
    // Start is called before the first frame update
    void Start()
    {
        hatSR = GetComponent<SpriteRenderer>();
        playerSR = transform.parent.gameObject.GetComponentInParent<SpriteRenderer>();
        Debug.Log(playerSR.sprite.ToString() == "GhostLeft (UnityEngine.Sprite)");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        updateSprite();
    }

    public string GetHatID()
    {
        return hatID;
    }

    public int GetHatPrice()
    {
        return hatPrice;
    }

    public void updateSprite()
    {
        hatSR.sprite = forwardSprite;
        switch(playerSR.sprite.ToString())
        {
            case "GhostLeft (UnityEngine.Sprite)":
                transform.localPosition = offsetL;
                break;
            case "Ghost (UnityEngine.Sprite)":
                transform.localPosition = offsetR;
                break;
            case "GhostDown (UnityEngine.Sprite)":
                transform.localPosition = offsetD;
                break;
            case "GhostUp (UnityEngine.Sprite)":
                transform.localPosition = offsetU;
                hatSR.sprite = backwardSprite;
                break;

        }
    }

    
}
