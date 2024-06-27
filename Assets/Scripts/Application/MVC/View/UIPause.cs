using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIPause : View
{
    [SerializeField] Button btn_resume;
    [SerializeField] Button btn_menu;

    public TextMeshProUGUI txt_distance;
    public TextMeshProUGUI txt_coin;
    public TextMeshProUGUI txt_score;

    [SerializeField] SkinnedMeshRenderer playerSkm;
    [SerializeField] MeshRenderer footballMr;
    GameModel gm;
    private void Awake()
    {
        btn_resume.onClick.AddListener(OnClickResume);
        btn_menu.onClick.AddListener(OnMenuClick);
        gm = GetModel<GameModel>();
        
    }
    private void Init()
    {
        playerSkm.material = Game.Instance.staticData.GetPlayerCloth(gm.TakeOnCloth.skinID, gm.TakeOnCloth.clothID).material;
        footballMr.material = Game.Instance.staticData.GetFootBallInfo(gm.TakeOnFootBall).material;
    }
    private void OnMenuClick()
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        Game.Instance.LoadScene(1);
    }
    private void OnClickResume()
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        SendEvent(Const.E_Resume);
    }

    public override string Name { get { return Const.V_Pause; } }

    public override void HandleEvent(string name, object data)
    {

    }
    public void Show()
    {
        gameObject.SetActive(true);
        Init();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
