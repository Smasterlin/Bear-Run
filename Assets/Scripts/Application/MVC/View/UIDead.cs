using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIDead : View
{
    [SerializeField] Button btn_cancle;
    [SerializeField] Button btn_bribery;
    [SerializeField] TextMeshProUGUI txt_briberyCoins;
    int briberyTime=1;
    public override string Name { get { return Const.V_UIDead; } }

    public int BriberyTime { get => briberyTime; set => briberyTime = value; }

    public override void HandleEvent(string name, object data)
    {
        
    }
    private void Awake()
    {
        btn_cancle.onClick.AddListener(OnCancleClick);
        btn_bribery.onClick.AddListener(OnBriberyClick);
        briberyTime = 1;
    }
    /// <summary>
    /// 点击贿赂按钮
    /// </summary>
    public void OnBriberyClick()
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        CoinArg e = new()
        {
            coins = briberyTime * 500
        };
        SendEvent(Const.E_BriberyClick,e);
    }
    /// <summary>
    /// 显隐
    /// </summary>
    public void Show()
    {
        txt_briberyCoins.text = (500 * briberyTime).ToString();
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 取消按钮
    /// </summary>
    public void OnCancleClick()
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        SendEvent(Const.E_UIFinalScore);
    }
}
