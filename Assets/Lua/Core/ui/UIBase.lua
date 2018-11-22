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

-- virtual 子类可以申请一些变量
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

return UIBase