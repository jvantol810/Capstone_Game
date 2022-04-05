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
    //GameObject[] ownedHats;
    List<GameObject> ownedHats;
    [SerializeField]
    string[] ownedBandanas;
    public GameObject Player;
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
    //BIG ISSUE: MUST ADD PURCHASED HATS
    public void SetHat(string hatID)
    {
        Debug.Log("Setting Hat to: " + hatID);
        for(int i = 0; i < ownedHats.Count; i++)
        {
            if(hatID == ownedHats[i].GetComponent<HatItem>().GetHatID())
            { //Allow player to equip item!
                if(currentHat != null)
                {
                    GameObject.Destroy(currentHat);
                }
                
                currentHat = Instantiate(ownedHats[i], Player.transform);
            }
        }
    }

    public void RemoveHat()
    {
        if (currentHat != null)
        {
            GameObject.Destroy(currentHat);
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

    public List<GameObject> GetOwnedHats()
    {
        return ownedHats;
    }

    public void AddOwnedHat(GameObject newHat)
    {
        ownedHats.Add(newHat);
    }

    public string[] GetOwnedBandanas()
    {
        return ownedBandanas;
    }
}
