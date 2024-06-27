using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootGoal : ReusableObject
{
    [SerializeField] Animation goalKeeper;
    [SerializeField] Animation door;
    [SerializeField] GameObject net;
    float speed = 10;
    bool m_isFly;
    public override void OnSpawn()
    {
        
    }

    public override void OnUnSpawn()
    {
        net.SetActive(true);
        door.Play("QiuMen_St");
        goalKeeper.Play("standard");
        goalKeeper.transform.parent.parent.gameObject.SetActive(true);      
        m_isFly = false;
        StopAllCoroutines();

        goalKeeper.transform.localPosition = Vector3.zero;
    }
    /// <summary>
    /// 球撞到球门之后，守门员消失
    /// </summary>
    public void ShootAGoal(int i)
    {
        switch (i)
        {
            case -2:
                goalKeeper.Play("right_flutter");
                break;
            case 0:
                goalKeeper.Play("flutter");
                break;
            case 2:
                goalKeeper.Play("left_flutter");
                break;
        }
        StartCoroutine(HideGoalKeeper());
    }
    IEnumerator HideGoalKeeper()
    {
        yield return new WaitForSeconds(1);
        goalKeeper.transform.parent.parent.gameObject.SetActive(false);
    }
    /// <summary>
    /// 玩家撞到守门员
    /// </summary>
    public void HitGoalKeeper()
    {
        Debug.Log("撞到守门员了" + m_isFly);
        m_isFly = true;
        goalKeeper.Play("fly");
    }
    private void Update()
    {
        if (m_isFly==true)
        {
            goalKeeper.gameObject.transform.position += new Vector3(0, speed, speed) * Time.deltaTime;
        }
    }
    /// <summary>
    /// 玩家撞到球门
    /// </summary>
    /// <param name="i"></param>
    public void HitDoor(int i)
    {
        //球门动画
        switch (i)
        {
            case 0:
                door.Play("QiuMen_RR");
                break;
            case 1:
                door.Play("QiuMen_St");
                break;
            case 2:
                door.Play("QiuMen_LR");
                break;
            default:
                break;
        }
        //球门的球网消失
        net.SetActive(false);
    }
}
