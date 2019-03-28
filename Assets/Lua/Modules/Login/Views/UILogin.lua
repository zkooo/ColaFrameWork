local UIBase = require("Core.ui.UIBase")
local UILogin = Class("UILogin",UIBase)

local _instance = nil

function UILogin.Instance()
    if nil == _instance then
        _instance = UILogin:new()
    end
    return _instance
end

-- virtual 子类可以初始化一些变量,ResId要在这里赋值
function UILogin:InitParam()
    self.ResId = 100
    self.uiDepthLayer = ECEnumType.UIDepth.NORMAL
end

-- override UI面板创建结束后调用，可以在这里获取gameObject和component等操作
function UILogin:OnCreate()
    local logoImage = self.Panel:GetComponentByPath("logo","Image")
    UTL.UI.SetImageSpriteFromAtlas(2001, logoImage, "airfightSheet_3", false)
    TimeHelper.SetTimer(function ()
        UIManager.Instance():Open(ECEnumType.UIEnum.Setting)
    end,6)
    TimeHelper.SetTimer(function ()
        self:BringTop()
    end,10)
end

-- 界面可见性变化的时候触发
function UILogin:OnShow(isShow)

end

function UILogin:onClick(obj)
    if obj.name == "showLogBtn" then
        UIManager.Instance():Open(ECEnumType.UIEnum.DebugPanel)
    end
end

return UILogin