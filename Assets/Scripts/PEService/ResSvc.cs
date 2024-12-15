using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * 资源加载服务
 */
public class ResSvc : ILogic
{
    //更新场景加载进度回调
    private Action UpdateSceneLoadingProgressCB;

    private readonly Dictionary<string, GameObject> cacheDic = new Dictionary<string, GameObject>();

    //对该路径预制体判断是否需要缓存和实例化
    public GameObject LoadPrefab(string path,bool isCache = false)
    {
        if(!cacheDic.TryGetValue(path, out GameObject obj))
        {
            obj = Resources.Load<GameObject>(path);
            if (isCache)
            {
                cacheDic.Add(path, obj );
            }
        }

        GameObject prefab = null;
        if(obj != null)
        {
            prefab = UnityEngine.Object.Instantiate(obj);
        }
        return prefab;

    }
    public void Init()
    {
        this.Log("Init ResSvc Done.");
    }

    public void Tick()
    {
        //更新进度条
        UpdateSceneLoadingProgressCB?.Invoke();
    }

    public void UnInit()
    {
        this.Log("UnInit ResSvc");
    }
    
    //场景异步加载
    public void LoadSceneAsync(string sceneName,Action<float> loadingProgress,Action loadDone)
    {
        //获取unity存活当前场景名字
        if(sceneName==SceneManager.GetActiveScene().name)
        {
            //就是在当前场景中
            loadingProgress?.Invoke(1);
            loadDone?.Invoke();
        }
        else
        {
            //创建一个异步操作来加载名为sceneName的场景，并将这个操作存储在变量ao中
            //异步加载场景非常有用，因为它允许游戏在加载新场景时继续运行，而不会卡顿或冻结。
            //玩家可能会看到一个加载画面或进度条，但游戏的其他逻辑和动画可以继续进行。
            //具有场景加载进度属性
            AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);

            //订阅进度加载方法
            UpdateSceneLoadingProgressCB = () =>
            {
                loadingProgress?.Invoke(ao.progress);
                //异步操作完成，资源加载完毕
                if(ao.isDone)
                {
                    //执行加载场景完成的回调
                    loadDone?.Invoke();

                    // 将进度加载action置空
                    UpdateSceneLoadingProgressCB = null;
                    ao = null;
                }
            };
        }
    }

    readonly Dictionary<string, AudioClip> audioDic = new();

    //音频资源加载
    public AudioClip LoadAudio(string path,bool cache)
    {
        if(!audioDic.TryGetValue(path, out AudioClip audio))
        {
            audio=Resources.Load<AudioClip>(path);
            if(cache)
            {
                audioDic.Add(path, audio);
            }
        }
        return audio;
    }

    readonly Dictionary<string, Sprite> spriteDic = new();

    public Sprite LoadSprite(string path,bool cache)
    {
        if(!spriteDic.TryGetValue(path,out Sprite sprite))
        {
            sprite=Resources.Load<Sprite>(path);
            if (cache)
            {
                spriteDic.Add(path, sprite);
            }
        }
        return sprite;
    }

}
