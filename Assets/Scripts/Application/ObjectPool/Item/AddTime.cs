using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTime : Item
{
    public override void OnSpawn()
    {
        base.OnSpawn();
    }
    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.player))
        {
            HitPlayer(other.transform);
            other.SendMessage("HitAddTime",SendMessageOptions.RequireReceiver);
        }
    }
    protected override void HitPlayer(Transform pos)
    {
        //“Ù–ß
        Game.Instance.soundManager.PlayEffect("Se_UI_Time");

        //ªÿ ’
        Game.Instance.objectPool.UnSpawn(gameObject);
        //Destroy(gameObject);
    }
}
