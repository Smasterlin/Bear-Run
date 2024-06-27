using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item
{
    Transform effectParent;
    float moveSpeed = 30;
    private void Awake()
    {
        effectParent = GameObject.Find("EffectParent").transform;
    }
    public override void OnSpawn()
    {
        base.OnSpawn();
    }
    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        
    }
    protected override void HitPlayer(Transform pos)
    {
        base.HitPlayer(pos);
        //产生特效
        GameObject go= Game.Instance.objectPool.Spawn("FX_JinBi", effectParent);
        go.transform.position = pos.position;
        //音效
        Game.Instance.soundManager.PlayEffect("Se_UI_JinBi");
        //回收
        Game.Instance.objectPool.UnSpawn(gameObject);
        //Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.player))
        {
            HitPlayer(other.transform);
            other.SendMessage("HitCoin",SendMessageOptions.RequireReceiver);
        }
        else if (other.CompareTag(Tag.magnetCollider))//碰到吸铁石的检测范围
        {
            Debug.Log("金币被检测到了");
            //金币飞向玩家
            StartCoroutine(HitMagnet(other.transform));
        }
    }
    IEnumerator HitMagnet(Transform pos)
    {
        bool isLoop = true;
        while (isLoop)
        {
            Debug.Log("金币飞向玩家了");
            transform.position= Vector3.Lerp(transform.position, pos.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, pos.position) < 0.5f)
            {
                isLoop = false;
                HitPlayer(pos);
                pos.SendMessageUpwards("HitCoin", SendMessageOptions.DontRequireReceiver);
            }
            yield return 0;
        }
    }
}
