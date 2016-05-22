using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /// 当前UI
    [SerializeField]
    [Header("请将导航按钮设置标签NavBtn，以 名称_目标面板名称_当前面板名称 格式命名，Panle以 P_ 开头")]
    private GameObject currentUI;

    private Stack<GameObject> historyUI = new Stack<GameObject>();

    private void Start()
    {
        //为导航按钮添加事件
        foreach (var item in ObjBox.Get["NavBtn"].Values)
        {
            item.GetComponent<Button>().onClick.AddListener(OnNavBtnClick);
        }
        EndStartInit();
    }

    /// Start执行完毕后隐藏其他面板
    private void EndStartInit()
    {
        foreach (var item in ObjBox.Get["Panle"].Values)
        {
            if (!item.name.EndsWith("Main"))
            {
                item.SetActive(false);
            }
            else
            {
                currentUI = item;
            }
        }
    }

    /// 显示Panle
    private void Show(string PanleName)
    {
        currentUI.SetActive(false);
        currentUI = ObjBox.Get["Panle", "P_" + PanleName];
        currentUI.SetActive(true);
    }

    /// 导航按钮被单击
    private void OnNavBtnClick()
    {
        string panleName = EventSystem.current.currentSelectedGameObject.name.Split('_')[1];
        if (panleName == "Back")
        {    //回退页面
            if (historyUI.Count > 0)
            {
                currentUI.SetActive(false);
                currentUI = historyUI.Pop();
                currentUI.SetActive(true);
            }
        }
        else
        {
            //跳转页面
            historyUI.Push(currentUI);
            Show(panleName);
        }
    }
}