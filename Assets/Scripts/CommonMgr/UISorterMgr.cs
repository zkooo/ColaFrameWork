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

    public UISorterMgr(int minIndex, int maxIndex)
    {
        minSortIndex = minIndex;
        maxSortIndex = maxIndex;
        uiSortList = new List<UIBase>();
    }

    /// <summary>
    /// UI排序管理器构造器,序号越大，界面越靠上
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="sortIndex"></param>
    /// <returns></returns>
    public int SortIndexSetter(GameObject panel, int sortIndex)
    {
        throw new System.NotImplementedException();
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
