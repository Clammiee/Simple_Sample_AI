﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPooler Instance;

    void Awake()
    {
        Instance = this;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> pool_Dictionary;

    void Start()
    {
        pool_Dictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> object_Pool = new Queue<GameObject>();

            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                object_Pool.Enqueue(obj);
            }
            pool_Dictionary.Add(pool.tag, object_Pool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position)
    {
        if(!pool_Dictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectTo_Spawn = pool_Dictionary[tag].Dequeue();
        objectTo_Spawn.SetActive(true);
        objectTo_Spawn.transform.position = position;

        pool_Dictionary[tag].Enqueue(objectTo_Spawn);

        return objectTo_Spawn;
    }
}
