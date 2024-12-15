/*************************************************
作者: Plane
邮箱: 1785275942@qq.com
功能: 登录相关协议

           ~~获取更多教学资讯~~
//----------------*----------------\\
        教学官网：www.qiqiker.com
        官方微信服务号: qiqikertuts
        Plane老师微信: PlaneZhong
\\----------------*----------------//
*************************************************/

using System.Collections.Generic;

namespace SUProtocol {
    public class ReqAcctLogin {
        public string acct { get; set; }
        public string pass { get; set; }
        public int dataID { get; set; }
    }

    public class NtfRoleDatas {
        public List<RoleData> roleDatas { get; set; }
    }
    public class RoleData {
        public int uid { get; set; }
        public string nickName { get; set; }

        public float posX { get; set; }
        public float posZ { get; set; }
        public float dirX { get; set; }
        public float dirZ { get; set; }

        public ulong lastRid { get; set; }
        public ulong lastTid { get; set; }

        public int unitID { get; set; }
        public int level { get; set; }
        public int exp { get; set; }

        //TODO 业务数据

        public override string ToString() {
            return $"  uid:{uid}\t NickName:{nickName}\t level:{level}\t exp:{exp}";
        }
    }

    public class ReqRoleToken {
        public string acct { get; set; }
        public int selectUid { get; set; }
    }
    public class RspRoleToken {
        public string battleIP { get; set; }
        public int battlePort { get; set; }
        public string token { get; set; }
    }

    public partial class NetMsg {
        public ReqRoleToken reqRoleToken { get; set; }
        public RspRoleToken rspRoleToken { get; set; }
        public NtfRoleDatas ntfRoleDatas { get; set; }
        public ReqAcctLogin reqAcctLogin { get; set; }//common rsp
    }
}