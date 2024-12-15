using SUProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateWindow : WindowRoot {
    private AcctSys acctSys;

    public Transform itemRoot;

    //当前被选中的角色
    private int curSelectRoleIndex = 0;
    

    int selectIndex = 0;
    List<RoleData> roleDatas;

    //最大存储角色数量
    const int MAX_ROLES = 3;

    protected override void InitWnd()
    {
        base.InitWnd();

        acctSys = root.acctSys;
        //这里是长久的更新
        root.evtSvc.AddListener(EvtID.OnRoleDataUpdated, OnRoleDataUpadated);

        roleDatas = acctSys.RoleDatas;

        //这里是第一次更新
        OnRoleDataUpadated(roleDatas,null);
    }

    //查找根节点的子物体，roleData代表createWindow下的每个存档信息，itemRoot即所有存档的根节点
    private void OnRoleDataUpadated(object a,object b)
    {
        roleDatas=(List<RoleData>)a;
        for(int i=0;i<MAX_ROLES;i++)
        {
            Transform item = GetTrans(itemRoot, $"Item_{i}");
            if(roleDatas!=null&&i<roleDatas.Count)
            {
                SetActive(item,true);
                SetText(GetText(item, "bgName/name"), roleDatas[i].nickName);
            }
            else
            {
                SetActive(item,false);
            }
        }

        if(roleDatas!=null && roleDatas.Count > 0)
        {
            
            ClickRoleItem(curSelectRoleIndex);
            
        }
    }

    //why public
    public void ClickRoleItem(int index)
    {

        //这里做一个优化，空间换时间，老师写比较low真的
        
        //取消选中之前的，选中现在的
        
        if(curSelectRoleIndex == index)
        {
            return;
        }
        
        Transform item = GetTrans(itemRoot, $"Item_{curSelectRoleIndex}");
        SetActive(GetTrans(item, $"bg_selected"),false);

        item = GetTrans(itemRoot, $"Item_{index}");
        SetActive(GetTrans(item, $"bg_selected"), true);

        curSelectRoleIndex= index;


    }


    //发送给服务器请求登入该角色
    public void ClickEnterBtn()
    {
        audioSvc.PlayUIAudio("loginBtnClick");

        if(selectIndex == -1)
        {
            this.Error("无角色数据");
            return;
        }

        acctSys.selectUid = roleDatas[selectIndex].uid;

        NetMsg req = new()
        {
            cmd = CMD.ReqRoleToken,
            reqRoleToken = new ReqRoleToken
            {
                acct = root.acctSys.account,
                selectUid = acctSys.selectUid
            }
        };
        //选择角色，点击登入，需要的回调是，存储uid ip port roletoken 开启战斗服的连接
        root.netSvc.SendMsg(req, (rsp) => {
            if (rsp.errorCode == ErrorCode.None)
            {
                RspRoleToken rrt = rsp.rspRoleToken;
                acctSys.battleIP = rrt.battleIP;
                acctSys.battlePort = rrt.battlePort;
                acctSys.battleToken = rrt.token;
                netSvc.StartConnectToBattle();
            }
            else
            {
                this.Error($"error code:{rsp.errorCode}");
            }
        });
    }
}

