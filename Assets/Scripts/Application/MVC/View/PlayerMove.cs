using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMove : View
{
    #region 字段
    //获得组件
    CharacterController m_cc;
    GameModel gm;

    [SerializeField] private float m_moveSpeed = 10;

    //手势移动方向的判断
    [SerializeField] INPUTDIRECTION m_inputDir = INPUTDIRECTION.NULL;
    bool activeInput;
    Vector3 m_inputMouse;

    //手势移动
    float m_nowIndex = 1;
    float m_targetIndex = 1;
    float m_xDistance;
    float m_speed = 13;

    //动画部分
    float m_yDistance;
    float grivaty = 9.8f;
    float m_jumpValue = 5;

    //手势操作的时间间隔
    [SerializeField] bool isSlide = false;
    float slideTime;

    //速度更新
    [SerializeField] float speedAddCount;
    float speedAddDistance = 200;
    float speedAddRate = 0.5f;
    float speedMax = 40;

    //碰到障碍物减速
    float m_maskSpeed;
    float addRate = 10;
    bool isHit = false;

    //item相关
    [SerializeField] int m_doubleTime = 1;//吃到双倍金币
    int m_skillTime;//技能有效时间
    IEnumerator hitMultiplyCor;
    IEnumerator magnetCor;
    IEnumerator invincibleCor;
    SphereCollider m_MagnetCollider;
    bool isInvincible;

    //射门进球
    GameObject m_Ball;
    GameObject m_Trail;
    IEnumerator moveBallCor;
    bool isGoal;

    //人物和足球模型更新
    [SerializeField] SkinnedMeshRenderer playerSkm;
    [SerializeField] MeshRenderer footballMr;
    #endregion

    #region 属性
    public override string Name => Const.V_PlayerMove;

    public float MoveSpeed
    {
        get => m_moveSpeed; set
        {
            m_moveSpeed = value;
            if (m_moveSpeed > speedMax)
            {
                m_moveSpeed = speedMax;
            }
        }
    }
    #endregion

    #region 碰到障碍物
    private void HitObstacle()
    {
        if (isHit == true) return;
        isHit = true;
        m_maskSpeed = MoveSpeed;
        MoveSpeed = 0;
        StartCoroutine(DecreaseSpeed());
    }
    IEnumerator DecreaseSpeed()
    {
        while (MoveSpeed <= m_maskSpeed)
        {
            MoveSpeed += addRate * Time.deltaTime;
            yield return 0;
        }
        isHit = false;
    }
    #endregion

    #region 奖励物品
    //吃金币
    private void HitCoin()
    {
        CoinArg e = new CoinArg { coins = m_doubleTime };
        SendEvent(Const.E_UpdateCoins, e);
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
        m_doubleTime = 2;
        float timer = m_skillTime;
        while (timer > 0)
        {
            if (gm.IsPlay && !gm.IsPause)
            {
                timer -= Time.deltaTime;
            }
            yield return 0;
        }
        //yield return new WaitForSeconds(m_skillTime);
        m_doubleTime = 1;
    }
    //吸铁石
    public void HitMagnet()
    {
        if (magnetCor != null)//为了避免技能时间没结束的时候，碰到下一个吸铁石，碰到的话，就暂停上一个的
        {
            StopCoroutine(magnetCor);
        }
        magnetCor = MagnetCoroutine();
        StartCoroutine(magnetCor);
    }
    IEnumerator MagnetCoroutine()
    {
        m_MagnetCollider.enabled = true;
        float timer = m_skillTime;
        while (timer > 0)
        {
            if (gm.IsPlay && !gm.IsPause)
            {
                timer -= Time.deltaTime;
            }
            yield return 0;
        }
        //之所以不直接用yiled return new waitForSecond的方法，是因为暂停的时候，计时也会继续
        //yield return new WaitForSeconds(m_skillTime);
        m_MagnetCollider.enabled = false;

    }

    //加时间
    private void HitAddTime()
    {
        SendEvent(Const.E_HitAddTime);
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
        isInvincible = true;
        float timer = m_skillTime;
        while (timer > 0)
        {
            if (gm.IsPlay && !gm.IsPause)
            {
                timer -= Time.deltaTime;
            }
            yield return 0;
        }
        //yield return new WaitForSeconds(m_skillTime);
        isInvincible = false;
    }

    //磁铁，双倍金币，无敌总的回调
    public void HitItem(ITEMKIND itemKind)
    {
        ItemArg e = new ItemArg
        {
            kind = itemKind,
            hitCount = 0,
        };
        SendEvent(Const.E_HitItem, e);
        //switch (itemKind)
        //{
        //    case ITEMKIND.ITEMINVINCIBLE:
        //        HitInvincible();
        //        break;
        //    case ITEMKIND.ITEMMAGNET:
        //        HitMagnet();
        //        break;
        //    case ITEMKIND.ITEMMULTIPLY:
        //        HitMultiply();
        //        break;
        //    default:
        //        break;
        //}
    }
    #endregion

    #region Unity回调
    private void Awake()
    {
        m_Ball = transform.Find("Ball").gameObject;
        m_Trail = GameObject.Find("trail").gameObject;
        m_Trail.SetActive(false);

        m_cc = GetComponent<CharacterController>();
        gm = GetModel<GameModel>();
        m_skillTime = gm.SkillTime;

        m_MagnetCollider = GetComponentInChildren<SphereCollider>();
        m_MagnetCollider.enabled = false;

        playerSkm.material = Game.Instance.staticData.GetPlayerCloth(gm.TakeOnCloth.skinID, gm.TakeOnCloth.clothID).material;
        footballMr.material = Game.Instance.staticData.GetFootBallInfo(gm.TakeOnFootBall).material;
    }
    private void Start()
    {
        StartCoroutine(UpdateAction());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            gm.IsPause = true;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            gm.IsPause = false;
        }
    }
    /// <summary>
    /// 碰到障碍物
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tag.smallFence))
        {
            if (isInvincible == true) return;
            //音效
            Game.Instance.soundManager.PlayEffect("Se_UI_Hit");
            //特效
            other.gameObject.SendMessage("HitPlayer", transform.position);
            //减速
            HitObstacle();
        }
        else if (other.CompareTag(Tag.bigFence))
        {
            if (isSlide == true || isInvincible == true) return;
            //音效
            Game.Instance.soundManager.PlayEffect("Se_UI_Hit");
            //特效
            other.gameObject.SendMessage("HitPlayer", transform.position);
            //减速
            HitObstacle();
        }
        else if (other.CompareTag(Tag.block))
        {
            //音效
            Game.Instance.soundManager.PlayEffect("Se_UI_End");
            //特效
            other.gameObject.SendMessage("HitPlayer", transform.position);

            //游戏结束

            SendEvent(Const.E_endGame);
            Debug.Log("是否在游戏" + gm.IsPlay);
        }
        else if (other.CompareTag(Tag.smallBlock))
        {
            //音效
            Game.Instance.soundManager.PlayEffect("Se_UI_End");
            //特效
            other.gameObject.SendMessageUpwards("HitPlayer", transform.position);

            //游戏结束
            SendEvent(Const.E_endGame);
        }
        else if (other.CompareTag(Tag.beforeTrigger))
        {
            Debug.Log("碰到汽车触发器了11111");
            other.gameObject.SendMessageUpwards("HitTrigger", SendMessageOptions.RequireReceiver);

        }
        else if (other.CompareTag(Tag.beforeGoalTrigger))//碰到球门触发器  让点击按钮可用
        {
            //传到uiboard
            SendEvent(Const.E_HitGoalTrigger);

            Game.Instance.objectPool.Spawn("FX_JiaSu", m_Trail.transform.parent.transform);
        }
        else if (other.CompareTag(Tag.goalkeeper))//如果没有踢球，直接撞上守门员
        {
            Debug.Log("碰到守门员了");
            //减速
            HitObstacle();
            other.SendMessageUpwards("HitGoalKeeper", SendMessageOptions.RequireReceiver);
        }
        else if (other.CompareTag(Tag.ballDoor))//没有点击踢球按钮，撞到球门
        {
            if (isGoal)//如果踢球进球了
            {
                isGoal = false;
                return;
            }
            //减速
            HitObstacle();
            //生成球网  一段时间后自动消失
            Game.Instance.objectPool.Spawn("Effect_QiuWang", m_Trail.transform.parent);
            //球门动画，和球门的球网消失
            other.SendMessageUpwards("HitDoor", m_nowIndex);
        }
    }
    #endregion

    #region UI更新
    /// <summary>
    /// 更新跑了多远
    /// </summary>
    public void UpdateDis()
    {
        DistanceArg e = new DistanceArg { distance = (int)transform.position.z };


        SendEvent(Const.E_UpdateDis, e);
    }
    #endregion

    #region 射门方法
    /// <summary>
    /// 点击射球按钮
    /// </summary>
    public void OnGoalClick()
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        if (moveBallCor != null)
        {
            StopCoroutine(moveBallCor);
        }
        m_Trail.SetActive(true);
        m_Ball.SetActive(false);
        SendMessage("SendPlayerMessage");//播放射球动画 传到PlayerAnim
        moveBallCor = MoveBall();
        StartCoroutine(moveBallCor);
    }
    IEnumerator MoveBall()//球飞出去
    {
        while (true)
        {
            if (!gm.IsPause && gm.IsPlay)
            {
                m_Trail.transform.Translate(transform.forward * 40 * Time.deltaTime);
            }
            yield return 0;
        }
    }
    /// 点击按钮射球之后  在球碰到球门的时候被触发
    public void HitGoalDoor()
    {
        //1.停止协程  球飞动画停止
        StopCoroutine(moveBallCor);
        //2.归位
        m_Trail.transform.localPosition = new Vector3(0, 0.66f, 3.23f);
        m_Trail.SetActive(false);
        m_Ball.SetActive(true);
        //3.产生特效
        Game.Instance.objectPool.Spawn("FX_GOAL", m_Trail.transform.parent.transform);
        //4.进门设置为true
        isGoal = true;
        //5.进球的音效
        Game.Instance.soundManager.PlayEffect("Se_UI_Goal");

        //6.射门进球之后,进球分数计分
        SendEvent(Const.E_ShootGoal);
    }
    #endregion
    #region 事件回调
    public override void RegisterAttentionEvent()
    {
        AttentionList.Add(Const.E_OnGoalClick);
    }
    public override void HandleEvent(string name, object data)
    {
        switch (name)
        {
            case Const.E_OnGoalClick:
                OnGoalClick();
                break;
            default:
                break;
        }
    }
    #endregion
    #region 移动方法
    IEnumerator UpdateAction()
    {
        while (true)
        {
            if (gm.IsPause == false && gm.IsPlay == true)
            {
                //更新ui
                UpdateDis();

                m_yDistance -= grivaty * Time.deltaTime;
                m_cc.Move((transform.forward * MoveSpeed + new Vector3(0, m_yDistance, 0)) * Time.deltaTime);
                MoveControl();
                UpdatePosition();
                UpdateSpeed();
            }
            yield return 0;
        }
    }

    private void UpdateSpeed()
    {
        if (speedAddCount < speedAddDistance)
        {
            speedAddCount += Time.deltaTime;
            if (speedAddCount >= speedAddDistance)
            {
                speedAddCount = 0;
                MoveSpeed += speedAddRate;
            }
        }
    }

    /// <summary>
    /// 手势输入
    /// </summary>
    private void GetInputDirection()
    {
        m_inputDir = INPUTDIRECTION.NULL;
        if (Input.GetMouseButtonDown(0))
        {
            activeInput = true;
            m_inputMouse = Input.mousePosition;
            //Debug.Log("第一次点击的位置是" + m_inputMouse);
        }
        if (Input.GetMouseButtonUp(0) && activeInput)
        {
            Vector3 dir = Input.mousePosition - m_inputMouse;

            //Debug.Log("按着移动的点是" + Input.mousePosition);
            //Debug.Log("向量的长度是" + dir.magnitude);
            if (dir.magnitude > 20)
            {
                if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y) && dir.x > 0)
                {
                    m_inputDir = INPUTDIRECTION.RIGHT;
                }
                else if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y) && dir.x < 0)
                {
                    m_inputDir = INPUTDIRECTION.LEFT;
                }
                else if (Mathf.Abs(dir.x) < Mathf.Abs(dir.y) && dir.y > 0)
                {
                    m_inputDir = INPUTDIRECTION.UP;
                }
                else if (Mathf.Abs(dir.x) < Mathf.Abs(dir.y) && dir.y < 0)
                {
                    m_inputDir = INPUTDIRECTION.DOWN;
                }

            }

            activeInput = false;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            m_inputDir = INPUTDIRECTION.UP;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            m_inputDir = INPUTDIRECTION.DOWN;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            m_inputDir = INPUTDIRECTION.LEFT;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            m_inputDir = INPUTDIRECTION.RIGHT;
        }
        //Debug.Log(m_inputDir);
    }
    /// <summary>
    /// 更新位置信息
    /// </summary>
    private void UpdatePosition()
    {
        GetInputDirection();
        switch (m_inputDir)
        {
            case INPUTDIRECTION.NULL:
                break;
            case INPUTDIRECTION.LEFT:
                if (m_targetIndex > 0)
                {
                    m_targetIndex--;
                    m_xDistance = -2;
                    SendMessage("AnimManager", m_inputDir);
                    Game.Instance.soundManager.PlayEffect("Se_UI_Huadong");
                }
                break;
            case INPUTDIRECTION.RIGHT:
                if (m_targetIndex < 2)
                {
                    m_targetIndex++;
                    m_xDistance = 2;
                    SendMessage("AnimManager", m_inputDir);
                    Game.Instance.soundManager.PlayEffect("Se_UI_Huadong");
                }
                break;
            case INPUTDIRECTION.UP:
                if (m_cc.isGrounded)
                {
                    m_yDistance = m_jumpValue;
                    SendMessage("AnimManager", m_inputDir);
                    Game.Instance.soundManager.PlayEffect("Se_UI_Jump");
                }
                break;
            case INPUTDIRECTION.DOWN:
                if (isSlide == false)
                {
                    isSlide = true;
                    slideTime = 0.733f;
                    SendMessage("AnimManager", m_inputDir);
                    Game.Instance.soundManager.PlayEffect("Se_UI_Slide");
                }
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 左右移动
    /// </summary>
    private void MoveControl()
    {
        if (m_targetIndex != m_nowIndex)
        {
            float move = Mathf.Lerp(0, m_xDistance, m_speed * Time.deltaTime);
            transform.position += new Vector3(move, 0, 0);//左右移动
            m_xDistance -= move;
            if (Mathf.Abs(m_xDistance) < 0.05f)//最终位置的确定
            {
                m_xDistance = 0;
                m_nowIndex = m_targetIndex;
                switch (m_nowIndex)
                {
                    case 0:
                        transform.position = new Vector3(-2, transform.position.y, transform.position.z);
                        break;
                    case 1:
                        transform.position = new Vector3(0, transform.position.y, transform.position.z);
                        break;
                    case 2:
                        transform.position = new Vector3(2, transform.position.y, transform.position.z);
                        break;
                    default:
                        break;
                }
            }
        }

        if (isSlide == true)
        {
            slideTime -= Time.deltaTime;
            if (slideTime <= 00)
            {
                isSlide = false;
                slideTime = 0;
            }
        }
    }
    #endregion
}
