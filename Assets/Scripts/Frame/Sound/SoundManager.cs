using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleTon<SoundManager>
{
    [HideInInspector]
    public AudioSource m_bg;
    private AudioSource m_effect;
    [SerializeField]string ResourcesDir = "";
    GameModel gm;
    protected override void Awake()
    {
        base.Awake();
        
        m_bg = gameObject.AddComponent<AudioSource>();
        m_bg.playOnAwake = false;
        m_bg.loop = true;
        m_effect = gameObject.AddComponent<AudioSource>();
    }
    private void Start()
    {
        gm = MVC.GetModel<GameModel>();
    }
    public void PlayBg(string audioName)
    {
        if (gm.Sound==0)
        {
            return;
        }
        string oldName = "";
        if (m_bg.clip==null)
        {
            oldName = "";
        }
        else
        {
            oldName = m_bg.clip.name;
        }

        if (oldName!=audioName)
        {
            string path = ResourcesDir + "/" + audioName;
            AudioClip clip = Resources.Load<AudioClip>(path);

            if (clip!=null)
            {
                m_bg.clip = clip;
                m_bg.Play();
            }     
        }
    }

    public void PlayEffect(string audioName)
    {
        if (gm.Sound == 0)
        {
            return;
        }
        string path = ResourcesDir + "/" + audioName;
        AudioClip clip = Resources.Load<AudioClip>(path);

        if (clip!=null)
        {
            m_effect.PlayOneShot(clip);
        }
    }
    public void PauseSound()
    {
        m_bg.Pause();
    }
    public void PlaySound()
    {
        m_bg.Play();
    }
}
