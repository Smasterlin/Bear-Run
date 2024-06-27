using System;
using System.Collections.Generic;
using UnityEngine;

public class ExitScenesController : Controller
{
    public override void Excute(object data)
    {
        ArgScenes e = data as ArgScenes;
        switch (e.sceneIndex)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                
                break;
            case 4:
                Debug.Log("重新加载场景");
                //点击重玩的时候，重新加载当前场景，把对象池清空
                Game.Instance.objectPool.Clear();
                break;
        }
        GameModel gm = GetModel<GameModel>();
        gm.lastIndex = e.sceneIndex;
    }
}
