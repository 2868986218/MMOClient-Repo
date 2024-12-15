/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: PE消息回调处理器

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

//这是老师写的类，msgQue是存储角色存档信息的

using System;
using System.Collections.Generic;

public class PEMsger<T> {
    private static readonly string m_lock = "PEMsger_Lock";
    private readonly Queue<MsgPararms> msgQue = new Queue<MsgPararms>();
    private readonly PEMsgMap<T> evtMap = new PEMsgMap<T>();

    public void MsgerInit() {
        msgQue.Clear();
    }
    public void MsgerTick() {
        lock(m_lock) {
            while(msgQue.Count > 0) {
                MsgPararms data = msgQue.Dequeue();
                TriggerMsgHandler(data.GetMsgID(), data.GetParam1(), data.GetParam2());
            }
        }
    }
    public void MsgerUnInit() { }

    public void AddMsgHandler(T evt, Action<object, object> cb) {
        lock(m_lock) {
            evtMap.AddMsgHandler(evt, cb);
        }
    }
    public void RmvHandlerByMsgID(T id) {
        lock(m_lock) {
            evtMap.RmvMsgHandler(id);
        }
    }
    public void RmvHandlerByTarget(object target) {
        lock(m_lock) {
            evtMap.RmvTargetHandler(target);
        }
    }

    public void InvokeMsgHandler(T evt, object param1 = null, object param2 = null) {
        lock(m_lock) {
            msgQue.Enqueue(new MsgPararms(evt, param1, param2));
        }
    }
    public void InvokeMsgHandlerImmediately(T evt, object param1 = null, object param2 = null) {
        TriggerMsgHandler(evt, param1, param2);
    }
    void TriggerMsgHandler(T t, object param1, object param2) {
        List<Action<object, object>> lst = evtMap.GetMsgAllHandler(t);
        if(lst != null) {
            for(int i = 0; i < lst.Count; i++) {
                lst[i](param1, param2);
            }
        }
    }

    class MsgPararms {
        T m_t = default(T);
        object m_param1 = null;
        object m_param2 = null;
        public MsgPararms(T t, object param1, object param2) {
            m_t = t;
            m_param1 = param1;
            m_param2 = param2;
        }

        public T GetMsgID() {
            return m_t;
        }
        public object GetParam1() { return m_param1; }
        public object GetParam2() { return m_param2; }
    }
}
