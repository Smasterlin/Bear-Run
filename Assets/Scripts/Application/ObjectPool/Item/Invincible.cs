using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincible : Item
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
        //��Ч
        Game.Instance.soundManager.PlayEffect("Se_UI_Whist");

        //����
        Game.Instance.objectPool.UnSpawn(gameObject);
        //Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.player))
        {
            HitPlayer(other.transform);
            //other.SendMessage("HitInvincible",SendMessageOptions.RequireReceiver);
            other.SendMessage("HitItem",ITEMKIND.ITEMINVINCIBLE);
        }
    }
}
