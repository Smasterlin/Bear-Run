using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : Item
{
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
        //��Ч
        Game.Instance.soundManager.PlayEffect("Se_UI_Magnet");
        //����
        Game.Instance.objectPool.UnSpawn(gameObject);
        //Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.player))
        {
            Debug.Log("��������ʯ��");
            HitPlayer(other.transform);
            //other.SendMessage("HitMagnet", SendMessageOptions.RequireReceiver);//��������ʯ�ļ�ⷶΧ
            other.SendMessage("HitItem",ITEMKIND.ITEMMAGNET );
        }
    }
}
