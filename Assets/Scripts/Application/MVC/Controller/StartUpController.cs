using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpController : Controller
{
    public override void Excute(object data)
    {
        //注册所有的Controller
        RegisterController(Const.E_enterScene,typeof(SceneController));
        RegisterController(Const.E_exitScene, typeof(ExitScenesController));

        RegisterController(Const.E_endGame,typeof(EndGameController));
        RegisterController(Const.E_PauseGame, typeof(PauseGameController));
        RegisterController(Const.E_Resume,typeof(ResumeController));
        RegisterController(Const.E_HitItem,typeof(HitItemController));
        RegisterController(Const.E_UIFinalScore,typeof(UIFinalScoreController));
        RegisterController(Const.E_BriberyClick,typeof(BriberyClickController));
        RegisterController(Const.E_ContinueGame,typeof(ContinueGameController));
        RegisterController(Const.E_BuyTools, typeof(BuyToolController));
        RegisterController(Const.E_BuyFootball,typeof(BuyFootballController));
        RegisterController(Const.E_EquipFootball,typeof(EquipFootballController));
        RegisterController(Const.E_BuyCloth,typeof(BuyClothController));

        RegisterController(Const.E_EquipCloth,typeof(EquipClothController));
        RegisterController(Const.E_Setting,typeof(UISettingController));
        RegisterController(Const.E_ResetData, typeof(ResetDataController));
        //注册Model
        RegisterModel(new GameModel());

        //获得数据模型
        GameModel gm = GetModel<GameModel>();
        gm.Init();
    }
}
