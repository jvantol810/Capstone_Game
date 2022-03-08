using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    public GameObject Leaderboard;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToLeaderboard()
    {
        Leaderboard.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SwitchToTitle()
    {
        Leaderboard.SetActive(false);
        gameObject.SetActive(true);
    }
}
