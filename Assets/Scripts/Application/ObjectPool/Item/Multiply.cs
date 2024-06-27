using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiply : Item
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
        //“Ù–ß
        Game.Instance.soundManager.PlayEffect("Se_UI_JinBi");
        //ªÿ ’
        Game.Instance.objectPool.UnSpawn(gameObject);
        //Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.player))
        {
            HitPlayer(other.transform);
            //other.SendMessage("HitMultiply", SendMessageOptions.RequireReceiver);
            other.SendMessage("HitItem",ITEMKIND.ITEMMULTIPLY);
        }
    }
}
