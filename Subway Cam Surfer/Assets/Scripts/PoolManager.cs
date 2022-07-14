using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //Int = key
    Dictionary<int, Queue<GameObject>> poolDictionary = new Dictionary<int, Queue<GameObject>>();

    //Singleton pattern to get acces to these methods without reference to the PoolManager
    static PoolManager _instance;

    public static PoolManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PoolManager>();
            }
            return _instance;
        }
    }

    //Create the pool defined by a gameObject and a size
    public void CreatePool(GameObject prefab, int poolSize)
    {
        //Key is unique for each prefab
        int poolKey = prefab.GetInstanceID();

        //Make sure the poolkey is already in our dictionary because it will cause errors if we don't
        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<GameObject>());
        }

        //Instantiate prefabs to create te pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newObject = Instantiate(prefab) as GameObject;
            newObject.SetActive(false);
            //Add to the pool
            poolDictionary[poolKey].Enqueue(newObject);
        }
    }

    public void ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();
        //Make sure that the pool contains the key
        if (poolDictionary.ContainsKey(poolKey))
        {
            //Get first object of the queue
            GameObject objectToReuse = poolDictionary[poolKey].Dequeue();
            //Add object back to the end of the queue so we can reuse it again later
            poolDictionary[poolKey].Enqueue(objectToReuse);

            objectToReuse.SetActive(true);
            objectToReuse.transform.position = position;
            objectToReuse.transform.rotation = rotation;
        }
    }
}
