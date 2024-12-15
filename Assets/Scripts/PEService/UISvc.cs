using SUProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*UI管理*/
public class UISvc :ILogic
{
    SURoot root;

    //每个界面都需要用到的UI
    LoadingWindow loadingWindow;
    TipsWindow tipsWindow;
    //所有ui脚本的引用
    readonly Dictionary<UIWindowEnum,WindowRoot>windowDic=new Dictionary<UIWindowEnum,WindowRoot>();
    public void Init()
    {
        root = SURoot.Instance;

        //top
        loadingWindow=root.transform.Find("Canvas/LoadingWindow").GetComponent<LoadingWindow>();
        windowDic.Add(UIWindowEnum.Loading, loadingWindow);
        tipsWindow = root.transform.Find("Canvas/TipsWindow").GetComponent<TipsWindow>();
        windowDic.Add(UIWindowEnum.Tips, tipsWindow);

        //base
        LoginWindow loginWindow = root.transform.Find("Canvas/LoginWindow").GetComponent<LoginWindow>();
        windowDic.Add(UIWindowEnum.Login, loginWindow);
        CreateWindow createWindow = root.transform.Find("Canvas/CreateWindow").GetComponent<CreateWindow>();
        windowDic.Add(UIWindowEnum.Create, createWindow);
        MajorWindow majorWindow = root.transform.Find("Canvas/MajorWindow").GetComponent<MajorWindow>();
        windowDic.Add(UIWindowEnum.Major, majorWindow);
        SingleFBWindow singleFBWindow = root.transform.Find("Canvas/SingleFBWindow").GetComponent<SingleFBWindow>();
        windowDic.Add(UIWindowEnum.SingleFuben, singleFBWindow);

        SetWindowState(UIWindowEnum.Tips,true);
        this.Log("Init UISvc Done.");
    }

    public void Tick()
    {

    }

    public void UnInit()
    {
        this.Log("UnInit UISvc");
    }

    //设置加载进度窗口
    public void SetLoading(int pct,bool state = true)
    {
        //TODO
        loadingWindow.SetProgress(pct,state);
    }

    //此方法是将设置UIWindow是否显示
    public void SetWindowState(UIWindowEnum uIWindowEnum,bool state = true)
    {
        if(windowDic.TryGetValue(uIWindowEnum,out WindowRoot windowRoot))
        {
            windowRoot.SetWindowState(state);
        }
    }

    //切换到目标游戏模式的基础ui,关闭释放其他模式所有ui
    //更新到对应模式
    public void SwitchStageBaseUI(CPlayMode modeEnum)
    {
        UIWindowEnum uiWindow = UIWindowEnum.None;

        switch (modeEnum)
        {
            case CPlayMode.Login:
                uiWindow = UIWindowEnum.Login;
                break;
            case CPlayMode.Create:
                uiWindow = UIWindowEnum.Create;
                break;
            case CPlayMode.Major:
                uiWindow = UIWindowEnum.Major;
                break;
            case CPlayMode.Wild:
                uiWindow = UIWindowEnum.Wild;
                break;
            case CPlayMode.Single:
                uiWindow = UIWindowEnum.SingleFuben;
                break;
            case CPlayMode.Multiple:
                uiWindow = UIWindowEnum.MultiFuben;
                break;
        }
        
        //为什么要遍历所有的WindowUI？？？
        //将上一次的隐藏，这一次的打开不就行了吗？
        //原因是我并没有对当前当前模式的引用
        foreach (var item in windowDic)
        {
            WindowRoot windowRoot=item.Value;
            //基础层ui说明要切换场景
            //弹窗层ui即需要unInit
            if(windowRoot.windowLayer==WindowLayer.BaseUI||windowRoot.windowLayer==WindowLayer.PopUI)
            {
                if (item.Key == uiWindow)
                {
                    windowRoot.SetWindowState(true);
                }
                else
                {
                    windowRoot.SetWindowState(false);
                }

            }
            
        }

        


    }

    //tip弹窗，高层级
    public void ShowTips(string tips)
    {
        tipsWindow.AddTips(tips);
    }
}
