using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIShop : View
{
    #region �ֶ�
    [Header("***����װ***")]
    [SerializeField] private Toggle btn_footBall1;
    [SerializeField] private Toggle btn_footBall2;
    [SerializeField] private Toggle btn_footBall3;
    [SerializeField] MeshRenderer ball;//�������ϵ�����
    int selectIndex;
    GameModel gm;

    [SerializeField] ITEMSTATE state = ITEMSTATE.EQUIP;
    [SerializeField] Image img_buyButton;
    [SerializeField] Sprite buySprite;
    [SerializeField] Sprite equipSprite;

    [SerializeField] Sprite buySpr;
    [SerializeField] Sprite equipSpr;
    [SerializeField] Sprite unbuySpr;

    [SerializeField] List<Image> footballGizmoList = new();

    [Header("***����Ƥ������***")]
    [SerializeField] Toggle btn_momo;
    [SerializeField] Toggle btn_sali;
    [SerializeField] Toggle btn_sugar;
    [SerializeField] Button btn_buySkin;
    [SerializeField] SkinnedMeshRenderer playerSkm;
    [SerializeField] Image img_buyCloth;
    [SerializeField] Image img_buySkin;

    [SerializeField] List<Image> skinGizmoList = new();

    [Header("***�����·�����***")]
    [SerializeField] Toggle btn_normal;
    [SerializeField] Toggle btn_baxi;
    [SerializeField] Toggle btn_german;
    [SerializeField] Button btn_buyCloth;
    [SerializeField] List<Image> clothGizmo = new();

    [Header("����ѡ��İ�ť")]
    [SerializeField] Toggle btn_skin;
    [SerializeField] Toggle btn_cloth;
    [SerializeField] Toggle btn_football;

    [Header("***ͷ��ui�ĸ���***")]
    [SerializeField] List<Sprite> head = new();
    [SerializeField] TextMeshProUGUI txt_coin;
    [SerializeField] TextMeshProUGUI txt_name;
    [SerializeField] Image img_head;
    [SerializeField] TextMeshProUGUI txt_grade;

    [Header("***�ײ���ť***")]
    [SerializeField] Button btn_playGame;
    [SerializeField] Button btn_return;
    [SerializeField] Button btn_set;

    [Header("����Ʒ�۸�")]
    [SerializeField] TextMeshProUGUI txt_momoPrice;
    [SerializeField] TextMeshProUGUI txt_saliPrice;
    [SerializeField] TextMeshProUGUI txt_sugarPrice;

    [SerializeField] TextMeshProUGUI txt_Normal;
    [SerializeField] TextMeshProUGUI txt_Baxi;
    [SerializeField] TextMeshProUGUI txt_German;
    #endregion

    #region Unity�ص�
    private void Awake()
    {
        gm = GetModel<GameModel>();
        //����ťע��
        btn_footBall1.onValueChanged.AddListener(NormalFootBall);
        btn_footBall2.onValueChanged.AddListener(FireFootBall);
        btn_footBall3.onValueChanged.AddListener(ColorFootBall);
        img_buyButton.GetComponent<Button>().onClick.AddListener(OnBuyFootballClick);
        //���ﻻ����ťע��
        btn_momo.onValueChanged.AddListener(OnMomoClick);
        btn_sali.onValueChanged.AddListener(OnSaliClick);
        btn_sugar.onValueChanged.AddListener(OnSugarClick);
        btn_buySkin.onClick.AddListener(OnBuySkinButtonClick);
        //�����·���ť����
        btn_normal.onValueChanged.AddListener(OnNormalClick);
        btn_baxi.onValueChanged.AddListener(OnBaxiClick);
        btn_german.onValueChanged.AddListener(OnGermanClick);
        btn_buyCloth.onClick.AddListener(OnBuyClothButtonClick);
        //����ѡ�ťע��
        btn_skin.onValueChanged.AddListener(OnSkinToggleClick);
        btn_cloth.onValueChanged.AddListener(OnClothToggleClick);
        btn_football.onValueChanged.AddListener(OnFootballToggleClick);
        //�ײ���ť
        btn_playGame.onClick.AddListener(OnPlayGameClick);
        btn_return.onClick.AddListener(OnReturnClick);
        //btn_set.onClick.AddListener(OnSettingClick);

        UpdateUI();//��ʼ������ť
        UpdateSkinGizmo();//��ʼ������״̬
        InitPrice();//��ʼ������۸�
        ShowClothPrice();//��ʼ���·��۸�

        BuyID defalt = new() { skinID = 0, clothID = 0 };
        gm.m_buyPlayerClothList.Add(defalt);

        playerSkm.material = Game.Instance.staticData.GetPlayerCloth(gm.TakeOnCloth.skinID, gm.TakeOnCloth.clothID).material;
        ball.material = Game.Instance.staticData.GetFootBallInfo(gm.TakeOnFootBall).material;
        txt_grade.text = gm.Grade.ToString();
    }
    #endregion
    /// <summary>
    /// ��ʼ������۸�
    /// </summary>
    private void InitPrice()
    {
        txt_momoPrice.text = Game.Instance.staticData.GetPlayerCloth(0, 0).coins.ToString();
        txt_saliPrice.text = Game.Instance.staticData.GetPlayerCloth(1, 0).coins.ToString();
        txt_sugarPrice.text = Game.Instance.staticData.GetPlayerCloth(2, 0).coins.ToString();
    }
    private void ShowClothPrice()
    {
        txt_Normal.text = Game.Instance.staticData.GetPlayerCloth(selectIndex, 0).coins.ToString();
        txt_Baxi.text = Game.Instance.staticData.GetPlayerCloth(selectIndex, 1).coins.ToString();
        txt_German.text = Game.Instance.staticData.GetPlayerCloth(selectIndex, 2).coins.ToString();
    }
    #region ��ť����
    private void OnSettingClick()
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        SendEvent(Const.E_Setting);
    }
    private void OnPlayGameClick()
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        Game.Instance.LoadScene(3);
    }
    private void OnReturnClick()
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        if (gm.lastIndex == 4)
        {
            gm.lastIndex = 1;
        }
        Game.Instance.LoadScene(gm.lastIndex);
    } 
    #endregion

    #region ����Ƥ��
    //����δ�����ѹ���װ��������
    public void UpdateFootballGizmo()
    {
        for (int i = 0; i < 3; i++)
        {
            state = gm.CheackFootballState(i);
            switch (state)
            {
                case ITEMSTATE.BUY:
                    footballGizmoList[i].sprite = buySpr;
                    break;
                case ITEMSTATE.UNBUY:
                    footballGizmoList[i].sprite = unbuySpr;
                    break;
                case ITEMSTATE.EQUIP:
                    footballGizmoList[i].sprite = equipSpr;
                    break;
                default:
                    break;
            }
        }
    }
    //�������ť
    private void OnBuyFootballClick()
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        state = gm.CheackFootballState(selectIndex);
        switch (state)
        {
            case ITEMSTATE.BUY:
                //װ��
                int moneys = Game.Instance.staticData.m_footBallInfo[selectIndex].coins;
                BuyFootballArg ee = new()
                {
                    coin = moneys,
                    index = selectIndex,
                };
                //�����¼�
                SendEvent(Const.E_EquipFootball,ee);
                break;
            case ITEMSTATE.UNBUY:
                //�������ť
                int money = Game.Instance.staticData.m_footBallInfo[selectIndex].coins;
                
                BuyFootballArg e = new()
                {
                    coin = money,
                    index = selectIndex,
                };
                //���͹���ť�¼�
                SendEvent(Const.E_BuyFootball,e);
                break;
            case ITEMSTATE.EQUIP:
                break;
            default:
                break;
        }
        UpdateBuyFootballButton(selectIndex);
        UpdateFootballGizmo();
    }
    //���¹���ui
    public void UpdateBuyFootballButton(int i)
    {
        state = gm.CheackFootballState(i);
        switch (state)
        {
            case ITEMSTATE.BUY:
                img_buyButton.transform.gameObject.SetActive(true);
                img_buyButton.sprite = equipSprite;
                break;
            case ITEMSTATE.UNBUY:
                img_buyButton.transform.gameObject.SetActive(true);
                img_buyButton.sprite = buySprite;
                break;
            case ITEMSTATE.EQUIP:
                img_buyButton.transform.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    //���򻻷�
    private void UpdateMatAndUI(int i)
    {
        //buy��ť�ĸ���
        UpdateBuyFootballButton(i);
        //������ʵĸ���
        ball.material = Game.Instance.staticData.GetFootBallInfo(i).material;
    }
    private void NormalFootBall(bool isON)
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Dress");
        selectIndex = 0;
        UpdateMatAndUI(selectIndex);
    } 
    private void FireFootBall(bool isON)
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Dress");
        selectIndex = 1;
        UpdateMatAndUI(selectIndex);
    }
    private void ColorFootBall(bool isON)
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Dress");
        selectIndex = 2;
        UpdateMatAndUI(selectIndex);
    }
    #endregion

    #region ���ﻻ��
    //��ťע���¼�
    private void ChangePlayerSkin(int i,int j)
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Dress");
        playerSkm.material = Game.Instance.staticData.GetPlayerCloth(i, j).material;

        UpdateBuySkinButton();
    }
    //������ť
    private void OnMomoClick(bool isOn)
    {
        selectIndex = 0;
        ChangePlayerSkin(selectIndex, 0);
        
    }
    private void OnSaliClick(bool isOn)
    {
        selectIndex = 1;
        ChangePlayerSkin(selectIndex, 0);
       
    }
    private void OnSugarClick(bool isOn)
    {
        selectIndex = 2;
        ChangePlayerSkin(selectIndex, 0);
       
    }

    //������ﰴť������ťUI����
    public void UpdateBuySkinButton()
    {
        state = gm.CheckSkinState(selectIndex);
        switch (state)
        {
            case ITEMSTATE.BUY:
                img_buySkin.transform.gameObject.SetActive(true);
                img_buySkin.sprite = equipSprite;
                break;
            case ITEMSTATE.UNBUY:
                img_buySkin.transform.gameObject.SetActive(true);
                img_buySkin.sprite = buySprite;
                break;
            case ITEMSTATE.EQUIP:
                img_buySkin.transform.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
    //�������Ƥ���İ�ť,UI����
    public void OnBuySkinButtonClick()
    {
        state = gm.CheckSkinState(selectIndex);
        switch (state)
        {
            case ITEMSTATE.BUY:
                //װ��
                BuyID ID = new() { skinID = selectIndex, clothID = 0 };
                int money = Game.Instance.staticData.GetPlayerCloth(selectIndex, 0).coins;
                BuyClothArg e = new()
                {
                    coin = money,
                    id = ID
                };
                ShowClothPrice();
                SendEvent(Const.E_EquipCloth,e);
                break;
            case ITEMSTATE.UNBUY:
                //�������ť
                BuyID Id = new() {skinID=selectIndex,clothID=0 };
                int moneys = Game.Instance.staticData.GetPlayerCloth(selectIndex,0).coins;
                BuyClothArg ee = new()
                {
                    coin = moneys,
                    id = Id
                };
                SendEvent(Const.E_BuyCloth,ee);
     
                //���͹���ť�¼�
               
                break;
            case ITEMSTATE.EQUIP:
                break;

        }
        UpdateBuySkinButton();
        UpdateSkinGizmo();
    }

    //����δ�����ѹ���װ��������
    public void UpdateSkinGizmo()
    {
        foreach (var item in gm.m_buyPlayerClothList)
        {
            Debug.Log("�Ѿ��������Ʒ��" + item.skinID + " " + item.clothID);
        }
        for (int i = 0; i < 3; i++)
        {
            //�жϸ�Ƥ��״̬
            state = gm.CheckSkinState(i);
            switch (state)
            {
                case ITEMSTATE.BUY:
                    skinGizmoList[i].sprite = buySpr;
                    break;
                case ITEMSTATE.UNBUY:
                    skinGizmoList[i].sprite = unbuySpr;
                    break;
                case ITEMSTATE.EQUIP:
                    skinGizmoList[i].sprite = equipSpr;
                    break;
                default:
                    break;
            }
        }
    }
    #endregion

    #region �����·�����
    //�·���ť�ĵ��
    private void ChangePlayerCloth(int i)
    {
        i = selectIndex;
        playerSkm.material = Game.Instance.staticData.m_playerClothData[gm.TakeOnCloth.skinID][i].material;
        UpdateBuyClothUI();
    }
    private void OnNormalClick(bool isOn)
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Dress");
        selectIndex = 0;
        ChangePlayerCloth(selectIndex);
    }
    private void OnBaxiClick(bool isOn)
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Dress");
        selectIndex = 1;
        ChangePlayerCloth(selectIndex);
    }
    private void OnGermanClick(bool isOn)
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Dress");
        selectIndex = 2;
        ChangePlayerCloth(selectIndex);
    } 
    //����ť��ui����
    private void UpdateBuyClothUI()
    {
        BuyID id = new() {skinID=gm.TakeOnCloth.skinID,clothID=selectIndex };
        state = gm.CheckClothState(id);
        switch (state)
        {
            case ITEMSTATE.BUY:
                img_buyCloth.transform.gameObject.SetActive(true);
                img_buyCloth.sprite = equipSprite;
                break;
            case ITEMSTATE.UNBUY:
                img_buyCloth.transform.gameObject.SetActive(true);
                img_buyCloth.sprite = buySprite;
                break;
            case ITEMSTATE.EQUIP:
                img_buyCloth.transform.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
    //�������ť�ĸ���
    private void OnBuyClothButtonClick()
    {
        BuyID ID = new() {skinID=gm.TakeOnCloth.skinID,clothID=selectIndex };
        state = gm.CheckClothState(ID);
        switch (state)
        {
            case ITEMSTATE.BUY:
                int moneys = Game.Instance.staticData.m_playerClothData[gm.TakeOnCloth.skinID][selectIndex].coins;
                BuyClothArg ee = new() { coin = moneys,id=ID };
                SendEvent(Const.E_EquipCloth,ee);//�Ѵ��ϵ��·���ţ���¼����
                break;
            case ITEMSTATE.UNBUY:
                int money = Game.Instance.staticData.m_playerClothData[gm.TakeOnCloth.skinID][selectIndex].coins;
                BuyClothArg e = new BuyClothArg() {coin=money,id=ID };
                SendEvent(Const.E_BuyCloth,e);//�ж�Ǯ�����������Ļ�������Ķ�����ӵ��б�����
                break;
            case ITEMSTATE.EQUIP:
                break;
            default:
                break;
        }
        //���°�ťui
        UpdateBuyClothUI();
        //�����·���gizmo
        UpdateClothGizmo();
    }
    private void UpdateClothGizmo()
    {
       
        for (int i = 0; i < 3; i++)
        {
            BuyID id = new() { skinID = gm.TakeOnCloth.skinID, clothID = i };
            state = gm.CheckClothState(id);
            switch (state)
            {
                case ITEMSTATE.BUY:
                    clothGizmo[i].sprite = buySpr;
                    break;
                case ITEMSTATE.UNBUY:
                    clothGizmo[i].sprite = unbuySpr;
                    break;
                case ITEMSTATE.EQUIP:
                    clothGizmo[i].sprite = equipSpr;
                    break;
                default:
                    break;
            }
        }
    }
    #endregion

    #region ��������Ͱ�ť����
    private void UpdateToggleUI()
    {
        
    }
    private void OnSkinToggleClick(bool isOn)
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        UpdateSkinGizmo();
        Hide();
    }
    private void OnClothToggleClick(bool isOn)
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        UpdateClothGizmo();
        Hide();
    }
    private void OnFootballToggleClick(bool isOn)
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        UpdateFootballGizmo();
        Hide();
    } 
    private void Hide()
    {
        img_buyButton.transform.gameObject.SetActive(false);
        btn_buyCloth.transform.gameObject.SetActive(false);
        btn_buySkin.transform.gameObject.SetActive(false);
    }
    #endregion

    #region ���½�Ǯ��ͷ���ui��Ϣ
    public void UpdateUI()
    {
        //���½�Ǯ����
        txt_coin.text = gm.Coin.ToString();
        //��������
        switch (gm.TakeOnCloth.skinID)
        {
            case 0:
                txt_name.text = "XiJie";
                break;
            case 1:
                txt_name.text = "FanFan";
                break;
            case 2:
                txt_name.text = "Test";
                break;
            default:
                break;
        }
        //����ͷ��
        img_head.sprite = head[gm.TakeOnCloth.skinID];
    } 
    #endregion

    #region ����
    public override string Name { get { return Const.V_UIShop; } }
    #endregion

    public override void HandleEvent(string name, object data)
    {

    }
}
