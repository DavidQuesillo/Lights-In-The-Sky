using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValkSpawnVfxPool : pool
{
    /*public GameObject obj;
    public List<GameObject> objList;

    public int poolsize = 5;*/

    private static ValkSpawnVfxPool instance;
    public static new ValkSpawnVfxPool Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        AddObjToPool(poolsize);
    }
    /*
    public virtual void AddObjToPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject gO = Instantiate(obj, transform);
            gO.SetActive(false);
            objList.Add(gO);
        }
    }

    public virtual GameObject RequestPoolObject()
    {
        for (int i = 0; i < objList.Count; i++)
        {
            if(objList[i].activeSelf == false)
            {
                objList[i].SetActive(true);
                return objList[i]; //ends the function if an inactive object is found. it returns that single object and ends the iteration
            }
        }
        AddObjToPool(1);
        objList[objList.Count - 1].SetActive(true);
        return objList[objList.Count - 1]; //creates a new object and adds it to the pool to return it to the function
    }*/
}