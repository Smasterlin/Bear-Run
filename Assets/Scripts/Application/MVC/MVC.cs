using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class MVC
{
    public static Dictionary<string, Model> Models = new();
    public static Dictionary<string, View> Views = new();
    public static Dictionary<string, Type> CommandMap = new();


    #region 注册
    //注册View
    public static void RegisterView(View view)
    {
        if (Views.ContainsKey(view.Name))
        {
            Views.Remove(view.Name);
        }
        view.RegisterAttentionEvent();
        Views[view.Name] = view;
    }
    //注册Model
    public static void RegisterModel(Model model)
    {
        Models[model.Name] = model;
    }
    //注册Controller
    public static void RegisterController(string eventName, Type controllerType)
    {
        CommandMap[eventName] = controllerType;
    }
    #endregion

    #region 获得视图和数据模型
    //获得Model
    public static T GetModel<T>()
 where T : Model
    {
        foreach (Model m in Models.Values)
        {
            if (m is T)
            {
                return (T)m;
            }
        }
        return null;
    }
    //获得View
    public static T GetView<T>()
        where T : View
    {
        foreach (var v in Views.Values)
        {
            if (v is T)
            {
                return (T)v;
            }
        }
        return null;
    }

    #endregion
    /// <summary>
    /// 发送事件消息
    /// </summary>
    public static void SendEvent(string eventName,object data=null)
    {
        //如果有收到事件消息的话，就先传给controller
        if (CommandMap.ContainsKey(eventName))
        {
            Type t = CommandMap[eventName];
            Controller c = Activator.CreateInstance(t) as Controller;//控制器生成
            c.Excute(data);
        }

        foreach (var v in Views.Values)
        {
            if (v.AttentionList.Contains(eventName))
            {
                v.HandleEvent(eventName,data);
            }
        }
    }


}
