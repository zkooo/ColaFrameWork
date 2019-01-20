local UIBase = require("Core.ui.UIBase")
local UILogin = Class("UILogin",UIBase)

-- virtual 子类可以初始化一些变量,ResId要在这里赋值
function UILogin:InitParam()
    self.ResId = 100
    EventMgr:Instance():RegisterEvent(1,1,self.OnClick)
end

-- override UI面板创建结束后调用，可以在这里获取gameObject和component等操作
function UILogin:OnCreate()
    print("------------->UILogin OnCreate")
    EventMgr:Instance():DispatchEvent(1,1,{1,2,3,4,5})
end

-- 界面可见性变化的时候触发
function UILogin:OnShow(isShow)
    print("----------------->UILogin OnShow",isShow)
end

function UILogin:onClick(obj)
    print("---------------------->UILogin Click",obj)
end

function UILogin.OnClick(param)
    print("---------------------->UILogin Click WithParam",param)
end

return UILogin