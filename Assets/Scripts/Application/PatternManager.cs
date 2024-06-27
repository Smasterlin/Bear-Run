using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PatternManager : MonoSingleTon<PatternManager>
{
    public List<Pattern> patternList = new();

}
[System.Serializable]
public class PatternItems
{
    public string prefabsName;
    public Vector3 pos;
}
[System.Serializable]
public class Pattern
{
    public List<PatternItems> patternItemList = new(); 
}
