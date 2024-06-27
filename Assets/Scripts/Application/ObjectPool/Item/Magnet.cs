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
        //音效
        Game.Instance.soundManager.PlayEffect("Se_UI_Magnet");
        //回收
        Game.Instance.objectPool.UnSpawn(gameObject);
        //Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.player))
        {
            Debug.Log("碰到吸铁石了");
            HitPlayer(other.transform);
            //other.SendMessage("HitMagnet", SendMessageOptions.RequireReceiver);//开启吸铁石的检测范围
            other.SendMessage("HitItem",ITEMKIND.ITEMMAGNET );
        }
    }
}
