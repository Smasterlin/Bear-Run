using System;
using System.Collections.Generic;
using UnityEngine;
public abstract class Model
{
    public abstract string Name { get; }

    #region MVC框架  发送消息
    protected void SendEvent(string eventName, object data = null)
    {
        MVC.SendEvent(eventName, data);
    } 
    #endregion
}
