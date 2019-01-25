---
--- Modules的定义
---
local Modules = {}

Modules.moduleId = require("Game.Main.ModuleId")
Modules.notifyId = require("Game.Main.ModuleId")

Modules.moduleList = {
    ---require("Modules.Login.Module.LoginModule")
}

-- 注册全局变量
define("Modules",Modules)

return Modules