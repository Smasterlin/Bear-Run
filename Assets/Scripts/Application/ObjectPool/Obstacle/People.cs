using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class People : Obstacle
{
    Animation anim;
    public bool isHit;
    bool isFly;
    float speed = 10;
    GameModel gm;
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponentInChildren<Animation>();
        gm = MVC.GetModel<GameModel>();
    }
    private void Update()
    {
        if (isHit == true && gm.IsPlay && !gm.IsPause)
        {
            transform.position -= new Vector3(speed, 0, 0) * Time.deltaTime;
        }
        if (isFly == true && gm.IsPlay && !gm.IsPause)
        {
            transform.position += new Vector3(0, speed, speed) * Time.deltaTime;
            anim.Play("fly");
        }
    }
    public void HitTrigger()
    {
        isHit = true;
    }
    public override void OnSpawn()
    {
        base.OnSpawn();
        anim.Play("run");
    }
    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        isFly = false;
        isHit = false;
        anim.transform.position = Vector3.zero;
    }
    public override void HitPlayer(Vector3 pos)
    {
        GameObject go = Game.Instance.objectPool.Spawn("FX_ZhuangJi", effectParentTrans);
        go.transform.position = pos;
        isHit = false;
        isFly = true;
    }
}
