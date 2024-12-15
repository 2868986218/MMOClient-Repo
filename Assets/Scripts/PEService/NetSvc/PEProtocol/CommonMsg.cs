

using System.Collections.Generic;

namespace SUProtocol {
    public class AOIItem {
        public List<EnterUnit> enters { get; set; }
        public List<int> exits { get; set; }
    }

    public class EnterUnit {
        public int uid { get; set; }

        //base
        public int unitID { get; set; }
        public int level { get; set; }

        //move
        public float posX { get; set; }
        public float posZ { get; set; }
        public float dirX { get; set; }
        public float dirZ { get; set; }
        public float moveSpeed { get; set; }

        //attrs TODO

        public override string ToString() { return $"uid:{uid} unitID:{unitID}"; }
    }
}
