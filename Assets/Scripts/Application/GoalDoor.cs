using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.ball))//�����ť����֮����ײ������
        {
            //����ҷ�����Ϣ
            other.SendMessageUpwards("HitGoalDoor",SendMessageOptions.RequireReceiver);
            //���Լ�������Ϣ��������Ա�������� Ȼ����ʧ
            gameObject.SendMessageUpwards("ShootAGoal",(int)other.transform.position.x);

        }
    }
}
