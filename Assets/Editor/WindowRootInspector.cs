using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(WindowRoot),true)]
public class WindowRootInspector : Editor
{
    //编辑模式下，点击挂载windowroot脚本及其子类的游戏物体执行该脚本，点击将实例化其所有组件
    private void OnEnable()
    {
        //父类指针指向子类对象
        WindowRoot root=(WindowRoot)target;

        if (root == null || root.windowLayer == WindowLayer.None || !root.EnableCompShow || Application.isPlaying || !root.gameObject.activeSelf)
        {
            return;
        }
        //获取其transform用来遍历其子物体
        Transform transform=root.transform;
        //不属于baseUI
        if(root.windowLayer == WindowLayer.None)
        {
            return;
        }

        //遍历其所有子节点empty gameobject
        for(int i=0;i<transform.childCount;i++)
        {
            Transform t=transform.GetChild(i);
            //该子节点名字是否以comp结尾
            if (t.name.EndsWith("Comp"))
            {
                GameObject go;
                //没有子孩子 没有挂载comp
                if (t.childCount == 0)
                {
                    //实例化预制体
                    string path = $"Assets/Resources/{PathDefine.CompPath}{t.name}.prefab";
                    GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    go = PrefabUtility.InstantiatePrefab(obj) as GameObject;
                    go.transform.SetParent(t);
                    go.transform.localPosition=Vector3.zero;
                    go.transform.localScale = Vector3.one;
                }
            }
        }
    }

    private void OnDisable()
    {
        WindowRoot root = (WindowRoot)target;

        if (root == null || root.windowLayer == WindowLayer.None || !root.EnableCompShow || Application.isPlaying || !root.gameObject.activeSelf)
        {
            return;
        }
        Transform transform = root.transform;

        //获取选中并且处于激活状态的对象
        GameObject selObj = Selection.activeObject as GameObject;
        //是否为选中物体的子孩子
        bool isSelectChild = false;
        //为空说明该对象未被激活
        while (selObj != null && selObj.transform.parent != null)
        {
            if (selObj.transform.parent.name == root.name)
            {
                isSelectChild = true;
                break;
            }
            else
            {
                selObj = selObj.transform.parent.gameObject;
            }
        }

        if (!isSelectChild)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform t = transform.GetChild(i);
                if (t.name.EndsWith("Comp") && t.childCount > 0)
                {
                    Transform trans = t.GetChild(0);
                    if (trans != null)
                    {
                        DestroyImmediate(trans.gameObject);
                    }
                }
            }
        }
    }
}
