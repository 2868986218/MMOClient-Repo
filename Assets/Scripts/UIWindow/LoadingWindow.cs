
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
//关卡切换进度显示窗口

public class LoadingWindow:WindowRoot
{
    public Image imgProgress;
    public Text txtProgress;

    protected override void InitWnd()
    {
        base.InitWnd();

        imgProgress.fillAmount = 0;
        txtProgress.text = "0%";
    }

    protected override void UnInitWnd()
    {
        base.UnInitWnd();
        imgProgress.fillAmount = 0;
        txtProgress.text = "0%";
    }

    public void SetProgress(int pct,bool state = true)
    {
        SetWindowState(state);
        imgProgress.fillAmount = pct * 1.0f / 100;
        txtProgress.text = pct.ToString()+"%";
    }
}

