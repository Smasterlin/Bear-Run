using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    Transform playerTrans;
    Vector3 offset;
    float speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        playerTrans = GameObject.FindWithTag(Tag.player).transform;
        offset = transform.position - playerTrans.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position,playerTrans.position+offset,speed*Time.deltaTime);
    }
}
