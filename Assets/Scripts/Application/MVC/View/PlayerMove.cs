using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMove : View
{
    #region �ֶ�
    //������
    CharacterController m_cc;
    GameModel gm;

    [SerializeField] private float m_moveSpeed = 10;

    //�����ƶ�������ж�
    [SerializeField] INPUTDIRECTION m_inputDir = INPUTDIRECTION.NULL;
    bool activeInput;
    Vector3 m_inputMouse;

    //�����ƶ�
    float m_nowIndex = 1;
    float m_targetIndex = 1;
    float m_xDistance;
    float m_speed = 13;

    //��������
    float m_yDistance;
    float grivaty = 9.8f;
    float m_jumpValue = 5;

    //���Ʋ�����ʱ����
    [SerializeField] bool isSlide = false;
    float slideTime;

    //�ٶȸ���
    [SerializeField] float speedAddCount;
    float speedAddDistance = 200;
    float speedAddRate = 0.5f;
    float speedMax = 40;

    //�����ϰ������
    float m_maskSpeed;
    float addRate = 10;
    bool isHit = false;

    //item���
    [SerializeField] int m_doubleTime = 1;//�Ե�˫�����
    int m_skillTime;//������Чʱ��
    IEnumerator hitMultiplyCor;
    IEnumerator magnetCor;
    IEnumerator invincibleCor;
    SphereCollider m_MagnetCollider;
    bool isInvincible;

    //���Ž���
    GameObject m_Ball;
    GameObject m_Trail;
    IEnumerator moveBallCor;
    bool isGoal;

    //���������ģ�͸���
    [SerializeField] SkinnedMeshRenderer playerSkm;
    [SerializeField] MeshRenderer footballMr;
    #endregion

    #region ����
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

    #region �����ϰ���
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

    #region ������Ʒ
    //�Խ��
    private void HitCoin()
    {
        CoinArg e = new CoinArg { coins = m_doubleTime };
        SendEvent(Const.E_UpdateCoins, e);
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
    //����ʯ
    public void HitMagnet()
    {
        if (magnetCor != null)//Ϊ�˱��⼼��ʱ��û������ʱ��������һ������ʯ�������Ļ�������ͣ��һ����
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
        //֮���Բ�ֱ����yiled return new waitForSecond�ķ���������Ϊ��ͣ��ʱ�򣬼�ʱҲ�����
        //yield return new WaitForSeconds(m_skillTime);
        m_MagnetCollider.enabled = false;

    }

    //��ʱ��
    private void HitAddTime()
    {
        SendEvent(Const.E_HitAddTime);
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

    //������˫����ң��޵��ܵĻص�
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

    #region Unity�ص�
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
    /// �����ϰ���
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tag.smallFence))
        {
            if (isInvincible == true) return;
            //��Ч
            Game.Instance.soundManager.PlayEffect("Se_UI_Hit");
            //��Ч
            other.gameObject.SendMessage("HitPlayer", transform.position);
            //����
            HitObstacle();
        }
        else if (other.CompareTag(Tag.bigFence))
        {
            if (isSlide == true || isInvincible == true) return;
            //��Ч
            Game.Instance.soundManager.PlayEffect("Se_UI_Hit");
            //��Ч
            other.gameObject.SendMessage("HitPlayer", transform.position);
            //����
            HitObstacle();
        }
        else if (other.CompareTag(Tag.block))
        {
            //��Ч
            Game.Instance.soundManager.PlayEffect("Se_UI_End");
            //��Ч
            other.gameObject.SendMessage("HitPlayer", transform.position);

            //��Ϸ����

            SendEvent(Const.E_endGame);
            Debug.Log("�Ƿ�����Ϸ" + gm.IsPlay);
        }
        else if (other.CompareTag(Tag.smallBlock))
        {
            //��Ч
            Game.Instance.soundManager.PlayEffect("Se_UI_End");
            //��Ч
            other.gameObject.SendMessageUpwards("HitPlayer", transform.position);

            //��Ϸ����
            SendEvent(Const.E_endGame);
        }
        else if (other.CompareTag(Tag.beforeTrigger))
        {
            Debug.Log("����������������11111");
            other.gameObject.SendMessageUpwards("HitTrigger", SendMessageOptions.RequireReceiver);

        }
        else if (other.CompareTag(Tag.beforeGoalTrigger))//�������Ŵ�����  �õ����ť����
        {
            //����uiboard
            SendEvent(Const.E_HitGoalTrigger);

            Game.Instance.objectPool.Spawn("FX_JiaSu", m_Trail.transform.parent.transform);
        }
        else if (other.CompareTag(Tag.goalkeeper))//���û������ֱ��ײ������Ա
        {
            Debug.Log("��������Ա��");
            //����
            HitObstacle();
            other.SendMessageUpwards("HitGoalKeeper", SendMessageOptions.RequireReceiver);
        }
        else if (other.CompareTag(Tag.ballDoor))//û�е������ť��ײ������
        {
            if (isGoal)//������������
            {
                isGoal = false;
                return;
            }
            //����
            HitObstacle();
            //��������  һ��ʱ����Զ���ʧ
            Game.Instance.objectPool.Spawn("Effect_QiuWang", m_Trail.transform.parent);
            //���Ŷ����������ŵ�������ʧ
            other.SendMessageUpwards("HitDoor", m_nowIndex);
        }
    }
    #endregion

    #region UI����
    /// <summary>
    /// �������˶�Զ
    /// </summary>
    public void UpdateDis()
    {
        DistanceArg e = new DistanceArg { distance = (int)transform.position.z };


        SendEvent(Const.E_UpdateDis, e);
    }
    #endregion

    #region ���ŷ���
    /// <summary>
    /// �������ť
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
        SendMessage("SendPlayerMessage");//�������򶯻� ����PlayerAnim
        moveBallCor = MoveBall();
        StartCoroutine(moveBallCor);
    }
    IEnumerator MoveBall()//��ɳ�ȥ
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
    /// �����ť����֮��  �����������ŵ�ʱ�򱻴���
    public void HitGoalDoor()
    {
        //1.ֹͣЭ��  ��ɶ���ֹͣ
        StopCoroutine(moveBallCor);
        //2.��λ
        m_Trail.transform.localPosition = new Vector3(0, 0.66f, 3.23f);
        m_Trail.SetActive(false);
        m_Ball.SetActive(true);
        //3.������Ч
        Game.Instance.objectPool.Spawn("FX_GOAL", m_Trail.transform.parent.transform);
        //4.��������Ϊtrue
        isGoal = true;
        //5.�������Ч
        Game.Instance.soundManager.PlayEffect("Se_UI_Goal");

        //6.���Ž���֮��,��������Ʒ�
        SendEvent(Const.E_ShootGoal);
    }
    #endregion
    #region �¼��ص�
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
    #region �ƶ�����
    IEnumerator UpdateAction()
    {
        while (true)
        {
            if (gm.IsPause == false && gm.IsPlay == true)
            {
                //����ui
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
    /// ��������
    /// </summary>
    private void GetInputDirection()
    {
        m_inputDir = INPUTDIRECTION.NULL;
        if (Input.GetMouseButtonDown(0))
        {
            activeInput = true;
            m_inputMouse = Input.mousePosition;
            //Debug.Log("��һ�ε����λ����" + m_inputMouse);
        }
        if (Input.GetMouseButtonUp(0) && activeInput)
        {
            Vector3 dir = Input.mousePosition - m_inputMouse;

            //Debug.Log("�����ƶ��ĵ���" + Input.mousePosition);
            //Debug.Log("�����ĳ�����" + dir.magnitude);
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
    /// ����λ����Ϣ
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
    /// �����ƶ�
    /// </summary>
    private void MoveControl()
    {
        if (m_targetIndex != m_nowIndex)
        {
            float move = Mathf.Lerp(0, m_xDistance, m_speed * Time.deltaTime);
            transform.position += new Vector3(move, 0, 0);//�����ƶ�
            m_xDistance -= move;
            if (Mathf.Abs(m_xDistance) < 0.05f)//����λ�õ�ȷ��
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
