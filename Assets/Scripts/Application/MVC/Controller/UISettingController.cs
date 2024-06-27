using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UISettingController : Controller
{
    public override void Excute(object data)
    {
        UISetting set = GetView<UISetting>();
        set.Show();
    }
}
