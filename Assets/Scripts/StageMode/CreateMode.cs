using SUProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CreateMode : BaseMode
{
    public CreateMode() : base(CPlayMode.Create)
    {
    }

    public override void Exit()
    {
        if (netSvc.IsConnectedLogin())
        {
            netSvc.ActiveCloseLoginConnection();
        }
    }

    public override void Update()
    {
        
    }
}
    
