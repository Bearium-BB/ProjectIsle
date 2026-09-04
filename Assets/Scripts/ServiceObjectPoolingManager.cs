using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class ServiceObjectPoolingManager : MonoBehaviour
{
    [SerializeField] private List<ObjectPool> pools = new();

    public ObjectPoolGameObject AddObjectToPool(int id)
    {
        ObjectPool pool = pools.FirstOrDefault(x => x.id == id);

        ObjectPoolGameObject objectPoolGameObject = new ObjectPoolGameObject(Instantiate(pool.originalObject));

        objectPoolGameObject.DisableObject();

        pool.objects.Add(objectPoolGameObject);

        return objectPoolGameObject;
    }

    public ObjectPoolGameObject GetObjectByID(int id)
    {
        ObjectPool pool = pools.FirstOrDefault(x => x.id == id);

        if (pool == null)
            return null;

        ObjectPoolGameObject obj = pool.objects
            .FirstOrDefault(x => !x.StateOfObject());

        if (obj == null)
        {
            return AddObjectToPool(id);
        }

        return obj;
    }

    public ObjectPoolGameObject EnableGameObjectById(
        int id,
        Vector3 position,
        Quaternion rotation,
        Transform parent = null)
    {
        ObjectPoolGameObject obj = GetObjectByID(id);

        obj.GetTransform().SetPositionAndRotation(position, rotation);

        if (parent != null)
            obj.GetTransform().SetParent(parent);

        obj.EnableObject();

        return obj;
    }
}

[System.Serializable]
public class ObjectPool
{
    public int id;
    public GameObject originalObject;
    public List<ObjectPoolGameObject> objects = new();
}

[System.Serializable]
public class ObjectPoolGameObject
{
    public GameObject gameObject;

    public ObjectPoolGameObject(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public void EnableObject()
    {
        gameObject.SetActive(true);
    }

    public void DisableObject()
    {
        gameObject.SetActive(false);
    }

    public bool StateOfObject()
    {
        return gameObject.activeSelf;
    }

    public Transform GetTransform()
    {
        return gameObject.transform;
    }

    public T GetComponent<T>() where T : Component
    {
        return gameObject.GetComponent<T>();
    }
}