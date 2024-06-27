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
        //������Ч
        GameObject go= Game.Instance.objectPool.Spawn("FX_JinBi", effectParent);
        go.transform.position = pos.position;
        //��Ч
        Game.Instance.soundManager.PlayEffect("Se_UI_JinBi");
        //����
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
        else if (other.CompareTag(Tag.magnetCollider))//��������ʯ�ļ�ⷶΧ
        {
            Debug.Log("��ұ���⵽��");
            //��ҷ������
            StartCoroutine(HitMagnet(other.transform));
        }
    }
    IEnumerator HitMagnet(Transform pos)
    {
        bool isLoop = true;
        while (isLoop)
        {
            Debug.Log("��ҷ��������");
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
