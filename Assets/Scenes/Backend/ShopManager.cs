using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject viewportContent;
    public PHPManager phpManager;
    public Button[] TabButtons;
    public CashEquipsData CEData;
    [Header("Prefabs")]
    public GameObject ShopItemPrefab;


    // Start is called before the first frame update
    void Start()
    {
        DisplayItems("Hat");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayItems(string item)
    {
        StopAllCoroutines(); // Hopefully doesn't cause issues, but keep watch!
        ClearViewportContent();
        ResetTabButtons();
        switch (item)
        {
            case "Hat":
                TabButtons[0].interactable = false;
                StartCoroutine(RefreshShopItems("Hat"));
                break;
            case "Bandana":
                TabButtons[1].interactable = false;
                StartCoroutine(RefreshShopItems("Bandana"));
                break;
            default:

                break;
        }
    }

    private void ClearViewportContent()
    {
        Transform[] currentVC;
        currentVC = viewportContent.GetComponentsInChildren<Transform>();
        //Debug.Log("Currently have: " + currentVC.Length + " entries.");
        for (int i = 1; i < currentVC.Length; i++)
        {
            Destroy(currentVC[i].gameObject);
        }
    }

    private void ResetTabButtons()
    {
        for(int i = 0; i < TabButtons.Length; i++)
        {
            TabButtons[i].interactable = true;
        }

    }

    private IEnumerator RefreshShopItems(string item)
    {
        //GameObject LoadingEntry = Instantiate(ShopItemPrefab, viewportContent.transform);
        //CoroutineWithData cwd = new CoroutineWithData(this, phpManager.GetGlobalLeaderboard());
        //yield return cwd.coroutine;
        //Destroy(LoadingEntry);
        //string shopitems = cwd.result.ToString();
        string shopitems = "True\tHat #1\t150\t001\tHat #2\t250\t002\tHat #3\t50\t003\tHat #4\t100\t004";
        string playeritemspurchased = "";
        string[] OwnedHats = CEData.GetOwnedHats();
        for (int i = 0; i < OwnedHats.Length; i++)
        {
            playeritemspurchased += OwnedHats[i] + "\t";
        }
        //Debug.Log(leaderboardscores);
        yield return new WaitForSeconds(1f);
        string[] sortedEntries = shopitems.Split('\t');
        if (sortedEntries[0] == "False")
        {
            //Instantiate(failedConnectionPrefab, viewportContent.transform);
        }
        else
        {
            int addedEntries = 0;
            for (int i = 1; i < sortedEntries.Length; i += 3)
            {
                GameObject newEntry = Instantiate(ShopItemPrefab, viewportContent.transform);
                Text[] newEntryTexts = newEntry.GetComponentsInChildren<Text>();
                newEntryTexts[0].text = sortedEntries[i];
                newEntryTexts[2].text = sortedEntries[i+1] + " GB";
                if(playeritemspurchased.Contains(sortedEntries[i+2]))
                {
                    Debug.Log(CEData.GetCurrentHat() + "  " + sortedEntries[i + 2]);
                    if(CEData.GetCurrentHat() == sortedEntries[i+2])
                    {
                        newEntryTexts[1].text = "Unequip";
                    }
                    else
                    {
                        newEntryTexts[1].text = "Equip";
                    }
                    newEntryTexts[1].transform.parent.GetComponent<Image>().color = new Color(1f, 1f, 1f);
                }

                addedEntries++;
            }
        }
    }
}
