using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
public class PauseGameController : Controller
{
    
    public override void Excute(object data)
    {
        //显示暂停界面
        UIPause pause = GetView<UIPause>();
        pause.Show();

        PauseArg e = data as PauseArg;
        pause.txt_distance.text = e.distance.ToString();
        pause.txt_coin.text = e.coin.ToString();
        pause.txt_score.text = e.score.ToString();

        //暂停游戏
        GameModel gm = GetModel<GameModel>();
        gm.IsPause = true;


    }
}
