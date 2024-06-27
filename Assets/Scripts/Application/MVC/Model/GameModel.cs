using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModel : Model
{
    #region ����
    const int initCoin = 5000;
    #endregion

    #region �ֶ�
    bool m_IsPlay = true;
    bool m_IsPause = false;
    int m_skillTime = 5;

    int m_magnet;
    int m_invincible;
    int m_multiply;

    int m_exp;
    int m_grade;
    int m_coin;
    int sound;
    public int lastIndex;
    //����
    int m_TakeOnFootBall = 0;//��ǰ���������
    public List<int> m_BuyFootball = new();//�Ѿ������������뼯��
    //Ƥ�����·�
    BuyID m_takeOnCloth = new() { skinID = PlayerPrefs.GetInt("SkinId", 0), clothID = PlayerPrefs.GetInt("ClothId", 0) };//��ǰ����
    public List<BuyID> m_buyPlayerClothList = new();//�ѹ�����·�����
    #endregion

    #region ����
    public override string Name { get { return Const.G_GameModel; } }

    public bool IsPlay { get => m_IsPlay; set => m_IsPlay = value; }
    public bool IsPause { get => m_IsPause; set => m_IsPause = value; }
    public int SkillTime { get => m_skillTime; set => m_skillTime = value; }
    public int Magnet { get { return PlayerPrefs.GetInt("Magnet",0); } set => PlayerPrefs.SetInt("Magnet",value); }
    public int Invincible { get{ return PlayerPrefs.GetInt("Invincible", 0); } set => PlayerPrefs.SetInt("Invincible",value); }
    public int Multiply { get => m_multiply; set => m_multiply = value; }
    ///��Ҫ���ݴ洢
    public int Exp
    {
        get { return PlayerPrefs.GetInt("Exp", 0); }
        set
        {
            while (value >= 500 + Grade * 100)
            {
                //����ֵ��ֵ�仯
                value -= (500 + Grade * 100);
                //����
                Grade++;
            }
            PlayerPrefs.SetInt("Exp",value);
        }
    }
    public int Grade { get { return PlayerPrefs.GetInt("Grade",1); } set => PlayerPrefs.SetInt("Grade",value); }
    public int Coin
    {
        get { return PlayerPrefs.GetInt("Coins", 5000); }
        set
        {
            //m_coin = value;
            PlayerPrefs.SetInt("Coins", value);
            Debug.Log(value);
        }
    }

    public int TakeOnFootBall { get { return PlayerPrefs.GetInt("TakeOnFootBall", 0); } set => PlayerPrefs.SetInt("TakeOnFootBall",value); }

    public BuyID TakeOnCloth {
        get {
            m_takeOnCloth.skinID = PlayerPrefs.GetInt("SkinId", 0);
            m_takeOnCloth.clothID = PlayerPrefs.GetInt("ClothId", 0);
            return m_takeOnCloth; } 
        set {
            PlayerPrefs.SetInt("SkinId", value.skinID);
            PlayerPrefs.SetInt("ClothId", value.clothID);

            //m_takeOnCloth.skinID = PlayerPrefs.GetInt("SkinId");
            //m_takeOnCloth.clothID = PlayerPrefs.GetInt("ClothId");
        } }
    public int Sound { get { return PlayerPrefs.GetInt("Sound",1); } set { PlayerPrefs.SetInt("Sound",value); } }


    #endregion
    #region ����
    public bool GetMoney(int coins)
    {
        if (Coin >= coins)
        {
            Coin -= coins;
            return true;
        }
        return false;
    }
    /// <summary>
    /// �������״̬
    /// </summary>
    public ITEMSTATE CheackFootballState(int i)
    { 
        if (i == m_TakeOnFootBall)
        {
            return ITEMSTATE.EQUIP;
        }
        else
        {
            if (m_BuyFootball.Contains(i))
            {
                return ITEMSTATE.BUY;
            }
            else
            {
                return ITEMSTATE.UNBUY;
            }
        }
    }
    /// <summary>
    /// �������Ƥ��״̬
    /// </summary>
    public ITEMSTATE CheckSkinState(int i)
    {
        if (i == TakeOnCloth.skinID)
        {
            return ITEMSTATE.EQUIP;
        }
        else
        {
            foreach (var x in m_buyPlayerClothList)
            {
                if (x.skinID == i)
                {
                    return ITEMSTATE.BUY;
                }
            }
            return ITEMSTATE.UNBUY;
        }
    }
    /// <summary>
    /// ��������·�״̬
    /// </summary>
    public ITEMSTATE CheckClothState(BuyID id)
    {
        if (TakeOnCloth.skinID == id.skinID && TakeOnCloth.clothID == id.clothID)
        {
            return ITEMSTATE.EQUIP;
        }
        else
        {
            foreach (var cloth in m_buyPlayerClothList)
            {
                if (cloth.skinID == id.skinID && cloth.clothID == id.clothID)
                {
                    return ITEMSTATE.BUY;
                }
            }
            return ITEMSTATE.UNBUY;
        }
    }
    #endregion
    #region ��ʼ��
    public void Init()
    {
        //Magnet = 3;
        //Invincible = 1;
        Multiply = 2;
        SkillTime = 5;
        //m_grade = 1;
        //m_exp = 0;
        //Coin = initCoin;
        InitSkin();
    }
    private void InitSkin()
    {
        //��ʼ������Ƥ��
        m_BuyFootball.Add(m_TakeOnFootBall);
        //��ʼ���·�Ƥ��
        m_buyPlayerClothList.Add(TakeOnCloth);
        foreach (var item in m_buyPlayerClothList)
        {
            Debug.Log("�Ѿ��������Ʒ��" + item.skinID + " " + item.clothID);
        }
    }
    #endregion
    #region Unity�ص�

    #endregion
}
public class BuyID
{
    public int skinID;
    public int clothID;
}

