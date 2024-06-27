using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Controller
{
    public abstract void Excute(object data);

    //获取模型
    protected T GetModel<T>()
        where T:Model
    {
        return MVC.GetModel<T>();
    }
    //获取视图
    protected T GetView<T>()
        where T:View
    {
        return MVC.GetView<T>();
    }
    #region 注册
    //注册View
    protected void RegisterView(View view)
    {
        MVC.RegisterView(view);
    }
    //注册Model
    protected void RegisterModel(Model model)
    {
        MVC.RegisterModel(model);
    }
    //注册Controller
    protected void RegisterController(string eventName, Type controller)
    {
        MVC.RegisterController(eventName, controller);
    } 
    #endregion
}
