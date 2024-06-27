using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerAnim : View
{
    Animation anim;
    Action playAnim;
    GameModel gm;
    public override string Name { get { return Const.V_PlayerAnim; } }

    #region Unity»Øµ÷
    private void Awake()
    {
        anim = GetComponent<Animation>();
        gm = GetModel<GameModel>();
    }
    private void Start()
    {
        playAnim = PlayRun;
    }
    private void Update()
    {
        if (playAnim != null)
        {
            if (gm.IsPause == false && gm.IsPlay == true)
            {
                playAnim();
            }
            else
            {
                anim.Stop();
            }

        }
    } 
    #endregion
    void PlayRun()
    {
        anim.Play("run");
    }

    void PlayLeft()
    {
        anim.Play("left_jump");
        if (anim["left_jump"].normalizedTime>0.95f)
        {
            playAnim = PlayRun;
        }
    }
    void PlayRight()
    {
        anim.Play("right_jump");
        if (anim["right_jump"].normalizedTime > 0.95f)
        {
            playAnim = PlayRun;
        }
    }
    void PlayJump()
    {
        anim.Play("jump");
        if (anim["jump"].normalizedTime > 0.95f)
        {
            playAnim = PlayRun;
        }
    }
    void PlayRoll()
    {
        anim.Play("roll");
        if (anim["roll"].normalizedTime > 0.95f)
        {
            playAnim = PlayRun;
        }
    }

    void PlayShoot()
    {
        anim.Play("Shoot01");
        if (anim["Shoot01"].normalizedTime>0.95f)
        {
            playAnim = PlayRun;
        }
    }
    public void SendPlayerMessage()
    {
        playAnim = PlayShoot;
    }
    public void AnimManager(INPUTDIRECTION inputDir)
    {
        switch (inputDir)
        {
            case INPUTDIRECTION.NULL:
                break;
            case INPUTDIRECTION.LEFT:
                playAnim = PlayLeft;
                break;
            case INPUTDIRECTION.RIGHT:
                playAnim = PlayRight;
                break;
            case INPUTDIRECTION.UP:
                playAnim = PlayJump;
                break;
            case INPUTDIRECTION.DOWN:
                playAnim = PlayRoll;
                break;
            default:
                break;
        }
    }

    public override void HandleEvent(string name, object data)
    {

    }
}
