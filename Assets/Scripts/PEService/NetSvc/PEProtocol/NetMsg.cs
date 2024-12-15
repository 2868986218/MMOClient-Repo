/*************************************************
作者: Plane
邮箱: 1785275942@qq.com
功能: 网络通信协议数据类

           ~~获取更多教学资讯~~
//----------------*----------------\\
        教学官网：www.qiqiker.com
        官方微信服务号: qiqikertuts
        Plane老师微信: PlaneZhong
\\----------------*----------------//
*************************************************/

namespace SUProtocol {
    public partial class NetMsg {
        public CMD cmd { get; set; }
        public ErrorCode errorCode { get; set; }
        public NetMsg() { }
        public NetMsg(CMD cmd, ErrorCode errorCode = ErrorCode.None) {
            this.cmd = cmd;
            this.errorCode = errorCode;
        }

        public NtfEnterStage ntfEnterStage { get; set; }
        public FinEnterStage finEnterStage { get; set; }
    }

    public enum EnterStageMode {
        Login,
        Teleport,
        Reconnect
    }
    //通知客户端我准备进入关卡了
    public class NtfEnterStage {
        public EnterStageMode mode { get; set; }
        public int prefixID { get; set; }
        public int stageID { get; set; }
    }
    //确认进入关卡
    public class FinEnterStage {
        public EnterStageMode mode { get; set; }
        public int prefixID { get; set; }
        public int stageID { get; set; }
    }
}