using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject pooledObject;
    public int pooledAmount = 20;
    public bool willGrow = false;

    public List<GameObject> pooledObjects;
    public int currentActiveObjectIndex = 0;

    void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = Instantiate(pooledObject);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject(Vector2 spawnPosition)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i] == null)
            {
                GameObject obj = Instantiate(pooledObject);
                obj.SetActive(false);
                pooledObjects[i] = obj;
                pooledObjects[i].transform.position = spawnPosition;
                return pooledObjects[i];
            }
            if (!pooledObjects[i].activeInHierarchy)
            {
                pooledObjects[i].SetActive(true);
                pooledObjects[i].transform.position = spawnPosition;
                return pooledObjects[i];
            }
        }

        //If the for loop is exited, it means no inactive objects were found. In this case, return the first gameobject instead.
        GameObject obj_ = pooledObjects[currentActiveObjectIndex];
        obj_.SetActive(true);
        if(currentActiveObjectIndex + 1 >= pooledObjects.Count)
        {
            currentActiveObjectIndex = 0;
        }
        else
        {
            currentActiveObjectIndex++;
        }
        obj_.transform.position = spawnPosition;
        return obj_;
        //if (willGrow)
        //{
        //    GameObject obj = Instantiate(pooledObject);
        //    pooledObjects.Add(obj);
        //    obj.transform.position = spawnPosition;
        //    return obj;
        //}

        //return null;
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i] == null)
            {
                GameObject obj = Instantiate(pooledObject);
                obj.SetActive(false);
                pooledObjects[i] = obj;
                return pooledObjects[i];
            }
            if (!pooledObjects[i].activeInHierarchy)
            {
                pooledObjects[i].SetActive(true);
                return pooledObjects[i];
            }
        }

        if (willGrow)
        {
            GameObject obj = Instantiate(pooledObject);
            pooledObjects.Add(obj);
            return obj;
        }

        return null;
    }
}
