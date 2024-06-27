using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : ReusableObject
{
    protected Transform effectParentTrans;
    public override void OnSpawn()
    {

    }

    public override void OnUnSpawn()
    {

    }
    #region Unity回调
    protected virtual void Awake()
    {
        effectParentTrans = GameObject.Find("EffectParent").transform;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion
    public virtual void HitPlayer(Vector3 pos)//玩家撞到障碍物
    {
        GameObject go = Game.Instance.objectPool.Spawn("FX_ZhuangJi", effectParentTrans);
        go.transform.position = pos;

        Game.Instance.objectPool.UnSpawn(gameObject);
        //Destroy(gameObject);
    }

}
