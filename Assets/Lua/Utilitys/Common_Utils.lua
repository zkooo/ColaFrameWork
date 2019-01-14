---
---  通用接口工具类
---

local Common_Utils = Class("Common_Utils")

Common_Utils._instance  = nil
-- 可读写的路径
Common_Utils.AssetPath = ""
-- lua脚本的根目录
Common_Utils.LuaDir = UnityEngine.Application.dataPath .. "/Lua"

-- override 初始化各种数据
function Common_Utils.initialize()

end

return Common_Utils