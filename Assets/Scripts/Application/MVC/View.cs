using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class View : MonoBehaviour
{
    public abstract string Name { get; }
    [HideInInspector]
    public List<string> AttentionList = new();

    public virtual void RegisterAttentionEvent()
    {

    }
    public abstract void HandleEvent(string name,object data);


    #region MVC框架 发送事件消息，访问模型
    //发送事件
    protected void SendEvent(string eventName, object data = null)
    {
        MVC.SendEvent(eventName, data);
    }
    //获取模型
    protected T GetModel<T>()
        where T : Model
    {
        return MVC.GetModel<T>();
    } 
    #endregion
}
