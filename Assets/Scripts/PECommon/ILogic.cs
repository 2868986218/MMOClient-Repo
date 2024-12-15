using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//所有业务
public interface ILogic
{

    public void Init();

    //tick 滴答
    //执行周期性逻辑检查或更新。在游戏开发中，Tick() 方法通常在每一帧调用，用于处理游戏逻辑，如移动、碰撞检测等。
    public void Tick();

    //用于反初始化或清理资源。当对象不再需要时，可以调用这个方法来释放占用的资源，如内存、文件句柄等。
    public void UnInit();
}
