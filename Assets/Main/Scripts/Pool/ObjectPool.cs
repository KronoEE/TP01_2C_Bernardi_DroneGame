using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private T prefab;
    private Transform container;
    private List<T> pool = new List<T>();
    private int maxCapacity;

    public ObjectPool(T prefab, Transform container, int initialSize, int maxCapacity)
    {
        this.prefab = prefab;
        this.container = container;
        this.maxCapacity = maxCapacity;

        for (int i = 0; i < initialSize; i++)
            CreateObject();
    }
    private T CreateObject()
    {
        if (pool.Count >= maxCapacity) return null;

        T obj = GameObject.Instantiate(prefab, container);
        obj.gameObject.SetActive(false);
        pool.Add(obj);
        return obj;
    }
    public T Get()
    {
        foreach (T obj in pool)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                obj.gameObject.SetActive(true);
                return obj;
            }
        }
        T newObj = CreateObject();
        if (newObj != null)
        {
            newObj.gameObject.SetActive(true);
            return newObj;
        }
        return null;
    }
    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(container);
    }
    public int ActiveCount()
    {
        int count = 0;
        foreach (T obj in pool)
            if (obj.gameObject.activeInHierarchy) count++;
        return count;
    }
}
