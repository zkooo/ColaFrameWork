using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI排序管理器
/// </summary>
public class UISorterMgr : ISorter
{
    private int minSortIndex = 0;
    private int maxSortIndex = 0;
    private List<UIBase> uiSortList;
    private List<Canvas> canvasSortList;

    public UISorterMgr(int minIndex, int maxIndex)
    {
        minSortIndex = minIndex;
        maxSortIndex = maxIndex;
        uiSortList = new List<UIBase>();
        canvasSortList = new List<Canvas>();
    }

    /// <summary>
    /// UI排序管理器构造器,序号越大，界面越靠上
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="sortIndex"></param>
    /// <returns></returns>
    public int SortIndexSetter(GameObject panel, int sortIndex)
    {
        if (null == panel)
        {
            Debug.LogWarning("参与排序的ui不能为空！");
            return 0;
        }
        canvasSortList.Clear();
        var canvasList = panel.GetComponentsInChildren<Canvas>(true);
        for (int i = 0; i < canvasList.Length; i++)
        {
            canvasSortList.Add(canvasList[i]);
        }
        canvasSortList.Sort((x, y) => { return x.sortingOrder.CompareTo(y.sortingOrder); });

        for (int i = 0; i < canvasSortList.Count; i++)
        {
            canvasSortList[i].sortingOrder = sortIndex;
            //DropDown组件关闭按钮的层级为DropDown层级减一，所以多加一个间隔
            sortIndex += 2;
        }
        return sortIndex + 2;
    }

    public int SortTagIndexSetter(GameObject panel, int sortIndex)
    {
        throw new System.NotImplementedException();
    }

    public int SortTag3DSetter(GameObject model, Vector3 postion)
    {
        throw new System.NotImplementedException();
    }

    public void MovePanelToTop(UIBase ui)
    {
        throw new System.NotImplementedException();
    }

    public void ReSortPanels()
    {
        throw new System.NotImplementedException();
    }

    public void AddPanel(UIBase ui, UILevel uiLevel)
    {
        throw new System.NotImplementedException();
    }

    public void RemovePanel(UIBase ui)
    {
        throw new System.NotImplementedException();
    }
}
