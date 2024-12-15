using PETimer;
using System;


public class TimerSvc : ILogic
{
    readonly TickTimer timer = new(0, false);
    public void Init()
    {
        timer.LogFunc = this.Log;
        timer.WarnFunc = this.Warn;
        timer.ErrorFunc = this.Error;
        this.Log("Init TimerSvc Done");
    }

    public void Tick()
    {
        timer.UpdateTask();
    }

    public void UnInit()
    {
        this.Log("UnInit TimerSvc");
    }

    public int AddTask(uint delay,Action<int> taskCB,Action<int> cancelCB=null,int count = 1)
    {
        return timer.AddTask(delay, taskCB, cancelCB, count);
    }

    public bool DeleteTask(int tid)
    {
        return timer.DeleteTask(tid);
    }

    
}
