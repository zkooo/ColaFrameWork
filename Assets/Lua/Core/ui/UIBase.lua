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
    self:InitParam()
end

-- virtual 子类可以初始化一些变量
function UIBase:InitParam()

end

function UIBase:Open()

end

function UIBase:Create()

end

function UIBase:OnCreate()

end

function UIBase:Close()

end

function UIBase:OnShow(isShow)

end

function UIBase:Show(isActive)

end

function UIBase:Destroy()

end

function UIBase:OnDestroy()

end

function UIBase:AttachSubPanel()

end

function UIBase:RegisterSubPanel()

end

function UIBase:DetchSubPanel()

end

function UIBase:DestroySubPanels()

end

function UIBase:BringTop()

end

function UIBase:ShowUIBlur()

end

function UIBase:SetOutTouchDisappear()

end

function UIBase:AttachListener(gameObejct)

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