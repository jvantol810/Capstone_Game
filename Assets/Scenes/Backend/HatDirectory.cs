using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatDirectory : MonoBehaviour
{
    [SerializeField]
    GameObject[] AvailableHats;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FindHatByID(string hatID)
    {
        for(int i = 0; i < AvailableHats.Length; i++)
        {

        }
    }
}
