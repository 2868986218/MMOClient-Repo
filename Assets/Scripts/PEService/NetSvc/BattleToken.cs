/*-------------------------------------------------------------------------
* 命名空间名称/文件名:    Assets.Scripts.PEService.NetSvc/BattleToken 
* 功 能：       战斗进程会话
* 类 名：       BattleToken
* 创建时间：  2024/7/3 18:16:36
* 创建人:        Meibiyaluokenai
*-------------------------------------------------------------------------*/

using SUProtocol;
using SUNet;

public class BattleToken : IOCPToken<NetMsg>
{
    protected override void OnConnected(bool result)
    {
        if (result)
        {
            SURoot.Instance.netSvc.AddMsgPacks(new NetMsg(CMD.OnClient2BattleConnected));
        }
        else
        {
            this.LogRed("connect battle failed.");
        }
    }
    protected override void OnDisConnected()
    {
        SURoot.Instance.netSvc.AddMsgPacks(new NetMsg(CMD.OnClient2BattleDisConnected));
    }
    protected override void OnReceiveMsg(NetMsg msg)
    {
        SURoot.Instance.netSvc.AddMsgPacks(msg);
    }
}






