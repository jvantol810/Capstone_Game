using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashEquipsData : MonoBehaviour
{
    [Header("Currently Equipped")]
    [SerializeField]
    GameObject currentHat;
    [SerializeField]
    GameObject currentBandana;
    [Header("Owned")]
    [SerializeField]
    string[] ownedHats;
    [SerializeField]
    string[] ownedBandanas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string GetCurrentHat()
    {
        return currentHat.GetComponent<HatItem>().GetHatID();
    }
    public void SetHat(string hatID)
    {
        for(int i = 0; i < ownedHats.Length; i++)
        {
            if(hatID == ownedHats[i])
            { //Allow player to equip item!
                GameObject.Destroy(currentHat);
                
            }
        }
    }

    public void SetBandana(string bandanaID)
    {
        for(int i = 0; i < ownedBandanas.Length; i++)
        {
            if(bandanaID == ownedBandanas[i])
            { //Allow player to equip item!
                GameObject.Destroy(currentBandana);
            }
        }
    }

    public string[] GetOwnedHats()
    {
        return ownedHats;
    }

    public string[] GetOwnedBandanas()
    {
        return ownedBandanas;
    }
}
