using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatItem : MonoBehaviour
{
    [SerializeField]
    private string hatID;
    [SerializeField]
    private Vector3 offset;
    public Sprite forwardSprite;
    public Sprite backwardSprite;

    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = offset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetHatID()
    {
        return hatID;
    }
}
