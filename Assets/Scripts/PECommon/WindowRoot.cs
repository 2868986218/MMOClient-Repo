using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//所有UI窗口基类
public class WindowRoot:MonoBehaviour
{
    //是否允许组件显示
    public bool EnableCompShow;

    protected SURoot root;
    protected NetSvc netSvc;
    protected ResSvc resSvc;
    protected UISvc uiSvc;
    protected AudioSvc audioSvc;

    public WindowLayer windowLayer = WindowLayer.None;
    public UIComp uiComp=UIComp.None;


    readonly protected Dictionary<int, WindowRoot> uiCompDic = new Dictionary<int, WindowRoot>();

    public void SetWindowState(bool isActive = true)
    {
        if(gameObject.activeSelf != isActive)
        {
            gameObject.SetActive(isActive);
        }
        if (isActive)
        {
            InitWnd();
        }
        else
        {
            UnInitWnd();
        }
    }
    protected virtual void InitWnd()
    {
        root = SURoot.Instance;
        netSvc = root.netSvc;
        resSvc = root.resSvc;
        uiSvc = root.uiSvc;
        audioSvc = root.audioSvc;

        //uiComp init
        for(int i=0;i<transform.childCount;i++)
        {
            Transform child=transform.GetChild(i);
            if (child.name.EndsWith("Comp"))
            {
                GameObject go;
                if(child.childCount == 0)
                {
                    go = root.resSvc.LoadPrefab($"{PathDefine.CompPath}{child.name}", true);
                    go.transform.SetParent(child);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localScale = Vector3.one;
                }else
                {
                    go = child.GetChild(0).gameObject;
                }

                WindowRoot compWindow=go.GetComponent<WindowRoot>();
                Type type = Type.GetType(child.name);
                if (type != null)
                {
                    int hashCode=type.GetHashCode();
                    if (!uiCompDic.ContainsKey(hashCode))
                    {
                        uiCompDic.Add(hashCode, compWindow);
                    }
                    compWindow.SetWindowState();
                    this.LogCyan($"add comp:{type.Name}");
                }
                else
                {
                    this.Error("gameObject命名与脚本不一致");
                }

            }
        }

    }

    protected virtual void UnInitWnd()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform t=transform.GetChild(i);
            if (t.name.EndsWith("Comp"))
            {
                if(t.childCount == 1)
                {
                    GameObject obj = t.GetChild(i).gameObject;
                    WindowRoot comp=obj.GetComponent<WindowRoot>();
                    comp.SetWindowState(false);
                }
            }
        }

        root = null;
        netSvc = null;
        resSvc = null;
        uiSvc = null;
        audioSvc = null;

    }

    //UI组件通用方法
    protected void SetActive(GameObject go, bool state = true)
    {
        go.SetActive(state);
    }
    protected void SetActive(Transform trans, bool state = true)
    {
        trans.gameObject.SetActive(state);
    }
    protected void SetActive(RectTransform rectTrans, bool state = true)
    {
        rectTrans.gameObject.SetActive(state);
    }
    protected void SetActive(Image img, bool state = true)
    {
        img.gameObject.SetActive(state);
    }
    protected void SetActive(Text txt, bool state = true)
    {
        txt.gameObject.SetActive(state);
    }
    protected void SetActive(InputField ipt, bool state = true)
    {
        ipt.gameObject.SetActive(state);
    }
    protected void SetText(Transform trans, int num = 0)
    {
        SetText(trans.GetComponent<Text>(), num.ToString());
    }
    protected void SetText(Transform trans, string context = "")
    {
        SetText(trans.GetComponent<Text>(), context);
    }
    protected void SetText(Text txt, int num = 0)
    {
        SetText(txt, num.ToString());
    }
    protected void SetText(Text txt, string context = "")
    {
        txt.text = context;
    }

    protected Transform GetTrans(Transform trans, string name)
    {
        if (trans != null)
        {
            return trans.Find(name);
        }
        else
        {
            return transform.Find(name);
        }
    }
    protected Image GetImage(Transform trans, string path)
    {
        if (trans != null)
        {
            return trans.Find(path).GetComponent<Image>();
        }
        else
        {
            return transform.Find(path).GetComponent<Image>();
        }
    }
    protected Image GetImage(Transform trans)
    {
        if (trans != null)
        {
            return trans.GetComponent<Image>();
        }
        else
        {
            return transform.GetComponent<Image>();
        }
    }
    protected Text GetText(Transform trans, string path)
    {
        if (trans != null)
        {
            return trans.Find(path).GetComponent<Text>();
        }
        else
        {
            return transform.Find(path).GetComponent<Text>();
        }
    }
    private T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T t = go.GetComponent<T>();
        if (t == null)
        {
            t = go.AddComponent<T>();
        }
        return t;
    }

    protected void OnClick(GameObject go, Action<PointerEventData, object[]> clickCB, params object[] args)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClick = clickCB;
        if (args != null)
        {
            listener.args = args;
        }
    }
    protected void OnClickDown(GameObject go, Action<PointerEventData, object[]> clickDownCB, params object[] args)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClickDown = clickDownCB;
        if (args != null)
        {
            listener.args = args;
        }
    }
    protected void OnClickUp(GameObject go, Action<PointerEventData, object[]> clickUpCB, params object[] args)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClickUp = clickUpCB;
        if (args != null)
        {
            listener.args = args;
        }
    }
    protected void OnDrag(GameObject go, Action<PointerEventData, object[]> dragCB, params object[] args)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onDrag = dragCB;
        if (args != null)
        {
            listener.args = args;
        }
    }


    //引用服务
}

