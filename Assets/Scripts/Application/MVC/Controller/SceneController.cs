using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : Controller
{
    public override void Excute(object data)
    {
        ArgScenes e = data as ArgScenes;
        switch (e.sceneIndex)
        {
            case 1:
                RegisterView(GameObject.Find("UIMainMenu").GetComponent<UIMainMenu>());
                RegisterView(GameObject.Find("UIMainMenu").transform.Find("UISetting").GetComponent<UISetting>());
                Game.Instance.soundManager.PlayBg("Bgm_JieMian");
                break;
            case 2:
                RegisterView(GameObject.Find("UIShop").GetComponent<UIShop>());
                Game.Instance.soundManager.PlayBg("Bgm_JieMian");
                break;
            case 3:
                RegisterView(GameObject.Find("Canvas").transform.Find("BuyTools").GetComponent<UIBuyTools>());
                Game.Instance.soundManager.PlayBg("Bgm_JieMian");
                break;
            case 4:
                RegisterView(GameObject.FindWithTag(Tag.player).GetComponent<PlayerMove>());
                RegisterView(GameObject.FindWithTag(Tag.player).GetComponent<PlayerAnim>());
                RegisterView(GameObject.Find("Canvas").transform.Find("UIBoard").GetComponent<UIBoard>());
                RegisterView(GameObject.Find("Canvas").transform.Find("UIPause").GetComponent<UIPause>());
                RegisterView(GameObject.Find("Canvas").transform.Find("UIResume").GetComponent<UIResume>());
                RegisterView(GameObject.Find("Canvas").transform.Find("UIDead").GetComponent<UIDead>());
                RegisterView(GameObject.Find("Canvas").transform.Find("UIFinalScore").GetComponent<UIFinalScore>());
                Game.Instance.soundManager.PlayBg("Bgm_ZhanDou");

                //让游戏重新开始
                GameModel gm = GetModel<GameModel>();
                gm.IsPause = false;
                gm.IsPlay = true;
                break;
            default:
                break;
        }
    }
}
