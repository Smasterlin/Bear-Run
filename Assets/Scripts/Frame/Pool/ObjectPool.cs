using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoSingleTon<ObjectPool>
{
    public string resourceDir = "";

    Dictionary<string , SubPool> m_pools = new Dictionary<string,SubPool>();

    public GameObject Spawn(string name,Transform trans)
    {
        SubPool pool = null;
        if (!m_pools.ContainsKey(name))
        {
            RegisterNew(name,trans);
        }
        pool = m_pools[name];
        return pool.Spawn();
    }
    public void UnSpawn(GameObject go)
    {
        SubPool pool = null;
        foreach (var p in m_pools.Values)
        {
            if (p.Contain(go))
            {
                pool = p;
                break;
            }
        }
        pool.UnSpawn(go);
    }
    public void UnSpawnAll()
    {
        foreach (var p in m_pools.Values)
        {
            p.UnSpawnAll();
        }
    }

    public void Clear()
    {
        m_pools.Clear();
    }
    public void RegisterNew(string name,Transform trans)
    {
        string path = resourceDir + "/" + name;
        GameObject go = Resources.Load<GameObject>(path);

        SubPool pool = new SubPool(go,trans);
        m_pools.Add(pool.Name,pool);
    }
}
