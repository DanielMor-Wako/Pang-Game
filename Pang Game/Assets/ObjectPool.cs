using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int MinimumObjects;
    public bool LimitObjectsCount = true;
    public List<GameObject> objects;
    public UnityEvent OnSpawnEvent;

    void Awake()
    {
        if (OnSpawnEvent == null)
            OnSpawnEvent = new UnityEvent();
    }

    private void Start()
    {
        if (MinimumObjects > 0)
        {
            int TotalExistingObjects = ReturnActiveObjectsCount();
            // Instantiate objects to fill the missing objects count
            while (TotalExistingObjects < MinimumObjects)
            {
                TotalExistingObjects++;
                GameObject newInstance = CreateNewObject();
                // Store new object to the pool
                AddObjectToList(newInstance);
                //Debug.Log("filling in object: "+ TotalExistingObjects+" / "+ MinimumObjects);
                newInstance.SetActive(false);

                OnSpawnEvent?.Invoke();
            }
            //Debug.Log("stored objects "+ prefab[0].name + ", quantity: " + TotalExistingObjects);
        }
    }
    public int ReturnActiveObjectsCount()
    {
        // Reads thru all the existing children of this transform
        // And returns the total count of active objects
        int TotalExistingObjects = 0;
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
                TotalExistingObjects++;
        }
        return TotalExistingObjects;
    }
    
    public GameObject SpawnObject(Vector2 pos)
    {
        // spawn an object to the spawn point from the object pool
        GameObject nextObjectToSpawn = ReturnNextPoolObject();
        if (nextObjectToSpawn != null)
        {
            //Debug.Log("found available object");
            nextObjectToSpawn.transform.position = pos;
            nextObjectToSpawn.SetActive(true);
        }
        else
        {
            // Debug.Log("couldnt find available object "+prefab[0]);
            // creating new object if possible
            if (!LimitObjectsCount)
            {
                // Store new object to the pool
                GameObject newInstance = CreateNewObject();
                AddObjectToList(newInstance);
                nextObjectToSpawn = newInstance;
            }
        }

        return nextObjectToSpawn;
    }

    public void DespawnAllObjects()
    {
        //Debug.Log("DespawnAllObjects called on "+transform.name);
        for (int i = 0; i < objects.Count; i++)
        {
            //Debug.Log("reseting object numb " + i + " = "+ objects[i].gameObject.activeSelf);
            if (objects[i].gameObject.activeSelf)
            {
                objects[i].gameObject.SetActive(false);
            }
        }
    }

    
    private GameObject CreateNewObject()
    {
        GameObject newInstance = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        newInstance.transform.SetParent(transform);
        return newInstance;
    }
    private int ReturnNextAvailableObject()
    {
        // find anext vailable inactive Object
        // -1 = none, 0 and above relates to the index number in objects array
        int AvailableObjectID = -1;
        for (int i = 0; i < objects.Count; i++)
        {
            //Debug.Log("checking available object numb " + i + " = "+ objects[i].gameObject.activeSelf);
            if (!objects[i].gameObject.activeSelf)
            {
                AvailableObjectID = i;
                i = objects.Count;
            }
        }
        return AvailableObjectID;
    }
    private GameObject ReturnNextPoolObject()
    {
        // find available inactive Object
        // -1 = none, 0 and above relates to the index number in objects array
        int AvailableObjectID = ReturnNextAvailableObject();
        if (AvailableObjectID > -1)
        {
            //Debug.Log("Object to pull found. ID = "+ AvailableObjectID);
            return objects[AvailableObjectID].gameObject;
        }
        else
        {
            //Debug.Log("none inactive objects to pool: "+prefab[0]);
            return null;
        }
    }
    private void AddObjectToList(GameObject newObject)
    {
        // comparing to existing objects on the list, prevent duplicates
        bool existing = false;
        foreach (GameObject t in objects.ToArray())
        {
            if (t == newObject)
                existing = true;
        }

        if (existing) { return; }

        //Debug.Log("added to list: " + newObject.tag);
        objects.Add(newObject);
    }
    private void RemoveObjectFromList(GameObject oldObject)
    {
        GameObject objectToRemove = null;
        // remove old objects on the list
        foreach (GameObject t in objects)
        {
            if (t == oldObject)
            {
                //Debug.Log("removed from list: " + oldObject.name);
                objectToRemove = t;
            }
        }
        if (objectToRemove != null)
        {
            objects.Remove(oldObject);
        }
    }

}
