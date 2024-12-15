/*-------------------------------------------------------------------------
* 命名空间名称/文件名:    Assets.Scripts.PEService.NetSvc/LoginToken 
* 功 能：      登录进程会话
* 类 名：       LoginToken
* 创建时间：  2024/7/3 18:16:28
* 创建人:        Meibiyaluokenai
*-------------------------------------------------------------------------*/



using SUNet;
using SUProtocol;

public class LoginToken : IOCPToken<NetMsg>
{
    protected override void OnConnected(bool result)
    {
        if (result)
        {
            SURoot.Instance.netSvc.AddMsgPacks(new NetMsg(CMD.OnClient2LoginConnected));
            this.LogRed("Login会话 连接成功");
        }
        else
        {
            this.LogRed("Login会话 连接失败");
        }
    }

    protected override void OnDisConnected()
    {
        SURoot.Instance.netSvc.AddMsgPacks(new NetMsg(CMD.OnClient2LoginDisConnected));
        this.LogRed("Login 会话断开连接.");

    }

    protected override void OnReceiveMsg(NetMsg msg)
    {
        //收到Login会话消息
        this.LogRed($"收到Login会话消息.{msg.cmd}");
        SURoot.Instance.netSvc.AddMsgPacks(msg);

    }
}


