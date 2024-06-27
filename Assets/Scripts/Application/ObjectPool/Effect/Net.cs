using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : Effect
{
    public override void OnSpawn()
    {
        transform.localPosition = new Vector3(0.22f,0,-1.83f);
        transform.localScale = Vector3.one * 1.66f;
        base.OnSpawn();
    }
    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
    }
}
