---
---  通用助手类，C#端命名为CommonHelper
---

local CommonUtility = Class("CommonUtility")

CommonUtility._instance  = nil

function CommonUtility:Instance()
    if CommonUtility._instance == nil then
        CommonUtility._instance = CommonUtility:new()
    end
    return CommonUtility._instance
end

-- override 初始化各种数据
function CommonUtility:initialize()
end

return CommonUtility