using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BriberyClickController : Controller
{
    public override void Excute(object data)
    {
        GameModel gm = GetModel<GameModel>();
        CoinArg e = data as CoinArg;
        UIDead dead = GetView<UIDead>();
             
        //点击贿赂按钮，根据贿赂次数算钱
        if (gm.GetMoney(e.coins))
        {
            dead.BriberyTime++;
            dead.Hide();
            UIResume resume = GetView<UIResume>();
            resume.StartCount();
        }
    }
}
