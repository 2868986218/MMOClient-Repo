using SUProtocol;
using PEUtils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SUNet;
/*
 * 所有网络通讯相关服务管理
 */

public class NetSvc : ILogic
{
    SURoot root;
    UISvc uiSvc;

    IOCPNet<LoginToken, NetMsg> loginNet;
    IOCPNet<BattleToken,NetMsg> battleNet;
    readonly ConcurrentQueue<NetMsg> netMsgs = new();
    readonly Dictionary<CMD, Action<NetMsg>> ntfHandlers = new();
    readonly Dictionary<CMD, Action<NetMsg>> rspHandlers = new();


    public void Init()
    {
        root = SURoot.Instance;
        uiSvc = root.uiSvc;

        IOCPTool.LogFunc = PELog.Log; IOCPTool.WarnFunc = PELog.Warn; IOCPTool.ErrorFunc = PELog.Error;
        IOCPTool.ColorLogFunc = (color, msg) => { PELog.ColorLog((LogColor)color, msg); };
        RegistHandler(CMD.OnClient2LoginConnected, OnClient2LoginConnected);
        RegistHandler(CMD.OnClient2LoginDisConnected, OnClient2LoginDisConnected);
        RegistHandler(CMD.OnClient2BattleConnected, OnClient2BattleConnected);
        RegistHandler(CMD.OnClient2BattleDisConnected, OnClient2BattleDisConnected);

        this.Log("NetSvc 初始化成功.");
    }

    public void Tick()
    {
        while (!netMsgs.IsEmpty)
        {
            if(netMsgs.TryDequeue(out NetMsg msg))
            {
                this.Log($"客户端收到消息：[Token] -> CMD:{msg.cmd}");
                if(rspHandlers.TryGetValue(msg.cmd, out Action<NetMsg> rspHandler))
                {
                    rspHandler.Invoke(msg);
                    rspHandlers.Remove(msg.cmd);

                }
                else
                {
                    //发送消息，回调都是一样的，可以不remove
                    if(ntfHandlers.TryGetValue(msg.cmd, out Action<NetMsg> ntfHandler))
                    {
                        ntfHandler.Invoke(msg);
                        //ntfHandlers.Remove(msg.cmd);
                    }
                }
            }
            

        }
    }

    public void UnInit()
    {
        this.Log("UnInit NetSvc");
    }

    //连接服务器
    public void StartConnectToLogin()
    {
        loginNet = new IOCPNet<LoginToken, NetMsg>();
        loginNet.StartAsClient("192.168.136.1", 11451);//登入进程端口号

    }
    public void StartConnectToBattle()
    {
        battleNet = new IOCPNet<BattleToken, NetMsg>();
        battleNet.StartAsClient(root.acctSys.battleIP, root.acctSys.battlePort);
    }

    //主动断开进程连接
    public void ActiveCloseLoginConnection()
    {
        loginNet.ClosetClient();
    }
    public void ActiveCloseBattleConnection()
    {
        battleNet.ClosetClient();
    }


    public void RegistHandler(CMD cmd,Action<NetMsg> handler)
    {
        if(!ntfHandlers.ContainsKey(cmd))
        {
            ntfHandlers.Add(cmd, handler);
        }
    }

    //发送消息，对应收到回应执行的回调
    public void SendMsg(NetMsg msg, Action<NetMsg> cb = null)
    {
        if (cb != null)
        {
            //判断此request消息回应respond是否已经存在   +1是约定俗成的，可以在枚举中发现规律
            CMD cmdMsg = msg.cmd + 1;
            if (!rspHandlers.ContainsKey(cmdMsg))
            {
                //回应，执行的回调
                rspHandlers.Add(cmdMsg, cb);
            }
        }

        switch (msg.cmd)
        {
            case CMD.ReqRoleToken:
            case CMD.ReqAcctLogin:
                loginNet?.token?.SendMsg(msg); break;
            default:
                battleNet?.token?.SendMsg(msg);
                break;
        }
    }

    void OnClient2LoginConnected(NetMsg msg)
    {
        this.LogGreen("Connected to login.");
    }
    void OnClient2LoginDisConnected(NetMsg msg)
    {
        this.LogYellow("DisConnected to login.");
    }
    void OnClient2BattleConnected(NetMsg msg)
    {
        this.LogGreen("战斗进程连接成功");
        NetMsg req = new NetMsg()
        {
            cmd = CMD.ReqTokenAccess,
            reqTokenAccess = new ReqTokenAccess
            {
                uid = root.acctSys.selectUid,
                token = root.acctSys.battleToken
            }
        };

        SendMsg(req, (rsp) => {
            switch (rsp.errorCode)
            {
                case ErrorCode.None:
                    root.acctSys.SetSelfRoleData();
                    uiSvc.ShowTips("登录成功");
                    break;
                case ErrorCode.token_already_online:
                case ErrorCode.token_error:
                case ErrorCode.token_not_exist:
                case ErrorCode.token_expired:
                    uiSvc.ShowTips($"Token异常：{rsp.errorCode}");
                    break;
            }
        });

    }
    void OnClient2BattleDisConnected(NetMsg msg)
    {
        this.LogYellow("DisConnected to battle.");
    }
    public void AddMsgPacks(NetMsg msg)
    {
        netMsgs.Enqueue(msg);
    }

    //-------------Tool Functions-------------//
    public bool IsConnectedLogin()
    {
        if (loginNet != null && loginNet.token != null)
        {
            return loginNet.token.IsConnected;
        }
        else
        {
            return false;
        }
    }
    public bool IsConnectedBattle()
    {
        if (battleNet != null && battleNet.token != null)
        {
            return battleNet.token.IsConnected;
        }
        else
        {
            return false;
        }
    }
}

