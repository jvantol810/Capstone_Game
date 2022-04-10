using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyData : MonoBehaviour
{
    public TitleScreenManager TSM;
    public HatDirectory hatDir;
    //[SerializeField]
    //private int currentGhostBucks = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartPurchase(GameObject PButton)
    {
        char ID = PButton.name[PButton.name.Length - 1];
        StartCoroutine(PressPurchaseButton(PButton, ID));
    }
    private IEnumerator PressPurchaseButton(GameObject PButton, char itemID)
    {
        PButton.GetComponentInChildren<Text>().text = "...";
        PButton.GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(1f);
        PurchaseCurrency(itemID);
        PButton.GetComponentInChildren<Text>().text = "Buy";
        PButton.GetComponent<Button>().interactable = true;
    }

    public void PressPurchaseScreenButton(GameObject ButtonPressed)
    {
        StartCoroutine(PressGBPButton(ButtonPressed));
    }
    private IEnumerator PressGBPButton(GameObject GBPButton)
    {
        GBPButton.GetComponentInChildren<Text>().text = "Please Login/Register";
        GBPButton.GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(1f);
        GBPButton.GetComponentInChildren<Text>().text = "Buy Ghost Bucks";
        GBPButton.GetComponent<Button>().interactable = true;
    }

    private void PurchaseCurrency(char itemID)
    {
        switch (itemID)
        {
            case '1':
                DatabaseManager.currency += 300;
                break;
            case '2':
                DatabaseManager.currency += 1000;
                break;
            case '3':
                DatabaseManager.currency += 3300;
                break;
            default:
                break;
        }
        FindObjectOfType<PHPManager>().CallUpdatePlayerCurrency();
        TSM.UpdateGhostBucksDisplay(DatabaseManager.currency);
    }

    public bool SpendCurrency(int amountspent, string itemPurchased, string itemID)
    {
        //Debug.Log(currentGhostBucks);
        if(Mathf.Abs(amountspent) > DatabaseManager.currency)
        {
            
            return false;
        }
        else
        {
            DatabaseManager.currency -= Mathf.Abs(amountspent);
            FindObjectOfType<PHPManager>().CallUpdatePlayerCurrency();
            TSM.UpdateGhostBucksDisplay(DatabaseManager.currency);
            //Save data to server!!!
            //Update owneditems
            if(itemPurchased == "Hat")
            {
                GameObject purchasedHat = hatDir.FindHatByID(itemID);
                if(purchasedHat.name == "None.")
                {
                    Debug.Log("Failed to add hat to owned hats!");
                    Destroy(purchasedHat);
                    return false;
                }
                else
                {
                    GetComponent<CashEquipsData>().AddOwnedHat(purchasedHat);
                }
                
            }


            return true;
        }
    }

    
}
