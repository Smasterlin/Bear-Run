using System;
using System.Collections.Generic;
using UnityEngine;

public class EndGameController : Controller
{
    
    public override void Excute(object data)
    {

        GameModel gm = GetModel<GameModel>();
        Debug.Log("获得了数据模型");
        
        gm.IsPlay = false;

        //TODO:游戏结束的UI显示

        UIDead dead = GetView<UIDead>();
        dead.Show();
    }
}
