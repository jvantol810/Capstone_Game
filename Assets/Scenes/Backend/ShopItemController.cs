using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopItemController : MonoBehaviour
{
    public GameObject hatItemButton;
    public Text hatItemButtonText;
    public Text priceText;
    [SerializeField]
    string buttonMode = "Purchase";
    [SerializeField]
    string hatID;
    [SerializeField]
    int itemPrice = 0;

    // Start is called before the first frame update
    void Start()
    {
        //hatItemButton = gameObject.GetComponentInChildren<Button>().gameObject;
        //hatItemButtonText = hatItemButton.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetHatID()
    {
        return hatID;
    }

    public void SetHatID(string newID)
    {
        //Check if legit
        hatID = newID;
    }
    public void setButtonMode(string newButtonMode)
    {
        switch (newButtonMode)
        {
            case "Buy":
                buttonMode = newButtonMode;
                hatItemButton.GetComponent<Image>().color = new Color(1f, 0.8184516f, 0.3349057f, 1f);
                hatItemButtonText.text = "Buy";
                priceText.text = itemPrice + " GP";
                break;
            case "Equip":
                buttonMode = newButtonMode;
                hatItemButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                hatItemButtonText.text = "Equip";
                priceText.text = "Bought";
                break;
            case "Unequip":
                buttonMode = newButtonMode;
                hatItemButton.GetComponent<Image>().color = new Color(1f, 0.4f, 0.3349057f, 1f);
                hatItemButtonText.text = "Unequip";
                priceText.text = "Bought";
                break;
            default:
                break;
        }
    }

    public void pressButton()
    {
        switch(buttonMode)
        {
            case "Buy"://Start Transaction Request
                CurrencyData playerCurrency = FindObjectOfType<CurrencyData>();
                Debug.Log(itemPrice);
                if (playerCurrency.SpendCurrency(itemPrice, "Hat", hatID))
                {
                    setButtonMode("Equip");
                    
                }
                else
                {
                    StartCoroutine(FailedToBuy());
                }
                break;
            case "Equip":
                ShopManager shopManager = FindObjectOfType<ShopManager>();
                shopManager.OverrideLastHatEquipped(gameObject.GetComponent<ShopItemController>());
                CashEquipsData equippedData = FindObjectOfType<CashEquipsData>();
                equippedData.SetHat(hatID);
                setButtonMode("Unequip");
                //Start giving player hat
                break;
            case "Unequip":
                setButtonMode("Equip");
                //Remove player hat
                FindObjectOfType<CashEquipsData>().RemoveHat();
                break;
            default:
                break;
        }
    }

    private IEnumerator FailedToBuy()
    {
        hatItemButton.GetComponent<Button>().interactable = false;
        hatItemButtonText.text = "Not Enough GB!";
        yield return new WaitForSeconds(1f);
        hatItemButton.GetComponent<Button>().interactable = true;
        hatItemButtonText.text = "Buy";
    }
}
