using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIFinalScore : View
{
    [SerializeField] TextMeshProUGUI txt_distance;
    [SerializeField] TextMeshProUGUI txt_coin;
    [SerializeField] TextMeshProUGUI txt_goal;
    [SerializeField] TextMeshProUGUI txt_score;

    [SerializeField] Slider sliExp;
    [SerializeField] TextMeshProUGUI txt_sliExp;
    [SerializeField] TextMeshProUGUI txt_grade;

    [SerializeField] Button btn_replay;
    [SerializeField] Button btn_shop;
    [SerializeField] Button btn_menu;
    public override string Name { get { return Const.V_UIFinalScore; } }

    public override void HandleEvent(string name, object data)
    {
        
    }
    private void Awake()
    {
        btn_replay.onClick.AddListener(OnReplayClick);

        btn_shop.onClick.AddListener(OnShopClick);
        btn_menu.onClick.AddListener(OnMenuClick);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void UpdateUI(int distance,int coin,int goal,int exp,int grade)
    {
        txt_distance.text = distance.ToString();
        txt_coin.text = coin.ToString();
        txt_goal.text = goal.ToString();
        txt_score.text = (distance * (goal+1) + coin).ToString();

        txt_sliExp.text = exp.ToString() + "/" + (500 + grade * 100).ToString();
        sliExp.value = (float)exp / (500 + grade * 100);
        txt_grade.text = grade.ToString()+"¼¶";
    }
    /// <summary>
    /// ÖØÍæ°´Å¥
    /// </summary>
    private void OnReplayClick()
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        Game.Instance.LoadScene(4);
    }
    private void OnShopClick()
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        Game.Instance.LoadScene(2);
    }
    private void OnMenuClick()
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        Game.Instance.LoadScene(1);
    }
}
