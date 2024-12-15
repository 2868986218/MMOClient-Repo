/*************************************************
作者: Plane
邮箱: 1785275942@qq.com
官网: www.qiqiker.com
微信: PlaneZhong
功能: 移动同步相关上行消息
*************************************************/

namespace SUProtocol {
    public partial class NetMsg {
        public SynMovePos synMovePos { get; set; }
    }

    public class SynMovePos {
        public MoveItem moveItem { get; set; }
    }

    public class MoveItem {
        public float posX { get; set; }
        public float posZ { get; set; }
        public float dirX { get; set; }
        public float dirZ { get; set; }
        public float moveSpeed { get; set; }
    }
}
