using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.ball))//点击按钮射球之后，球撞到球门
        {
            //给玩家发送消息
            other.SendMessageUpwards("HitGoalDoor",SendMessageOptions.RequireReceiver);
            //给自己发送消息，让守门员动画播放 然后消失
            gameObject.SendMessageUpwards("ShootAGoal",(int)other.transform.position.x);

        }
    }
}
