---
---  通用接口工具类
---

local Common_Utils = Class("Common_Utils")

Common_Utils._instance  = nil
-- 可读写的路径
Common_Utils.AssetPath = ""
-- lua脚本的根目录
Common_Utils.LuaDir = UnityEngine.Application.dataPath .. "/Lua"

function Common_Utils:Instance()
    if Common_Utils._instance == nil then
        Common_Utils._instance = Common_Utils:new()
    end
    return Common_Utils._instance
end

-- override 初始化各种数据
function Common_Utils:initialize()
end

return Common_Utils