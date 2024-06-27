using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadChange : MonoBehaviour
{
    public GameObject roadNow;
    public GameObject roadNext;
    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        if (parent == null)
        {
            parent = new();
            parent.transform.position = Vector3.zero;
            parent.name = "Road";
        }
        roadNow = Game.Instance.objectPool.Spawn("Pattern_1", parent.transform);
        roadNext = Game.Instance.objectPool.Spawn("Pattern_2", parent.transform);
        roadNext.transform.position = new Vector3(0, 0, 160);
        AddItem(roadNow);
        AddItem(roadNext);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tag.Road))
        {
            Game.Instance.objectPool.UnSpawn(other.gameObject);
            SpawnNewRoad();
        }
    }

    private void SpawnNewRoad()
    {
        int i = Random.Range(1, 5);
        roadNow = roadNext;
        roadNext = Game.Instance.objectPool.Spawn("Pattern_" + i.ToString(), parent.transform);
        roadNext.transform.position = roadNow.transform.position + new Vector3(0, 0, 160);
        AddItem(roadNext);
    }
    /// <summary>
    /// 在生成的道路上，随机生成障碍物
    /// </summary>
    private void AddItem(GameObject obj)
    {
        var itemChild = obj.transform.Find("Item");
        if (itemChild != null)
        {
            var patternManager = PatternManager.Instance;
            if (patternManager != null && patternManager.patternList != null && patternManager.patternList.Count > 0)
            {
                //随机取出一套方案
                var pattern = patternManager.patternList[Random.Range(0, patternManager.patternList.Count)];
                if (pattern != null &&pattern.patternItemList!=null&& pattern.patternItemList.Count > 0)
                {
                    foreach (var itemList in pattern.patternItemList)
                    {
                        GameObject go = Game.Instance.objectPool.Spawn(itemList.prefabsName, itemChild);
                        go.transform.parent = itemChild;
                        go.transform.localPosition = itemList.pos;
                    }
                }
            }
        }
    }
}
