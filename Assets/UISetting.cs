using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UISetting : View
{
    [SerializeField] Button btn_sound;
    [SerializeField] Button btn_cancle;
    [SerializeField] Button btn_resetData;
    [SerializeField] GameObject model;
    [SerializeField] List<Sprite> soundSprList = new();
    GameModel gm;
    private void Awake()
    {
        gm = GetModel<GameModel>();
        btn_sound.onClick.AddListener(OnSoundClick);
        btn_cancle.onClick.AddListener(OnCancleClick);
        btn_resetData.onClick.AddListener(OnResetDataClick);
    }

    #region °´Å¥µã»÷
    private void OnSoundClick()
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        if (gm.Sound == 1)
        {
            btn_sound.GetComponent<Image>().sprite = soundSprList[0];
            gm.Sound = 0;
            Game.Instance.soundManager.PauseSound();
        }
        else if (gm.Sound == 0)
        {
            btn_sound.GetComponent<Image>().sprite = soundSprList[1];
            gm.Sound = 1;
            Game.Instance.soundManager.PlaySound();
            if (!Game.Instance.soundManager.m_bg.isPlaying)
            {
                Game.Instance.soundManager.PlayBg("Bgm_JieMian");
            }
        }
    }
    private void OnCancleClick()
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        Hide();
    }
    private void OnResetDataClick()
    {
        Game.Instance.soundManager.PlayEffect("Se_UI_Button");
        PlayerPrefs.DeleteAll();
        
    }
    #endregion
    #region ÏÔÒþ
    public void Show()
    {
        gameObject.SetActive(true);
        model.SetActive(false);
        if (gm.Sound == 1)
        {
            btn_sound.GetComponent<Image>().sprite = soundSprList[1];
        }
        else if (gm.Sound == 0)
        {
            btn_sound.GetComponent<Image>().sprite = soundSprList[0];
        }
    }
    public void Hide()
    {
        model.SetActive(true);
        model.GetComponentInChildren<SkinnedMeshRenderer>().material = Game.Instance.
            staticData.GetPlayerCloth(gm.TakeOnCloth.skinID, gm.TakeOnCloth.clothID).material;
       
        gameObject.SetActive(false);
    } 
    #endregion
    public override string Name { get { return Const.V_UISetting; } }

    public override void HandleEvent(string name, object data)
    {
        
    }
}
