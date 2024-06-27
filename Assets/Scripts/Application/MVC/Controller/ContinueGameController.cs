using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ContinueGameController : Controller
{
    public override void Excute(object data)
    {
        UIBoard board = GetView<UIBoard>();
        GameModel gm = GetModel<GameModel>();
        
        if (board.Times<=0.1f)
        {
            board.Times += 20;
        }
        gm.IsPause = false;
        gm.IsPlay = true;
    }
}
