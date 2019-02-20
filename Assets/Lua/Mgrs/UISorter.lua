---
--- uiSorter UI界面层级排序
---

local uiSorter = Class("uiSorter")

function uiSorter:initialize()
    self.minSortIndex = 0
    self.maxSortIndex = 0
    self.uiSortList = {}
    self.canvasSortList = {}
end

function uiSorter.Create(minSortIndex, maxSortIndex)
    local uiSorter = uiSorter:new()
    uiSorter.minSortIndex = minSortIndex
    uiSorter.maxSortIndex = maxSortIndex
    return uiSorter
end

-- 对某个Gameobject下的Canvas进行动态排序功能,识别子canvas，根据sortingOrder对canvas的排序
function uiSorter:SortIndexSetter(panel,sortIndex)

end

-- 设置UI的SortTag,根据显示修改上下关系做到排序
function uiSorter:SortTagIndexSetter()

end

-- 设置带有3D模型UI的SortTag，带3d模型的ui需要排序设置
function uiSorter:SortTag3DSetter()

end

-- 添加打开面板时调用，会重排UI
function uiSorter:AddPanel(uiPanel)

end

--  移除关闭面板时调用，会重排UI
function uiSorter:RemovePanel(uiPanel)

end

return uiSorter