

namespace SUNet
{
    using System.Collections.Generic;
    public class IOCPTokenPool<T, K>
        where T : IOCPToken<K>, new()
        where K : new() {
        readonly Stack<T> stk;
        public int Size => stk.Count;
        public IOCPTokenPool(int capacity) {
            stk = new Stack<T>(capacity);
        }

        public T Pop() {
            lock(stk) {
                return stk.Pop();
            }
        }
        public void Push(T token) {
            if(token == null) {
                IOCPTool.Error("push token to pool cannot be null");
                return;
            }
            lock(stk) {
                stk.Push(token);
            }
        }
    }
}