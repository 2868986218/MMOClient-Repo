using SUProtocol;
using PEUtils;
using System.Collections.Generic;
using UnityEngine;

//驱动服务与所有的业务系统
//入口
public class SURoot : UnitySingleton<SURoot>
{
    //List<ILogic> services = new (); 一模一样
    List<ILogic> services = new List<ILogic>();
    List<ILogic> systems = new();

    public TimerSvc timerSvc;
    public ResSvc resSvc;
    public UISvc uiSvc;
    public NetSvc netSvc;
    public AudioSvc audioSvc;
    public EvtSvc evtSvc;

    public AcctSys acctSys;
    public StageSys stageSys;

    // Start is called before the first frame update
    void Start()
    {
        
        //创建对象时同时初始化其属性，日志配置
        LogConfig cfg = new() {
            enableLog = true,
            logPrefix = "",
            enableTime = true,
            logSeparate = ">",
            enableThreadID = true,
            enableTrace = true,
            enableCover = true,
            saveName = "MMOClientSULog.txt",
            loggerEnum=LoggerType.Unity
        };
        PELog.InitSettings(cfg);
        this.Log("GameStart...");

        Application.targetFrameRate = 60;

        
        DontDestroyOnLoad(this);

        
        //将所有UI设为不可见
        Transform uiRoot = transform.Find("Canvas");
        for (int i = 0; i < uiRoot.childCount; i++)
        {
            uiRoot.GetChild(i).gameObject.SetActive(false);
        }

        //初始化服务
        InitSVC();

        //初始位业务系统
        InitSYS();
        //初始化游戏模式字典
        InitFSM();

       
        stageSys.LoadGameStage(1, () =>
        {
            //进入登入模式
            EnterGameMode(CPlayMode.Login);
        });



    }

    // Update is called once per frame
    void Update()
    {
        //手动更新
        //更新不是继承至MonoBehaviour的类的update方法
        //services update
        for (int i = 0; i < services.Count; i++)
        {
            services[i].Tick();
        }
        //systems update
        for (int i = 0; i < systems.Count; i++)
        {
            systems[i].Tick();
        }

        if(currentMode != CPlayMode.None)
        {
            fsm[currentMode].Update();
        }

    }

    private void OnApplicationQuit()
    {
        for (int i = services.Count-1; i >=0; i--)
        {
            services[i].UnInit();
        }
    }

    readonly Dictionary<CPlayMode,BaseMode>fsm=new Dictionary<CPlayMode,BaseMode>();
    private CPlayMode currentMode = CPlayMode.None;

    //更改游戏模式
    public void EnterGameMode(CPlayMode targetMode)
    {
        //是否存在目标模式
        if (fsm.ContainsKey(targetMode))
        {
            //与老师不一样
            if(currentMode != CPlayMode.None)
            {
                //多态
                fsm[currentMode].Exit();
            }
            
            fsm[targetMode].Enter();
            currentMode = targetMode;
        }
        
    }


    public void ExitGameMode()
    {
        if(currentMode != CPlayMode.None)
        {
            fsm[currentMode].Exit();
            currentMode= CPlayMode.None;
        }
    }

    private void InitFSM()
    {
        //初始化游戏模式字典
        fsm.Add(CPlayMode.Login, new LoginMode());
        fsm.Add(CPlayMode.Major, new MajorMode());
        fsm.Add(CPlayMode.Create, new CreateMode());
        fsm.Add(CPlayMode.Wild, new WildMode());

    }

    private void InitSVC()
    {
        timerSvc = new TimerSvc();
        resSvc = new ResSvc();
        uiSvc = new UISvc();
        netSvc = new NetSvc();
        audioSvc=new AudioSvc();
        evtSvc=new EvtSvc();
        services.Add(timerSvc);
        services.Add(resSvc);
        services.Add(uiSvc);
        services.Add(netSvc);
        services.Add(audioSvc);
        services.Add(evtSvc);
        for (int i = 0; i < services.Count; i++)
        {
            services[i].Init();
        }
    }

    private void InitSYS()
    {
        acctSys =new AcctSys();
        stageSys =new StageSys();
        systems.Add(acctSys);
        systems.Add(stageSys);
        for (int i = 0; i < systems.Count; i++)
        {
            systems[i].Init();
        }

    }
}
