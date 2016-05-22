using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

public class XJYEditor : EditorWindow
{
    public static int No = 0;
    public static int ChildNo = 0;
    public static string setNumber = string.Empty;
    public static string firstName = "A";
    public static string endName = "1";
    //public static  string childNameToFind="";
    //public static  string childTypeToFind="Camera";

    private static void SelectByTag()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);
        Selection.objects = gos;
    }

    [MenuItem("XJY/改名/手动递增改名 %q")]
    private static void ChangeNameOneByOne()
    {
        Selection.activeGameObject.name = firstName + No.ToString("000");
        No++;
    }

    private static void ChangeChidrensEndName()
    {
        ChangeChidrensEndNameFun(Selection.activeGameObject.transform);
    }

    private static void RemoveChidrensEndName()
    {
        RemoveChidrensEndNameFun(Selection.activeGameObject.transform);
    }

    private static void ChangeChidrensEndNameFun(Transform t)
    {
        foreach (Transform item in t)
        {
            item.name = item.name + endName;
            ChangeChidrensEndNameFun(item);
        }
    }

    private static void RemoveChidrensEndNameFun(Transform t)
    {
        foreach (Transform item in t)
        {
            item.name = item.name.Remove(item.name.Length - 1);
            RemoveChidrensEndNameFun(item);
        }
    }

    private static void ChangeChidrensName()
    {
        foreach (Transform item in Selection.activeGameObject.transform)
        {
            item.name = firstName + ChildNo.ToString("000");
            ChildNo++;
        }
    }

    //创建工程文件夹
    [MenuItem("XJY/其他功能/创建工程文件夹/确认/再次确认")]
    private static void CreatFolders()
    {
        //AssetDatabase.CreateFolder("Assets", "Plugins");
        //AssetDatabase.CreateFolder("Assets/Plugins", "Android");
        //AssetDatabase.CreateFolder("Assets/Plugins", "IOS");
        //AssetDatabase.CreateFolder("Assets", "Scripts");
        //AssetDatabase.CreateFolder("Assets", "Scripts");
        //AssetDatabase.CreateFolder("Assets", "Resources");
        //AssetDatabase.CreateFolder("Assets/Resources", "Art");
        //AssetDatabase.CreateFolder("Assets/Resources/Art", "UI");
        //AssetDatabase.CreateFolder("Assets/Resources/", "Texture");
        //AssetDatabase.CreateFolder("Assets/Resources/", "Mode");
        //AssetDatabase.CreateFolder("Assets/Resources/", "Shader");
        //AssetDatabase.CreateFolder("Assets", "StreamingAssets");
        //AssetDatabase.CreateFolder("Assets", "TTTTTTTTTTT");
        string dataPath = Application.dataPath;

        string[] projectPaths = new string[] {
				"/Datas",
				"/Effects",
				"/Contents",
				"/Resources/Shaders",
				"/Scripts",
                "/StreamingAssets"
			};

        DirectoryInfo dirInfo;
        for (int i = 0; i < projectPaths.Length; ++i)
        {
            dirInfo = new DirectoryInfo(dataPath + projectPaths[i]);
            if (!dirInfo.Exists) dirInfo.Create();

            AssetDatabase.ImportAsset("Assets" + projectPaths[i]);
        }
    }

    [MenuItem("XJY/其他功能/创建资源 ")]
    private static void creat()
    {
        try
        {
            GameObject g = GameObject.Instantiate(Resources.Load<GameObject>("A"));
            g.name = "CameraRender";
            var tex = new RenderTexture(1, 1, 1);
            if (!Directory.Exists(Path.Combine(Application.dataPath, "renderTexture"))) Directory.CreateDirectory((Path.Combine(Application.dataPath, "renderTexture")));

            AssetDatabase.CreateAsset(tex, "Assets/renderTexture/" + g.name + ".renderTexture");
            g.GetComponent<Camera>().targetTexture = tex;
        }
        catch (Exception)
        {
            Debug.LogWarning("此功能需要在代码中定制");
        }

        // Add an animation clip to it
        //添加一个动画剪辑到材质上
        //var animationClip = new AnimationClip();
        //animationClip.name = "My Clip";
        //AssetDatabase.AddObjectToAsset(animationClip, material);

        //// Reimport the asset after adding an object.
        ////在新建一个对象后重新导入资源
        //// Otherwise the change only shows up when saving the project
        ////否则这个更改只会在保存工程时才显示
        //AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(animationClip));

        //// Print the path of the created asset
        ////打印新建的资源
        //Debug.Log(AssetDatabase.GetAssetPath(material));
    }

    //计数归零

    private static void NameTo_0()
    {
        No = 0;
        ChildNo = 0;
    }

    //打开工具窗口
    [MenuItem("XJY/工具窗口 ")]
    private static void OpenWindow()
    {
        EditorWindow windwo = GetWindow(typeof(XJYEditor));
        windwo.Show();
    }

    [MenuItem("GameObject/复制名称", false, 0)]
    static public void CopyName()
    {
        GameObject obj = Selection.activeGameObject;
        if (obj == null) return;
        string path = obj.name;
        Debug.Log(path);
        TextEditor te = new TextEditor();
        te.content = new GUIContent(path);
        te.OnFocus();
        te.Copy();
    }

    [MenuItem("GameObject/复制路径", false, 0)]
    static public void GetHierarchy()
    {
        GameObject obj = Selection.activeGameObject;
        if (obj == null) return;
        string path = obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = obj.name + "/" + path;
        }
        Debug.Log(path);
        TextEditor te = new TextEditor();
        te.content = new GUIContent(path);
        te.OnFocus();
        te.Copy();
    }

    [SerializeField]
    private static string childNoStr = 0.ToString();

    private static string tag;

    [MenuItem("Assets/复制资源路径")]
    public static void GetAssetsPath()
    {
        UnityEngine.Object obj = Selection.activeObject;
        if (obj && AssetDatabase.IsMainAsset(obj))
        {
            string path = AssetDatabase.GetAssetPath(obj);
            path = path.Remove(0, 7);
            Debug.Log(path);
            TextEditor te = new TextEditor();
            te.content = new GUIContent(path);
            te.OnFocus();
            te.Copy();
            Debug.Log(Application.dataPath);
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("<-----改名前缀与编号------>");
        firstName = EditorGUILayout.TextField("名字开头：", firstName);
        childNoStr = EditorGUILayout.TextField("初始编号：", childNoStr);
        ChildNo = childNoStr == string.Empty ? 0 : int.Parse(childNoStr);
        GUILayout.Label("预览:" + "  " + firstName + ChildNo.ToString());

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("<------统一子物体编号(后缀)----->");
        endName = EditorGUILayout.TextField("后缀（编号）：", endName);

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("改名"))
        {
            ChangeChidrensName();
        }
        if (GUILayout.Button("归零"))
        {
            NameTo_0();
        }
        if (GUILayout.Button("加后缀"))
        {
            ChangeChidrensEndName();
        }
        if (GUILayout.Button("除后缀"))
        {
            RemoveChidrensEndName();
        }
        EditorGUILayout.EndHorizontal();
        ///////////////////////////////////
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("<-----标签查找物体------>");
        EditorGUILayout.BeginHorizontal();
        tag = EditorGUILayout.TextField("标签", tag);
        if (GUILayout.Button("查找"))
        {
            SelectByTag();
        }
        EditorGUILayout.EndHorizontal();
    }

    //#if UNITY_EDITOR
    //    string filepath = Application.dataPath + "/StreamingAssets" + "/my.xml";

    //#elif UNITY_IPHONE
    //      string filepath = Application.dataPath +"/Raw"+"/my.xml";

    //#elif UNITY_ANDROID
    //      string filepath = "jar:file://" + Application.dataPath + "!/assets/"+"/my.xml;

    //#endif
    [MenuItem("XJY/其他功能/输出移动平台StreamingAssets路径")]
    public static void PrintStreamingAssetsPath() { Debug.Log("双击我从代码231行复制"); }

    [MenuItem("XJY/其他功能/文件对话框获取文件路径")]
    public static void WindowGetPathFile()
    {
        string path = EditorUtility.OpenFilePanel("选择文件", "", "");
        Debug.Log(path);
        TextEditor te = new TextEditor();
        te.content = new GUIContent(path);
        te.OnFocus();
        te.Copy();
    }

    [MenuItem("XJY/其他功能/文件对话框获取文件 夹 路径")]
    public static void WindowGetPathFloder()
    {
        string path = EditorUtility.OpenFolderPanel("获取文件夹", "", "");
        Debug.Log(path);
        TextEditor te = new TextEditor();
        te.content = new GUIContent(path);
        te.OnFocus();
        te.Copy();
    }

    [MenuItem("XJY/示例功能/打开对话框")]
    public static void DisplayDialog()
    {
        if (EditorUtility.DisplayDialog("Do you want to build all datas ?", "根据你的选择来决定做什么事！", "是", "否"))
        {
            //BuildResources ("Datas");
            Debug.Log("OK");
        }
        else
        {
            Debug.Log("No");
        }
    }
}