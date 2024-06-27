using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIBuyTools : View
{
    GameModel gm;
    [Header("需要显示的数字更新")]
    [SerializeField] TextMeshProUGUI txt_Magnet;
    [SerializeField] TextMeshProUGUI txt_Invincible;
    [SerializeField] TextMeshProUGUI txt_Multiply;

    [SerializeField] TextMeshProUGUI txt_coins;

    [Header("按钮")]
    [SerializeField] Button btn_BuyMagnet;
    [SerializeField] Button btn_BuyInvincible;
    [SerializeField] Button btn_BuyMultiply;
    [SerializeField] Button btn_BuyRandom;
    [SerializeField] Button btn_playGame;
    [SerializeField] Button btn_return;

    [Header("模型更新")]
    [SerializeField] SkinnedMeshRenderer playerSkm;
    [SerializeField] MeshRenderer footballMr;
    public override string Name { get { return Const.V_BuyTools; } }

    public override void HandleEvent(string name, object data)
    {
        
    }
    private void Awake()
    {
        gm = GetModel<GameModel>(); 
        InitUI();

        //注册按钮事件
        btn_BuyMagnet.onClick.AddListener(delegate { OnMagnetClick(100);});
        btn_BuyInvincible.onClick.AddListener(delegate { OnInvincibleClick(200); });
        btn_BuyMultiply.onClick.AddListener(delegate { OnMultiplyClick(200); });
        btn_BuyRandom.onClick.AddListener(delegate { OnRandomClick(300); });
        btn_playGame.onClick.AddListener(OnPlayGameClick);
        btn_return.onClick.AddListener(OnReturnClick);

        playerSkm.material = Game.Instance.staticData.GetPlayerCloth(gm.TakeOnCloth.skinID, gm.TakeOnCloth.clothID).material;
        footballMr.material = Game.Instance.staticData.GetFootBallInfo(gm.TakeOnFootBall).material;
    }
    private void OnPlayGameClick()
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        Game.Instance.LoadScene(4);
    }
    private void OnReturnClick()
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        if (gm.lastIndex == 2)
        {
            gm.lastIndex = 1;
        }
        Game.Instance.LoadScene(gm.lastIndex);
    }
    /// <summary>
    /// 购买按钮
    /// </summary>
    /// <param name="i"></param>
    public void OnMagnetClick(int i=100)
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        BuyToolArgs e = new()
        {
            coins = i,
            itemKind = ITEMKIND.ITEMMAGNET
        };
        SendEvent(Const.E_BuyTools,e);
    }
    public void OnInvincibleClick(int i=200)
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        BuyToolArgs e = new()
        {
            coins = i,
            itemKind = ITEMKIND.ITEMINVINCIBLE,
        };
        SendEvent(Const.E_BuyTools,e);
    }
    public void OnMultiplyClick(int i=200)
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        BuyToolArgs e = new()
        {
            coins = i,
            itemKind = ITEMKIND.ITEMMULTIPLY,
        };
        SendEvent(Const.E_BuyTools,e);
    }
    private void OnRandomClick(int i = 300)
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        int randomIndex = Random.Range(0, 3);
        switch (randomIndex)
        {
            case 0:
                OnMagnetClick(i);
                break;
            case 1:
                OnInvincibleClick(i);
                break;
            case 2:
                OnMultiplyClick(i);
                break;
        }
    }
    /// <summary>
    /// UI初始化
    /// </summary>
    public void InitUI()
    {
        txt_coins.text = gm.Coin.ToString();
        ShowOrHide(gm.Magnet,txt_Magnet);
        ShowOrHide(gm.Invincible,txt_Invincible);
        ShowOrHide(gm.Multiply,txt_Multiply);
    }
    private void ShowOrHide(int i ,TextMeshProUGUI txt)
    {
        if (i>0)
        {
            txt.transform.parent.gameObject.SetActive(true);
            txt.text = i.ToString();
        }
        else
        {
            txt.transform.parent.gameObject.SetActive(false);
        }
    }

   
}
