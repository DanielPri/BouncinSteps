using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    Queue<GameObject> objectPool;
    GameObject sample;

    public Pool(GameObject prototype)
    {
        sample = prototype;
        objectPool = new Queue<GameObject>();
    }

    public Pool(GameObject prototype, int size) : this(prototype)
    {
        for(int i = 0; i< size; i++)
        {
            objectPool.Enqueue(GameObject.Instantiate(sample, Vector3.zero, Quaternion.identity, null));
            objectPool.Peek().SetActive(false);
        }
    }

    public GameObject getObjectFromPool()
    {
        if(objectPool.Count > 0)
        {
            return objectPool.Dequeue();
        }
        else
        {
            Debug.Log("Not enough in pool, instantiating");
            return GameObject.Instantiate(sample, Vector3.zero, Quaternion.identity, null);
        }

        
    }

    public void addObjectToPool(GameObject objectToAdd)
    {
        objectToAdd.SetActive(false);
        objectPool.Enqueue(objectToAdd);
    }

}
