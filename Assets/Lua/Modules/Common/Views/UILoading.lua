---
--- 通用的Loading界面
---
local UIBase = require("Core.ui.UIBase")
local UILoading = Class("UILoading",UIBase)

-- virtual 子类可以初始化一些变量,ResId要在这里赋值
function UILogin:InitParam()
    self.ResId = 101
end

return UILoading