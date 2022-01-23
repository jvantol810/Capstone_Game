using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardController : MonoBehaviour
{
    [Header("Display Settings")]
    [SerializeField]
    int maxDisplayCount;
    [SerializeField]
    int displayCount;

    [Header("Required Objects")]
    public GameObject viewportContent;

    public GameObject[] textBoxes;
    
    // Start is called before the first frame update
    void Start()
    {
        textBoxes = new GameObject[displayCount];
        InitializeScoreList();
        //StartCoroutine(TestResizingList());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDisplayCount(int newcount)
    {
        displayCount = (newcount < maxDisplayCount) ? newcount : maxDisplayCount;
        InitializeScoreList();
    }

    private void InitializeScoreList()
    {
        Text[] viewportContentChildren = viewportContent.GetComponentsInChildren<Text>(true);
        textBoxes = new GameObject[displayCount];
        for (int i = 0; i < viewportContentChildren.Length; i++)
        {
            if(i < displayCount)
            {
                textBoxes[i] = viewportContentChildren[i].gameObject;
                textBoxes[i].SetActive(true);
            }
            else
            {
                viewportContentChildren[i].gameObject.SetActive(false);
            }
            
        }
    }

    IEnumerator TestResizingList()
    {
        Debug.Log(displayCount);
        yield return new WaitForSeconds(5);
        UpdateDisplayCount(100);
        Debug.Log(displayCount);
    }
}
