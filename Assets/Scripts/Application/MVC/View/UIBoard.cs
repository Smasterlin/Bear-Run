using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIBoard : View
{
    #region ����
    private int startTime = 50;
    #endregion

    #region �ֶ�
    private int m_distance;
    private int m_coins;
    private float m_time;
    private int m_goalCount;
    [Header("��ң����룬��ʱ�们��������")]
    [SerializeField] TextMeshProUGUI txtDistance;
    [SerializeField] TextMeshProUGUI txtCoins;
    [SerializeField] TextMeshProUGUI txtSlideTime;
    [SerializeField] Slider sliderTime;

    GameModel gm;

    [Header("��ť")]
    [SerializeField] Button btn_pause;
    [SerializeField] Button btn_magnet;
    [SerializeField] Button btn_invincible;
    [SerializeField] Button btn_multiply;
    [SerializeField] Button btn_goal;//����
    [SerializeField] Slider sliGoal;

    [Header("������ʾʱ��")]
    [SerializeField] TextMeshProUGUI txtMagnet;
    [SerializeField] TextMeshProUGUI txtInvincible;
    [SerializeField] TextMeshProUGUI txtMultiply;

    //Э��
    IEnumerator hitMultiplyCor;
    IEnumerator magnetCor;
    IEnumerator invincibleCor;
    float m_skillTime;
    #endregion

    #region ����
    public override string Name { get { return Const.V_Board; } }

    public int Distance { get => m_distance; set { m_distance = value; txtDistance.text = value.ToString() + "��"; } }

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
    #region Unity�ص�
    private void Awake()
    {
        gm = GetModel<GameModel>();
        //��ťע���¼�
        btn_pause.onClick.AddListener(OnClickPause);
        btn_invincible.onClick.AddListener(OnInvincibleClick);
        btn_multiply.onClick.AddListener(OnMultiplyClick);
        btn_magnet.onClick.AddListener(OnMagnetClick);
        btn_goal.onClick.AddListener(OnGoalClick);

        //��ʼ��
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

    #region ����
    /// <summary>
    /// ����
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
    /// ----------------------������ܰ�ť------------------------
    /// </summary>

    string GetTime(float timer)
    {
        return ((int)timer + 1).ToString();
    }
    //��˫�����
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
    //����ʯ
    public void HitMagnet()
    {
        if (magnetCor != null)//Ϊ�˱��⼼��ʱ��û������ʱ��������һ������ʯ�������Ļ�������ͣ��һ����
        {
            StopCoroutine(magnetCor);
            Debug.Log("ֹͣ��һ��");
        }
        magnetCor = MagnetCoroutine();
        StartCoroutine(magnetCor);
    }
    IEnumerator MagnetCoroutine()
    {
        float timer = m_skillTime;
        Debug.Log("����ʱ����" + timer);
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
    //�޵�
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
    /// ------------------------------��ť�ķ���------------------------------------------
    /// </summary>
    //��ͣ
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
    /// ��ʼ�����½Ǽ��ܰ�ť����ʾ
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
    /// ------------------------����-------------------
    /// </summary>
    //�������Ŵ����������Ű�ť��һ��ʱ���ڿɵ��
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
    //������Ű�ť
    public void OnGoalClick()
    {
        SendEvent(Const.E_OnGoalClick);//���͵�playerMove
        sliGoal.value = 0;
    }
    #endregion

    #region �ص��¼�
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
            case Const.E_HitGoalTrigger://��ҽ��뵽���Ŵ�����
                HitGoalTrigger();
                break;
            case Const.E_ShootGoal:
                m_goalCount += 1;
                Debug.Log("����"+m_goalCount+"��");
                break;
        }
    }
    #endregion

}
