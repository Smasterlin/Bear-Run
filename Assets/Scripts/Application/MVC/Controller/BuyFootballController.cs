using System;
using System.Collections.Generic;
using UnityEngine;
public class BuyFootballController : Controller
{
    public override void Excute(object data)
    {
        BuyFootballArg e = data as BuyFootballArg;
        GameModel gm = GetModel<GameModel>();
        UIShop shop = GetView<UIShop>();
        Debug.Log("点击购买足球了");
        Debug.Log("购买前的金币是"+gm.Coin);
        Debug.Log("需要购买的足球的价格是" + e.coin);
        if (gm.GetMoney(e.coin))
        {
            Debug.Log("购买后的金币是" + gm.Coin);
            gm.m_BuyFootball.Add(e.index);
            foreach (var item in gm.m_BuyFootball)
            {
                Debug.Log("当前购买的足球主要有号数" + item);
            }
            shop.UpdateBuyFootballButton(e.index);
            shop.UpdateFootballGizmo();
        }
    }
}
