/*************************************************
作者: Plane
邮箱: 1785275942@qq.com
官网: www.qiqiker.com
微信: PlaneZhong
功能: 推送状态数据
*************************************************/

using System.Collections.Generic;

namespace SUProtocol {
    public partial class NetMsg {
        public PshStateData pshStateData { get; set; }
    }

    public class PshStateData {
        public List<StateItem> stateItems { get; set; }
    }

    public class StateItem {
        public int uid { get; set; }
        public PushType pushType { get; set; }
        public AOIItem aoiItem { get; set; }
        public MoveItem moveItem { get; set; }
    }

    public enum PushType {
        None,
        AOI,
        Move,
        //TODO
    }
}
