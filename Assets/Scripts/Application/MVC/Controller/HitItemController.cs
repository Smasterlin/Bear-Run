using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HitItemController : Controller
{
    public override void Excute(object data)
    {
        ItemArg e = data as ItemArg;
        GameModel gm = GetModel<GameModel>();
        PlayerMove player = GetView<PlayerMove>();
        UIBoard board = GetView<UIBoard>();

        switch (e.kind)
        {
            case ITEMKIND.ITEMINVINCIBLE:
                player.HitInvincible();
                board.HitInvincible();
                gm.Invincible -= e.hitCount;
                break;
            case ITEMKIND.ITEMMAGNET:
                player.HitMagnet();
                board.HitMagnet();
                gm.Magnet -= e.hitCount;
                break;
            case ITEMKIND.ITEMMULTIPLY:
                player.HitMultiply();
                board.HitMultiply();
                gm.Multiply -= e.hitCount;
                break;
             
        }
        board.UpdateUI();
    }
}
