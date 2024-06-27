using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class SpawnManager 
{
    [MenuItem("Tools/Click ME")]
    static void PatternSys()
    {
        GameObject spawnManager = GameObject.Find("PatternManager");
        if (spawnManager!=null)
        {
            PatternManager patternManager = spawnManager.GetComponent<PatternManager>();
            if (Selection.gameObjects.Length==1)
            {
                var item= Selection.gameObjects[0].transform.Find("Item");
                if (item!=null)
                {
                    Pattern pattern = new();
                    foreach (var child in item)
                    {
                        Transform childTrans = child as Transform;
                        if (childTrans!=null)
                        {
                            Debug.Log("�ҵ���item�µ�����");
                            var prefab = PrefabUtility.GetCorrespondingObjectFromSource(childTrans);
                            if (prefab!=null)
                            {
                                PatternItems patternItem = new()
                                {
                                    pos = childTrans.localPosition,
                                    prefabsName=prefab.name
                                };
                                pattern.patternItemList.Add(patternItem);
                                Debug.Log("���Գɹ����");
                            }
                        }
                    }
                    //ȫ������
                    patternManager.patternList.Add(pattern);
                }
            }
        }
        
    }
}
