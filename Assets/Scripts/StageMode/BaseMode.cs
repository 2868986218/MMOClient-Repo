using SUProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




//游戏玩法模式基类
public abstract class BaseMode
{
    protected SURoot root;
    protected NetSvc netSvc;
    protected CPlayMode gameMode = CPlayMode.None;
    public BaseMode(CPlayMode modeEnum)
    {
        root = SURoot.Instance;
        netSvc = root.netSvc;
        gameMode = modeEnum;
    }
    //进入模式
    public virtual void Enter()
    {
        //在此根据不同的游戏模式选择UI显示
        root.uiSvc.SwitchStageBaseUI(gameMode);
        root.uiSvc.SetLoading(0, false);
    }

    //更新
    public abstract void Update();
    //退出
    public abstract void Exit();
}
