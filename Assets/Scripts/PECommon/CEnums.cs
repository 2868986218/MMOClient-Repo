public enum UIWindowEnum
{
    None,

    //cover
    Tips,
    Pop,
    Loading,
    Cover,

    //base
    Login,
    Create,
    Major,
    Wild,
    SingleFuben,
    MultiFuben
}

//UI组件枚举
public enum UIComp
{
    None,
    Activity,
    Chat,
    DirInput,
    HpItem,
    Operate,
    PlayerInfo,
    Skill,
    Task
}
public enum WindowLayer {
    None,
    BaseUI,
    PopUI,
    TopUI
}

//资源路径配置
public class PathDefine
{
    //路径并未完全验证可能有问题
    public const string ItemPath = "UIPrefab/Item/";

    public const string CompPath = "UIPrefab/Comp/";
    public const string NpcPath = "ResNpc/";
    public const string PropPath = "ResProp/";
    public const string MonsterPath = "ResMonster/";
    public const string PlayerPath = "ResPlayer/";
    public const string EffectPath = "ResEffects/";
    public const string AudioPath = "ResAudio/";

    public const string PlayerInfoComp = "PlayerInfoComp";
    public const string TaskComp = "TaskComp";
    public const string DirInputComp = "DirInputComp";
    public const string ChatComp = "ChatComp";
    public const string OperateComp = "OperateComp";
    public const string SkillComp = "SkillComp";
    public const string ActivtyComp = "ActivityComp";
}