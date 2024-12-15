using SUProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

//登入验证
public class LoginWindow:WindowRoot
{
    public InputField inputAcct;
    public Text zoneName;
    public Transform zoneRoot;

    protected override void InitWnd()
    {
        base.InitWnd();

        //激活位置，显示
        SetActive(zoneRoot, true);
        zoneName.text = "Game1Server";
        inputAcct.text = root.acctSys.account;
    }

    public void ClickChangeZone()
    {
        SetActive(zoneRoot, true);
    }
    public void ClickCloseBtn()
    {
        SetActive(zoneRoot, false);
    }

    public void ClickLoginBtn()
    {
        audioSvc.PlayUIAudio("loginBtnClick");

        if(string.IsNullOrEmpty(inputAcct.text))
        {
            uiSvc.ShowTips("输入数据不合法");
        }
        else
        {
            root.acctSys.account = inputAcct.text;
            //这样写是不是为了给属性赋值？ {} 属性初始化列表
            NetMsg req = new NetMsg() {
                cmd = CMD.ReqAcctLogin,
                reqAcctLogin = new ReqAcctLogin {
                    acct =inputAcct.text,
                    pass = inputAcct.text,
                    dataID = 1979
                }
            };



            root.netSvc.SendMsg(req, (rsp) => {
                switch (rsp.errorCode)
                {
                    case ErrorCode.None:
                        uiSvc.ShowTips($"登录成功");
                        break;
                    case ErrorCode.acct_online_login:
                        uiSvc.ShowTips($"账号已登录Login。");
                        break;
                    case ErrorCode.acct_online_data:
                        uiSvc.ShowTips($"账号已登录Data");
                        break;
                    case ErrorCode.acct_l2d_offline:
                        uiSvc.ShowTips($"当前区服离线，请选择其大区。");
                        break;
                    default:
                        this.Error($"errorCode:{rsp.errorCode} 未处理。");
                        break;
                }
            });

        }
    }

}

