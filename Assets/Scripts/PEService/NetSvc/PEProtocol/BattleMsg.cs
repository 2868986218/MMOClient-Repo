
namespace SUProtocol {
    public class ReqTokenAccess {
        public int uid { get; set; }
        public string token { get; set; }
    }

    public partial class NetMsg {
        public ReqTokenAccess reqTokenAccess { get; set; }
    }
}