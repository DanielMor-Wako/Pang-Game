using UnityEngine;

public class ObjectPoolList : MonoBehaviour
{
    #region SINGLETON PATTERN
    public static ObjectPoolList _instance;
    public static ObjectPoolList Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ObjectPoolList>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("ObjectPoolList");
                    _instance = container.AddComponent<ObjectPoolList>();
                }
            }

            return _instance;
        }
    }
    #endregion
    

    private void Awake()
    {
        #region SINGLETON PATTERN
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion

        // TODO: auto cicle through all the transform children and register all available pools to objectsList
    }

    public ObjectPool[] objectsList;

    public void SpawnObject(string prefab, Vector2 pos)
    {
        if (objectsList.Length <= 0)
            return;

        ObjectPool relevantPool = GetRelevantPool(prefab);
        if (relevantPool != null)
            relevantPool.SpawnObject(pos);
    }

    public ObjectPool GetRelevantPool(string objpool)
    {
        ObjectPool result = null;
        foreach (ObjectPool o in objectsList)
        {
            if (o.prefab.ToString() == objpool)
                result = o;
        }
        return result;
    }

    public void DespawnAll()
    {
        foreach (ObjectPool o in objectsList)
        {
            o.DespawnAllObjects();
        }
    }
}
