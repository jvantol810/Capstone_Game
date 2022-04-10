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
    public PHPManager phpManager;
    public HatDirectory hatDirectory;
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
        if(currentHat != null)
        {
            return currentHat.GetComponent<HatItem>().GetHatID();
        }
        else
        {
            return "0000";
        }
        
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
        HatItem hatDetails = newHat.GetComponent<HatItem>();
        phpManager.CallPostPlayerReceipt(hatDetails.GetHatID(), hatDetails.GetHatPrice());
    }

    public void InitializeOwnedHats(string receipts)
    {
        //string returnedHatIDs = phpManager.CallGetPlayerReceipts();
        string[] splitHatIDs = receipts.Split('\t');
        for(int i = 0; i < splitHatIDs.Length; i++)
        {
            
            GameObject FoundHat = hatDirectory.FindHatByID(splitHatIDs[i]);
            if (FoundHat.name != "None.")
            {
                Debug.Log("Player owns hat: " + splitHatIDs[i]);
                ownedHats.Add(hatDirectory.FindHatByID(splitHatIDs[i]));
            }
            else
            {
                Destroy(FoundHat);
            }
            
        }
    }

    public string[] GetOwnedBandanas()
    {
        return ownedBandanas;
    }
}
