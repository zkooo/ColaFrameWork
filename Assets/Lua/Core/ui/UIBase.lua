---
--- UIBase基类
---

local UIBase = Class("UIBase")

-- override 初始化各种数据
function UIBase:initialize()
    self.Panel = nil
    self.ResId = 0
    self.Layer = 0
    self.UILevel = 0
    self.subUIList = {}
    self.uiDepthLayer = 0
    self.uiCanvas = nil
    self.sortEnable = true
    self.sorterTag = nil
    self:InitParam()
end

-- virtual 子类可以初始化一些变量,ResId要在这里赋值
function UIBase:InitParam()

end

-- 对外调用，用于创建UI
function UIBase:Create()
    if nil ~= self.Panel then
        GameObject.Destroy(self.Panel)
    end
    self.Panel = UTL.LuaCommon.InstantiateGoById(self.ResId,Common_Utils.GetUIRootObj())
    if self.sortEnable then
        self.sorterTag = self.Panel:AddSingleComponent(SorterTag)
        self.uiCanvas = self.Panel:AddSingleComponent(Canvas)
        self.Panel:AddSingleComponent(GraphicRaycaster)
        self.uiCanvas.overrideSorting = true
        self.Panel:AddSingleComponent(ParticleOrderAutoSorter)

        --TODO:新的UI排序管理
        CommonHelper.GetUIMgr().GetUISorterMgr().AddPanel(self)
    end
    self:AttachListener(self.Panel)
    self:OnCreate()
end

--对外调用，用于创建UI，不走ResId加载，直接由现有gameObject创建
function UIBase:CreateWithGo(gameObejct)
    self.Panel = gameObejct
    self.Panel = CommonHelper.InstantiateGoByID(self.ResId, GUIHelper.GetUIRootObj())
    if self.sortEnable then
        self.sorterTag = self.Panel:AddSingleComponent(SorterTag)
        self.uiCanvas = self.Panel:AddSingleComponent(Canvas)
        self.Panel:AddSingleComponent(GraphicRaycaster)
        self.uiCanvas.overrideSorting = true
        self.Panel:AddSingleComponent(ParticleOrderAutoSorter)

        CommonHelper.GetUIMgr().GetUISorterMgr().AddPanel(self)
    end
    self:AttachListener(self.Panel)
    self:OnCreate()
    self:OnShow(self:IsVisible())
end

-- override UI面板创建结束后调用，可以在这里获取gameObject和component等操作
function UIBase:OnCreate()

end

-- 界面可见性变化的时候触发
function UIBase:OnShow(isShow)

end

-- 设置界面可见性
function UIBase:SetVisible(isVisible)

end

function UIBase:IsVisible()
    return self.Panel.activeSelf
end

-- 销毁一个UI界面
function UIBase:Destroy()

end

-- 界面销毁的过程中触发
function UIBase:OnDestroy()

end

-- 关联子UI，统一参与管理
function UIBase:AttachSubPanel()

end

-- 将一个UI界面注册为本UI的子界面，统一参与管理
function UIBase:RegisterSubPanel()

end

-- 解除子UI关联
function UIBase:DetchSubPanel()

end

--  销毁关联的子面板，不要重写
function UIBase:DestroySubPanels()

end

-- 将当前UI层级提高，展示在当前Level的最上层
function UIBase:BringTop()

end

-- 显示UI背景模糊
function UIBase:ShowUIBlur()

end

-- 设置点击外部关闭(执行该方法以后，当点击其他UI的时候，会自动关闭本UI)
function UIBase:SetOutTouchDisappear()

end

-- 注册UIEventListener
function UIBase:AttachListener(gameObejct)

end

-- 注册UI事件监听
function UIBase:RegisterEvent()

end

-- 取消注册UI事件监听
function UIBase:UnRegisterEvent()

end

------------------- UI事件回调 --------------------------
function UIBase:onClick(obj)

end

function UIBase:onBoolValueChange(obj, isSelect)

end

function UIBase:onEvent(eventName)

end

function UIBase:onFloatValueChange(obj, value)

end

function UIBase:onStrValueChange(obj, text)

end

function UIBase:onDrag(obj, deltaPos, curToucPosition)

end

function UIBase:onBeginDrag(obj, deltaPos, curToucPosition)

end

function UIBase:onEndDrag(obj, deltaPos, curToucPosition)

end
---------------------- UI事件回调 --------------------------

return UIBase