using SUProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

//关卡加载系统

public class StageSys : ILogic
{
    //当前关卡配置
    StageCfg cfg;

    SURoot root;
    ResSvc resSvc; UISvc uiSvc;NetSvc netSvc;

    Action LoadStageDone;
    private int currentStageID;
    public int CurrentStageID {  get { return currentStageID; } }

    public void Init()
    {
        this.Log("Init Stage");
        //获取引用
        root = SURoot.Instance;
        resSvc = root.resSvc;
        uiSvc = root.uiSvc;
        netSvc= root.netSvc;
        //发送请求登入消息
        netSvc.RegistHandler(CMD.NtfEnterStage, NtfEnterStage);
    }

    public void Tick()
    {
        
    }
    public void UnInit()
    {
    }
    //通知调整关卡
    public void NtfEnterStage(NetMsg netMsg)
    {
        NtfEnterStage ntf = netMsg.ntfEnterStage;
        LoadGameStage(ntf.stageID, () =>
        {
            root.EnterGameMode(XlsCfgTool.GetStageConfig(ntf.stageID).playMode);

            netSvc.SendMsg(new NetMsg
            {
                cmd = CMD.FinEnterStage,
                finEnterStage = new FinEnterStage
                {
                    mode = ntf.mode,
                    prefixID = ntf.prefixID,
                    stageID = ntf.stageID,
                }
            });
            root.evtSvc.SendEvt(EvtID.OnStageLoaded,ntf.stageID);
        });
    }

    

    //游戏关卡加载

    //知识点 Action Func Action 不能有返回值，Func<... , 返回值类型>必须有返回值 最后一个模板参数为
    //decimal

    //第一次在服务、系统、模式   加载一个默认关卡
    public void LoadGameStage(int stageID,Action loadStageDone)
    {
        //保存要加载场景信息
        currentStageID = stageID;
        this.LoadStageDone = loadStageDone;

        //根据id获取具体场景配置文件
        cfg =XlsCfgTool.GetStageConfig(stageID);

        if (cfg.isGhost)
        {
            //镜像场景地图，单人剧情模式 #TODO
            this.Log("水波屏幕特效过渡 #TODO");
        }
        else
        {
            uiSvc.SetLoading(0, true);
        }
        LoadScene(cfg.sceneName);
        
    }

    private void LoadScene(string  sceneName)
    {
        resSvc.LoadSceneAsync(sceneName,
            (progress) =>
            {
                UpdateLoadingProgress(progress);
            },
            () =>
            {
                UpdateLoadingProgress(1);
            }
        );
    }

    private void UpdateLoadingProgress(float progress)
    {
        this.LogCyan($"load:{progress}");
        int loadPercentage = (int)(progress * 100);
        //更新UI进度条
        if (LoadStageDone != null && !cfg.isGhost)
        {
            uiSvc.SetLoading(loadPercentage);
        }
        if ((loadPercentage == 100))
        {
            LoadStageDone?.Invoke();
            LoadStageDone= null;
        }
        
    }
}

