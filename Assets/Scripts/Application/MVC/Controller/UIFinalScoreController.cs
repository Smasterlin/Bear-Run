using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UIFinalScoreController : Controller
{
    public override void Excute(object data)
    {
        GameModel gm = GetModel<GameModel>();
        UIFinalScore finalScore = GetView<UIFinalScore>();
        UIDead dead = GetView<UIDead>();
        UIBoard board = GetView<UIBoard>();

        //更新经验值
        gm.Exp += board.Coins + board.Distance * (board.GoalCount+1);
        //更新ui
        finalScore.UpdateUI(board.Distance,board.Coins,board.GoalCount,gm.Exp,gm.Grade);


        dead.Hide();
        finalScore.Show();
        gm.Coin += board.Coins;
        board.Hide();
    }
}
