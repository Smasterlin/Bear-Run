using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class ResumeController : Controller
{

    public override void Excute(object data)
    {
        UIPause pause = GetView<UIPause>();
        pause.Hide();

        UIResume resume = GetView<UIResume>();   
        resume.StartCount();
    }

 

  
}
