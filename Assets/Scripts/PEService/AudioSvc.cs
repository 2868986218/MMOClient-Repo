using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//声音管理服务
public class AudioSvc : ILogic
{
    SURoot root;
    ResSvc resSvc;

    public bool TurnOnVoic = true;
    AudioSource bgAudio;
    AudioSource uiAudio;
    public void Init()
    {
        root = SURoot.Instance;
        resSvc = root.resSvc;

        bgAudio =root.transform.Find("BGAudio").GetComponent<AudioSource>();
        uiAudio =root.transform.Find("UIAudio").GetComponent<AudioSource>();

        this.Log("Init Audio done");
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TurnOnVoic = true;
        }else if(Input.GetKeyDown(KeyCode.F2)) {
            TurnOnVoic = false;
        }
    }

    public void UnInit()
    {
    }

    public void PlayUIAudio(string name)
    {
        if(!TurnOnVoic) { return; }
        AudioClip clip=resSvc.LoadAudio($"{PathDefine.AudioPath}{name}",true);
        uiAudio.clip= clip;
        uiAudio.Play();
    }
}

