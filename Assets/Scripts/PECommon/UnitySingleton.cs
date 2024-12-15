using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//unity中的单例模板

//T继承MonoBehaviour
public class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;
    public static T Instance {  get { return instance; } }

    private void Awake()
    {
        instance = (T)(MonoBehaviour)this;
    }

}
