using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// 游戏物体容器 By:xiongjunyu
/// </summary>
public class ObjBox : MonoBehaviour
{
    private static ObjBox mInstance;

    public static ObjBox Get
    {
        get
        {
                return ObjBox.mInstance;
        }
    }

    [Header("(建议)请将需要添加到容器的游戏物体标签设置为:")]
    public string[] Tags;

    [Header("如果不能使用Tag查找的物体请拖拽大这里:")]
    public GameObject[] NoTagObjs;

    [Header("如果物体需要多个Tag请在下面设置组名拖到相应的组内：")]
    public ObjGroup[] objGroups;

    private Dictionary<string, Dictionary<string, GameObject>> mObjs = new Dictionary<string, Dictionary<string, GameObject>>();
    void Awake()
    {
        if (mInstance !=null )
        {
            Debug.LogError("ObjBox:"+"场景中存在多个ObjBox!!!");
        }
        else
        {
            mInstance = this;
        }

        //添加无标签物体到字典
        mObjs.Add("", new Dictionary<string, GameObject>());
        foreach (var item in NoTagObjs)
        {
            mObjs[""].Add(item.name, item);
        }

        //通过标签查找物体并加入字典
        GameObject[] GameObjects;
        foreach (string tag in Tags)
        {
            mObjs.Add(tag, new Dictionary<string, GameObject>());
            GameObjects = GameObject.FindGameObjectsWithTag(tag);

            foreach (var item in GameObjects)
            {
                if (mObjs[tag].ContainsKey(item.name))
                {
                    Debug.LogError("ObjBox：" + tag + " 标签下存在同名游戏物体");
                }
                else
                {
                    mObjs[tag].Add(item.name, item);
                }
            }
        }
        //添加分组游戏物体
        foreach (ObjGroup objGroup in objGroups)
        {
            mObjs.Add(objGroup.GroupName, new Dictionary<string, GameObject>());
            foreach (var element in objGroup.elements)
            {
                mObjs[objGroup.GroupName].Add(element.name, element);
            }
        }
    }
    /// <summary>
    /// 根据标签和名字获取物体
    /// </summary>
    /// <param name="Tag">标签名或分组名</param>
    /// <param name="Name">游戏物体名</param>
    /// <returns></returns>
    public GameObject this[string Tag, string Name]
    {

        get { return mObjs[Tag][Name]; }
    }
    /// <summary>
    /// 根据标签获取 对应的游戏物体字典
    /// </summary>
    /// <param name="Tag"></param>
    /// <returns></returns>
    public Dictionary<string, GameObject> this[string Tag]
    {
        get { return mObjs[Tag]; }
    }
}
/// <summary>
/// 游戏物体组
/// </summary>
[Serializable]
public class ObjGroup
{
    public string GroupName;
    public GameObject[] elements;
}
