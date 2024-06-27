using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EquipClothController : Controller
{
    public override void Excute(object data)
    {
        BuyClothArg e = data as BuyClothArg;
        UIShop shop = GetView<UIShop>();
        GameModel gm = GetModel<GameModel>();

        gm.TakeOnCloth=e.id;
        shop.UpdateUI();
    }
}
