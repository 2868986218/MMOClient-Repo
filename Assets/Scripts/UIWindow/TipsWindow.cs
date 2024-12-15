
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

//弹出提示UI的类
public class TipsWindow : WindowRoot
{
    //背景，文字提示
    public Image bgTips;
    public Text txtTips;
    //动画
    public Animator animator;
    //当前是否有提示框ui在播放
    bool isTipsShow = false;

    //存储提示消息ui的队列
    readonly Queue<string> tipsQueue = new Queue<string>();

    protected override void InitWnd()
    {
        base.InitWnd();
        SetActive(bgTips, false);
        tipsQueue.Clear();
    }

    protected void Update()
    {
        //Debug.Log("Trueeeeee");
        //当前有消息需要弹出，并且处于空闲状态
        if (tipsQueue.Count > 0 && isTipsShow==false)
        {
            //获取消息内容
            string tips=tipsQueue.Dequeue();
            //将状态改为忙
            isTipsShow = true;

            //显示提示ui
            int len=tips.Length;
            SetActive(bgTips,true);
            txtTips.text = tips;
            //计算显示框的大小
            bgTips.GetComponent<RectTransform>().sizeDelta = new Vector2(35 * len + 100, 80);
            //播放提示ui动画
            animator.Play("TipsWindow", 0, 0);
        }
    }

    public void AddTips(string tips)
    {
        tipsQueue.Enqueue(tips);
    }

    //完成一次消息播放
    public void AnimatorPlayDone()
    {
        SetActive(bgTips, false);
        isTipsShow=false;
    }

}
