using System;
using System.Collections.Generic;
using UnityEngine;

public class BuyToolController : Controller
{
    public override void Excute(object data)
    {
        BuyToolArgs e = data as BuyToolArgs;
        GameModel gm = GetModel<GameModel>();
        UIBuyTools buyTools = GetView<UIBuyTools>();
        switch (e.itemKind)
        {
            case ITEMKIND.ITEMINVINCIBLE:
                if (gm.GetMoney(e.coins))
                {
                    Debug.Log("够买之前的无敌道具数量是"+gm.Invincible);
                    gm.Invincible++;
                    Debug.Log("够买之后的无敌道具数量是" + gm.Invincible);
                }
                break;
            case ITEMKIND.ITEMMAGNET:
                if (gm.GetMoney(e.coins))
                {
                    gm.Magnet++;
                }
                break;
            case ITEMKIND.ITEMMULTIPLY:
                if (gm.GetMoney(e.coins))
                {
                    gm.Multiply++;
                }
                break;
            default:
                break;
        }
        buyTools.InitUI();
    }
}
