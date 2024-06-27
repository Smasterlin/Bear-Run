using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIMainMenu : View
{
    [SerializeField] Button btn_playGame;
    [SerializeField] Button btn_shop;
    [SerializeField] Button btn_setting;
    [SerializeField] SkinnedMeshRenderer playerSkm;
    [SerializeField] MeshRenderer footballMr;
    GameModel gm;
    private void Awake()
    {
        btn_playGame.onClick.AddListener(OnPlayGameClick);
        btn_shop.onClick.AddListener(OnShopClick);
        btn_setting.onClick.AddListener(OnSettingClick);

        Init();
    }
    private void Init()
    {
        gm = GetModel<GameModel>();
        playerSkm.material = Game.Instance.staticData.GetPlayerCloth(gm.TakeOnCloth.skinID, gm.TakeOnCloth.clothID).material;
        footballMr.material = Game.Instance.staticData.GetFootBallInfo(gm.TakeOnFootBall).material;
    }
    private void OnPlayGameClick()
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        Game.Instance.LoadScene(3);
    }
    private void OnShopClick()
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        Game.Instance.LoadScene(2);
    }
    private void OnSettingClick()
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        SendEvent(Const.E_Setting);
    }
    public override string Name { get { return Const.V_MainMenu; } }

    public override void HandleEvent(string name, object data)
    {
        
    }
}
