using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObject prefab;
    public GameObject whitePrefab;
    public int size;
    public int whiteSize;

    public static ObjectPooler Instance;
    public Queue<GameObject> pool;
    public Queue<GameObject> whitePool;

    private void Awake()
    {
        Instance = this;
        pool = new Queue<GameObject>();
        whitePool = new Queue<GameObject>();

        for (int i = 0; i < size; i++)
        {
            GameObject go = Instantiate(prefab);
            go.SetActive(false);
            go.transform.SetParent(transform);
            pool.Enqueue(go);
        }

        for (int i = 0; i < whiteSize; i++)
        {
            GameObject go = Instantiate(whitePrefab);
            go.SetActive(false);
            go.transform.SetParent(transform);
            whitePool.Enqueue(go);
        }
    }

    public GameObject SpawnFromPool(Vector3 position, Quaternion rotation)
    {
        GameObject go = pool.Dequeue();
        go.SetActive(true);
        go.transform.position = position;
        go.transform.rotation = rotation;
        pool.Enqueue(go);
        return go;
    }
    public GameObject SpawnWhite(Vector3 position, Quaternion rotation)
    {
        GameObject go = whitePool.Dequeue();
        go.SetActive(true);
        go.transform.position = position;
        go.transform.rotation = rotation;
        whitePool.Enqueue(go);
        return go;
    }
}
