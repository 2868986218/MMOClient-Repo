/*************************************************
作者: Plane
邮箱: 1785275942@qq.com
功能: 关卡配置文件

           ~~获取更多教学资讯~~
//----------------*----------------\\
        教学官网：www.qiqiker.com
        官方微信服务号: qiqikertuts
        Plane老师微信: PlaneZhong
\\----------------*----------------//
*************************************************/

using System.Numerics;

namespace SUProtocol {
    public class StageCfg {
        public int stageID;
        public string stageName;
        public bool isGhost;
        public string sceneName;
        public CPlayMode playMode;
        public SWorldType worldType;
        public Vector3 teleportPos;
        public float radius;

        public int aoiSize = 5;
        public int[] npcArr;
        public int[] mccIDArr;
        public int[] deviceIDArr;
        public int[] dropIDArr;
    }
    public class NPCConfig {
        public int npcID;
        public int stageID;
        public Vector3 pos;
        public Vector3 rot;
        public string name;
        public string path;
    }
    public class DeviceConfig {
        public int deviceID;
        public Vector3 pos;
        public Vector3 rot;
        public string name;
        public string path;

        public int openCondition;
        public DeviceFunctionEnum effectEnum;
        public object[] args;
    }
    public enum DeviceFunctionEnum {
        None,
        TransMap,
        TransPos,
        OpenUI,
    }
    public enum SWorldType {
        None,
        Resident,
        Temporary,
        CrossRealm,//TODO 跨服战场地图
    }
    public enum CPlayMode {
        None,
        Login,//登录模式 
        Create,//角色创建模式
        Major,//主城模式（主要是客户端NPC任务，社交平台，技能效果表现等）
        Wild,//野外模式（野外刷怪，采集材料等任务）
        Single,//单人副本模式（单人战斗）
        Multiple,//多人组队模式（多人战斗）
        Activty, //定时运营活动，世界Boss战，多少人一组，刷伤害比排行拿奖励。
        Guild,//公会战，拿公会成员玩家数据站位PK TODO
        Marriage,//结婚场景模式 TODO
        Fishing,//钓鱼模式 TODO
        Concert,//演奏模式 TODO......
                //TOADD 通天塔模式......
    }

    public partial class XlsCfgTool {
        public static StageCfg GetStageConfig(int stageID) {
            return stageID switch {
                1 => new StageCfg {
                    stageID = 1,
                    stageName = "账号登录",
                    sceneName = "001_Login",
                    playMode = CPlayMode.Login
                },
                2 => new StageCfg {
                    stageID = 2,
                    stageName = "创建角色",
                    sceneName = "002_Create",
                    playMode = CPlayMode.Create
                },
                101 => new StageCfg {
                    stageID = 101,
                    stageName = "大唐帝国",
                    isGhost = false,
                    sceneName = "101_MajorCity",
                    playMode = CPlayMode.Major,
                    npcArr = new int[] { 101001, 101002, 101003 },
                    worldType = SWorldType.Resident,
                    teleportPos = new Vector3(-4, 0, 10),
                    radius = 1.5f,

                    mccIDArr = new int[] { 10101 },
                    deviceIDArr = new[] { 101201, 101202 }
                },
                _ => null,
            };
        }

        public static NPCConfig GetNPCConfig(int npcID) {
            return npcID switch {
                101001 => new NPCConfig {
                    npcID = 101001,
                    stageID = 101,
                    pos = new Vector3(-11, 0, 5),
                    rot = new Vector3(0, 175, 0),
                    name = "团长",
                    path = "npc_Chief"
                },
                101002 => new NPCConfig {
                    npcID = 101002,
                    stageID = 101,
                    pos = new Vector3(-2.4f, 0, 8.5f),
                    rot = new Vector3(0, -90, 0),
                    name = "副团长",
                    path = "npc_Assistant"
                },
                101003 => new NPCConfig {
                    npcID = 101003,
                    stageID = 101,
                    pos = new Vector3(-4.1f, 0, 16.5f),
                    rot = new Vector3(0, 180, 0),
                    name = "队长",
                    path = "npc_Captain"
                },
                _ => null
            };
        }
        public static DeviceConfig GetDeviceConfig(int deviceID) {
            return deviceID switch {
                101201 => new() {
                    deviceID = 101201,
                    pos = new Vector3(-8.64f, 0, -9.67f),
                    rot = new Vector3(0, 30, -9.67f),
                    name = "野外传送门",
                    path = "prop_portal",
                    openCondition = 0,
                    effectEnum = DeviceFunctionEnum.TransMap,
                    args = new object[] { 103 }
                },
                101202 => new() {
                    deviceID = 101202,
                    pos = new Vector3(0, 0, -9.67f),
                    rot = new Vector3(0, -30, -9.67f),
                    name = "世界Boss战入口",
                    path = "prop_bossfight",
                    openCondition = 2,
                    effectEnum = DeviceFunctionEnum.TransMap,
                    args = new object[] { 401 }
                },
                _ => null
            };
        }
    }
}