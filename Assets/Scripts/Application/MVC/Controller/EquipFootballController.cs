using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EquipFootballController : Controller
{
    public override void Excute(object data)
    {
        BuyFootballArg e = data as BuyFootballArg;
        GameModel gm = GetModel<GameModel>();
        UIShop shop = GetView<UIShop>();

        gm.TakeOnFootBall = e.index;
        //让装备按钮消失
        shop.UpdateBuyFootballButton(e.index);
        shop.UpdateFootballGizmo();
    }
}
