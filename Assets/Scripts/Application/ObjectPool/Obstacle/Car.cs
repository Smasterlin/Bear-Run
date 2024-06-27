using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : Obstacle
{
    [SerializeField] bool canMove;
    bool isBlock = false;
    float speed = 30;
    GameModel gm;
    protected override void Awake()
    {
        base.Awake();
        gm = MVC.GetModel<GameModel>();
    }
    public override void HitPlayer(Vector3 pos)
    {
        base.HitPlayer(pos);
    }
    public void HitTrigger()
    {
        isBlock = true;
        Debug.Log("碰到汽车触发器了");
    }
    private void Update()
    {
        if (isBlock == true && canMove == true && !gm.IsPause && gm.IsPlay)
        {
            transform.Translate(-transform.forward * speed * Time.deltaTime);
        }
    }
    public override void OnSpawn()
    {
        base.OnSpawn();
    }
    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        isBlock = false;
    }
}
