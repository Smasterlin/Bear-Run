using System;
using System.Collections.Generic;
using UnityEngine;

public class BuyClothController : Controller
{
    public override void Excute(object data)
    {
        BuyClothArg e = data as BuyClothArg;
        UIShop shop = GetView<UIShop>();
        GameModel gm = GetModel<GameModel>();

        if (gm.GetMoney(e.coin))
        {
            //购买完的物品添加进列表
            gm.m_buyPlayerClothList.Add(e.id);
           
            shop.UpdateUI();
        }
    }
}
