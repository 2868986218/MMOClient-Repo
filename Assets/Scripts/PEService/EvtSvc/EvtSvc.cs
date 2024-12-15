using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//事件处理回调类

public enum EvtID
{
    None,
    OnRoleDataUpdated,
    OnTaskInfoUpdated,
    OnSelfPlayerCreated,

    OnStageLoaded,
}

public class EvtSvc : ILogic
{

    readonly PEMsger<EvtID> msger = new();

    public void Init()
    {
        msger.MsgerInit();
        this.Log("Init EvtSvc Done.");
    }

    public void Tick()
    {
        msger.MsgerTick();
    }

    public void UnInit()
    {
        msger.MsgerUnInit();
    }

    //为对应枚举绑定处理委托
    public void AddListener(EvtID evt,Action<object , object> cb)
    {
        msger.AddMsgHandler(evt, cb);
    }

    public void RemoveListener(EvtID evt)
    {
        msger.RmvHandlerByMsgID(evt);
    }

    public void RemoveTargetListener(object target)
    {
        msger.RmvHandlerByTarget(target);
    }

    public void SendEvt(EvtID evt,object parameter1= null, object parameter2 = null)
    {
        //在这里更新角色存档列表
        msger.InvokeMsgHandler(evt, parameter1, parameter2);
    }

    public void SendEvtImmediately(EvtID evt,object parameter1, object parameter2)
    {
        msger.InvokeMsgHandlerImmediately(evt, parameter1, parameter2);
    }
}

