using SUProtocol;
using System;
using System.Collections.Generic;
using UnityEngine;

//处理账号系统的登入

public class AcctSys : ILogic
{
    SURoot root;
    NetSvc netSvc;
    EvtSvc evtSvc;
    UISvc uiSvc;

    List<RoleData> roleDatas;
    public List<RoleData> RoleDatas
    {//越来越简单了
        get => roleDatas; 
        set {
            roleDatas = value;
            //触发委托，更新存储的队列存档，每一次的数据更新都会触发委托调用，该委托在createwindow的init里已经绑定好更新事件了
            evtSvc.SendEvt(EvtID.OnRoleDataUpdated, roleDatas);
        }
    
    }
    RoleData selfRoleData;
    public RoleData SelfRoleData { get => selfRoleData; }
    public string account;
    public int selectUid;
    public string battleIP;
    public int battlePort;
    public string battleToken;
    
    public void Init()
    {
        root = SURoot.Instance;
        netSvc = root.netSvc;
        evtSvc = root.evtSvc;
        uiSvc = root.uiSvc;

        System.Random rd=new System.Random();
        account = $"{rd.Next(1000, 9999)}";
        netSvc.RegistHandler(CMD.NtfRoleDatas, NtfRoleDatas);

        this.Log("Init AcctSys Done.");
    }

    public void Tick()
    {
        
    }

    public void UnInit()
    {
        this.Log("UnInit AcctSys.");
    }

    //在这里更新账号数据
    private void NtfRoleDatas(NetMsg msg)
    {
        if ((msg.errorCode == ErrorCode.None))
        {
            //在这里赋值，赋值会触发回调
            RoleDatas = msg.ntfRoleDatas.roleDatas;
        }
        else
        {
            this.Error($"Error:{msg.errorCode}");
        }
    }
    public void SetSelfRoleData()
    {
        for (int i = 0; i < roleDatas.Count; i++)
        {
            if (roleDatas[i].uid == selectUid)
            {
                selfRoleData = roleDatas[i];
                break;
            }
        }
    }
}
