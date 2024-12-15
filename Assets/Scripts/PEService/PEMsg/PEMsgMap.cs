/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 消息ID与回调处理函数映射关系；目标实例与注册消息ID列表映射关系

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using System;
using System.Collections.Generic;

public class PEMsgMap<T> {
    Dictionary<T, List<Action<object, object>>> m_MsgHandlerDic = new Dictionary<T, List<Action<object, object>>>();
    Dictionary<object, List<T>> m_TargetMsgDic = new Dictionary<object, List<T>>();
    public void AddMsgHandler(T t, Action<object, object> cb) {
        //EvtMap
        if(!m_MsgHandlerDic.ContainsKey(t)) {
            m_MsgHandlerDic[t] = new List<Action<object, object>>();
        }
        List<Action<object, object>> lst = m_MsgHandlerDic[t];
        Action<object, object> cbt = lst.Find((Action<object, object> callback) => {
            return callback.Equals(cb);
        });
        if(cbt != null) {
            return;
        }
        lst.Add(cb);

        //TargetMap
        if(cb != null && cb.Target != null) {
            if(!m_TargetMsgDic.ContainsKey(cb.Target)) {
                m_TargetMsgDic[cb.Target] = new List<T>();
            }
            List<T> evtLst = m_TargetMsgDic[cb.Target];
            evtLst.Add(t);
        }
    }
    public void RmvMsgHandler(T id) {
        if(m_MsgHandlerDic.ContainsKey(id)) {
            var handlerLst = m_MsgHandlerDic[id];
            foreach(Action<object, object> cb in handlerLst) {
                if(cb != null
                    && cb.Target != null
                    && m_TargetMsgDic.ContainsKey(cb.Target)) {
                    var idLst = m_TargetMsgDic[cb.Target];
                    idLst.RemoveAll((T t) => {
                        return id.Equals(t);
                    });
                    if(idLst.Count == 0) {
                        m_TargetMsgDic.Remove(cb.Target);
                    }
                }
            }
        }
        m_MsgHandlerDic.Remove(id);
    }
    public void RmvTargetHandler(object target) {
        if(m_TargetMsgDic.ContainsKey(target)) {
            List<T> evtLst = m_TargetMsgDic[target];
            for(int i = evtLst.Count - 1; i >= 0; --i) {
                T evt = evtLst[i];
                if(m_MsgHandlerDic.ContainsKey(evt)) {
                    List<Action<object, object>> cbLst = m_MsgHandlerDic[evt];
                    cbLst.RemoveAll((Action<object, object> cb) => {
                        return cb.Target == target;
                    });
                    if(cbLst.Count == 0) {
                        m_MsgHandlerDic.Remove(evt);
                    }
                }
            }
            m_TargetMsgDic.Remove(target);
        }
    }

    public List<Action<object, object>> GetMsgAllHandler(T t) {
        m_MsgHandlerDic.TryGetValue(t, out List<Action<object, object>> lst);
        return lst;
    }
}