local UIBase = require("Core.ui.UIBase")
local UILogin = Class("UILogin",UIBase)

-- virtual 子类可以初始化一些变量,ResId要在这里赋值
function UILogin:InitParam()
    self.ResId = 100
end

-- override UI面板创建结束后调用，可以在这里获取gameObject和component等操作
function UILogin:OnCreate()
    print("------------->UILogin OnCreate")
end

-- 界面可见性变化的时候触发
function UILogin:OnShow(isShow)
    print("----------------->UILogin OnShow",isShow)
end

function UILogin:onClick(obj)
    print("---------------------->UILogin Click",obj.name)
end

return UILogin