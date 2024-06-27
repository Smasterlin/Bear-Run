using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : ReusableObject
{
    [SerializeField]float time = 1;
    public override void OnSpawn()
    {
        StartCoroutine(DestroyCoroutine());
    }

    public override void OnUnSpawn()
    {
        StopAllCoroutines();
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(time);
        Game.Instance.objectPool.UnSpawn(gameObject);
    }
}
