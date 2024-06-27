using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIBoard : View
{
    #region 常量
    private int startTime = 50;
    #endregion

    #region 字段
    private int m_distance;
    private int m_coins;
    private float m_time;
    private int m_goalCount;
    [Header("金币，距离，和时间滑动条参数")]
    [SerializeField] TextMeshProUGUI txtDistance;
    [SerializeField] TextMeshProUGUI txtCoins;
    [SerializeField] TextMeshProUGUI txtSlideTime;
    [SerializeField] Slider sliderTime;

    GameModel gm;

    [Header("按钮")]
    [SerializeField] Button btn_pause;
    [SerializeField] Button btn_magnet;
    [SerializeField] Button btn_invincible;
    [SerializeField] Button btn_multiply;
    [SerializeField] Button btn_goal;//射门
    [SerializeField] Slider sliGoal;

    [Header("技能显示时间")]
    [SerializeField] TextMeshProUGUI txtMagnet;
    [SerializeField] TextMeshProUGUI txtInvincible;
    [SerializeField] TextMeshProUGUI txtMultiply;

    //协程
    IEnumerator hitMultiplyCor;
    IEnumerator magnetCor;
    IEnumerator invincibleCor;
    float m_skillTime;
    #endregion

    #region 属性
    public override string Name { get { return Const.V_Board; } }

    public int Distance { get => m_distance; set { m_distance = value; txtDistance.text = value.ToString() + "米"; } }

    public int Coins { get => m_coins; set { m_coins = value; txtCoins.text = value.ToString(); } }

    public float Times
    {
        get => m_time; set
        {
            if (value < 0)
            {
                value = 0;
                SendEvent(Const.E_endGame);
            }
            if (value > startTime)
            {
                value = startTime;
            }
            m_time = value;
            txtSlideTime.text = value.ToString("f2");
            sliderTime.value = value / startTime;
        }
    }

    public int GoalCount { get => m_goalCount; set => m_goalCount = value; }
    #endregion
    #region Unity回调
    private void Awake()
    {
        gm = GetModel<GameModel>();
        //按钮注册事件
        btn_pause.onClick.AddListener(OnClickPause);
        btn_invincible.onClick.AddListener(OnInvincibleClick);
        btn_multiply.onClick.AddListener(OnMultiplyClick);
        btn_magnet.onClick.AddListener(OnMagnetClick);
        btn_goal.onClick.AddListener(OnGoalClick);

        //初始化
        UpdateUI();
        Times = startTime;
        m_skillTime = gm.SkillTime;
    }
    private void Update()
    {
        if (!gm.IsPause && gm.IsPlay)
        {
            Times -= Time.deltaTime;

        }
    }
    #endregion

    #region 方法
    /// <summary>
    /// 显隐
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// ----------------------点击技能按钮------------------------
    /// </summary>

    string GetTime(float timer)
    {
        return ((int)timer + 1).ToString();
    }
    //吃双倍金币
    public void HitMultiply()
    {
        if (hitMultiplyCor != null)
        {
            StopCoroutine(hitMultiplyCor);
        }
        hitMultiplyCor = HitMultiplyCoroutine();
        StartCoroutine(hitMultiplyCor);
    }
    IEnumerator HitMultiplyCoroutine()
    {
        float timer = m_skillTime;
        txtMultiply.transform.parent.gameObject.SetActive(true);
        while (timer > 0)
        {

            if (gm.IsPlay && !gm.IsPause)
            {
                timer -= Time.deltaTime;
                txtMultiply.text = GetTime(timer);
            }
            yield return 0;
        }

        txtMultiply.transform.parent.gameObject.SetActive(false);
    }
    //吸铁石
    public void HitMagnet()
    {
        if (magnetCor != null)//为了避免技能时间没结束的时候，碰到下一个吸铁石，碰到的话，就暂停上一个的
        {
            StopCoroutine(magnetCor);
            Debug.Log("停止上一个");
        }
        magnetCor = MagnetCoroutine();
        StartCoroutine(magnetCor);
    }
    IEnumerator MagnetCoroutine()
    {
        float timer = m_skillTime;
        Debug.Log("技能时间是" + timer);
        txtMagnet.transform.parent.gameObject.SetActive(true);
        while (timer > 0)
        {

            if (gm.IsPlay && !gm.IsPause)
            {
                timer -= Time.deltaTime;
                txtMagnet.text = GetTime(timer);
                
            }
            yield return 0;
        }
        txtMagnet.transform.parent.gameObject.SetActive(false);
    }
    //无敌
    public void HitInvincible()
    {
        if (invincibleCor != null)
        {
            StopCoroutine(invincibleCor);
        }
        invincibleCor = InvincibleCoroutine();
        StartCoroutine(invincibleCor);
    }
    IEnumerator InvincibleCoroutine()
    {
        float timer = m_skillTime;
        txtInvincible.transform.parent.gameObject.SetActive(true);

        while (timer > 0)
        {
            if (gm.IsPlay && !gm.IsPause)
            {
                timer -= Time.deltaTime;
                txtInvincible.text = GetTime(timer);
            }
            yield return 0;
        }
        txtInvincible.transform.gameObject.SetActive(false);
    }
    /// <summary>
    /// ------------------------------按钮的方法------------------------------------------
    /// </summary>
    //暂停
    public void OnClickPause()
    {
        PauseArg e = new()
        {
            distance = Distance,
            coin = Coins,
            score = Coins * 3 + Distance + GoalCount * 30,
        };
        SendEvent(Const.E_PauseGame, e);
    }
    /// <summary>
    /// 初始化左下角技能按钮的显示
    /// </summary>
    public void UpdateUI()
    {
        ShowOrHide(gm.Magnet, btn_magnet);
        ShowOrHide(gm.Invincible, btn_invincible);
        ShowOrHide(gm.Multiply, btn_multiply);
    }
    public void ShowOrHide(int i, Button btn)
    {
        if (i > 0)
        {
            btn.interactable = true;
            btn.transform.Find("Mask").gameObject.SetActive(false);
        }
        else
        {
            btn.interactable = false;
            btn.transform.Find("Mask").gameObject.SetActive(true);
        }
    }

    private void OnInvincibleClick()
    {
        ItemArg e = new()
        {
            kind = ITEMKIND.ITEMINVINCIBLE,
            hitCount = 1
        };
        SendEvent(Const.E_HitItem,e);
    }
    private void OnMultiplyClick()
    {
        ItemArg e = new()
        {
            kind = ITEMKIND.ITEMMULTIPLY,
            hitCount = 1
        };
        SendEvent(Const.E_HitItem, e);
    }
    private void OnMagnetClick()
    {
        ItemArg e = new()
        {
            kind = ITEMKIND.ITEMMAGNET,
            hitCount = 1
        };
        SendEvent(Const.E_HitItem, e);
    }
    /// <summary>
    /// ------------------------射门-------------------
    /// </summary>
    //碰到射门触发器，射门按钮在一定时间内可点击
    public void HitGoalTrigger()
    {
        StartCoroutine(GoalTriggerCoroutine());
    }
    IEnumerator GoalTriggerCoroutine()
    {
        btn_goal.interactable = true;
        sliGoal.value = 1;
        while (sliGoal.value>0)
        {
            if (!gm.IsPause&&gm.IsPlay)
            {
                sliGoal.value -= 0.8f * Time.deltaTime;
            }
            yield return 0;
        }
        btn_goal.interactable = false;
        sliGoal.value = 0;
    }
    //点击射门按钮
    public void OnGoalClick()
    {
        SendEvent(Const.E_OnGoalClick);//发送到playerMove
        sliGoal.value = 0;
    }
    #endregion

    #region 回调事件
    public override void RegisterAttentionEvent()
    {
        AttentionList.Add(Const.E_UpdateDis);
        AttentionList.Add(Const.E_UpdateCoins);
        AttentionList.Add(Const.E_HitAddTime);
        AttentionList.Add(Const.E_HitGoalTrigger);
        AttentionList.Add(Const.E_ShootGoal);
    }
    public override void HandleEvent(string name, object data)
    {
        switch (name)
        {
            case Const.E_UpdateDis:
                DistanceArg e1 = data as DistanceArg;
                Distance = e1.distance;
                break;
            case Const.E_UpdateCoins:
                CoinArg e2 = data as CoinArg;
                Coins += e2.coins;
                break;
            case Const.E_HitAddTime:
                Times += 20;
                break;
            case Const.E_HitGoalTrigger://玩家进入到球门触发器
                HitGoalTrigger();
                break;
            case Const.E_ShootGoal:
                m_goalCount += 1;
                Debug.Log("进了"+m_goalCount+"球");
                break;
        }
    }
    #endregion

}
