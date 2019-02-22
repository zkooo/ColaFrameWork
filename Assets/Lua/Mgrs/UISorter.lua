---
--- uiSorter UI界面层级排序
---

local uiSorter = Class("uiSorter")

function uiSorter:initialize()
    self.minSortIndex = 0
    self.maxSortIndex = 0
    self.is3DHigher = true
    self.uiDic = {} --key:UI名字,value:UIInfo
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
function uiSorter:SortIndexSetter(panel, sortIndex)

end

-- 设置UI的SortTag,根据显示修改上下关系做到排序
function uiSorter:SortTagIndexSetter()

end

-- 设置带有3D模型UI的SortTag，带3d模型的ui需要排序设置
function uiSorter:SortTag3DSetter()

end

-- 添加打开面板时调用，会重排UI
function uiSorter:AddPanel(uiPanel)
    if nil == uiPanel then
        error("invalid param #2 to AddPanel, can not be nil")
        return
    end
    if self.uiDic[uiPanel.PanelName] then
        error("AddPanel failed, the panel is already added: " .. uiPanel.PanelName)
        return
    end
    local panelInfo = { panelName = uiPanel.PanelName, panel = uiPanel, layer = uiPanel.Layer, index = 0, moveTop = 1 }
    self.uiDic[uiPanel.PanelName] = panelInfo
    table.insert(self.uiSortList, panelInfo)

    -- 重排UI界面
    self:ResortPanels()
end

--  移除关闭面板时调用，会重排UI
function uiSorter:RemovePanel(uiPanel)
    local panelInfo = self.uiDic[uiPanel.PanelName]
    if not panelInfo then
        warn("Failed to RemovePanel: the panel not found: " .. uiPanel.PanelName)
        return
    end
    self.uiDic[uiPanel.PanelName] = nil

    -- 重排UI界面
    self:ResortPanels()
end

-- 将指定的UI提升到当前UILEVEL的最上层
function uiSorter:MovePanelToTop(uiPanel)
    local panelInfo = self.uiDic[uiPanel.PanelName]
    if not panelInfo then
        warn("Failed to MovePanelToTop: the panel not found: " .. uiPanel.PanelName)
        return
    end
    panelInfo.moveTop = 1
    self:ResortPanels()
end

-- 将指定的UI提升到指定UILEVEL的最上层
function uiSorter:MovePanelToTopOfLayer(uiPanel, layer)
    local panelInfo = self.uiDic[uiPanel.PanelName]
    if not panelInfo then
        warn("Failed to MovePanelToTop: the panel not found: " .. uiPanel.PanelName)
        return
    end
    panelInfo.moveTop = 1
    panelInfo.layer = layer
    self:ResortPanels()
end

-- 重排UI界面
-- 根据UI的打开先后顺序先赋值index，然后根据uiDepthLayer\moveTop\index三者权重进行UI重排
function uiSorter:ResortPanels()

end

return uiSorter