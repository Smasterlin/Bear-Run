using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SubPool
{
    List<GameObject> m_objects = new ();

    GameObject m_prefab;

    public string Name
    {
        get
        {
            return m_prefab.name;
        }
    }
    Transform m_parent;
    public SubPool(GameObject go,Transform parent)
    {
        m_prefab = go;
        m_parent = parent;
    }

    public GameObject Spawn()
    {
        GameObject go = null;
        foreach (var item in m_objects)
        {
            if (!item.activeSelf)
            {
                go = item;
            }
        }

        if (go==null)
        {
            go =Object.Instantiate(m_prefab);
            go.transform.parent = m_parent;
            m_objects.Add(go);
        }
        go.SetActive(true);
        go.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
        return go;
    }

    public void UnSpawn(GameObject go)
    {
        if (Contain(go))
        {
            go.SendMessage("OnUnSpawn",SendMessageOptions.DontRequireReceiver);
            go.SetActive(false);
        }
    }

    public void UnSpawnAll()
    {
        foreach (var item in m_objects)
        {
            if (item.activeSelf)
            {
                UnSpawn(item);
            }
        }
    }

    public bool Contain(GameObject go)
    {
        return m_objects.Contains(go);
    }
}
