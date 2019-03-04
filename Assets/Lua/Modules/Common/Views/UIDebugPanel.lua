---
--- UIDebugPanel UI类
---

local UIBase = require("Core.ui.UIBase")
local UIDebugPanel = Class("UIDebugPanel",UIBase)

-- virtual 子类可以初始化一些变量,ResId要在这里赋值
function UIDebugPanel:InitParam()
    self.ResId = 102
end

-- override UI面板创建结束后调用，可以在这里获取gameObject和component等操作
function UIDebugPanel:OnCreate()
    self.DebugText = self.Panel:FindChildByPath("ScrollView/Viewport/Text"):GetComponent("UnityEngine.UI.Text")
end

-- 界面可见性变化的时候触发
function UIDebugPanel:OnShow(isShow)

end

-- 界面销毁的过程中触发
function UIDebugPanel:OnDestroy()

end

-- 注册UI事件监听
function UIDebugPanel:RegisterEvent()

end

-- 取消注册UI事件监听
function UIDebugPanel:UnRegisterEvent()

end

------------------- UI事件回调 --------------------------
function UIDebugPanel:onClick(obj)
    if obj.name == "BtnClose" then

    elseif obj.name == "BtnClear" then

    end
end

function UIDebugPanel:onBoolValueChange(obj, isSelect)

end

---------------------- UI事件回调 --------------------------

return UIDebugPanel