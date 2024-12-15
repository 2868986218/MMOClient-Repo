using SUProtocol;

//登入验证模式
public class LoginMode : BaseMode
{
    //在此更新父类的游戏模式
    public LoginMode() : base(CPlayMode.Login)
    {
    }
    public override void Enter()
    {
        base.Enter();
        //连接login服务器
        root.netSvc.StartConnectToLogin();
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        
    }
}

