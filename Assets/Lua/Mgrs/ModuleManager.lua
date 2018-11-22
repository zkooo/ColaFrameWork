---
--- Module类的管理器
---

local ModuleManager = Class("ModuleManager")
local Modules = require("module.Modules")

ModuleManager._instance = nil

function ModuleManager:Instance()
    if nil == ModuleManager._instance then
        ModuleManager._instance = ModuleManager:new()
    end
    return ModuleManager
end

-- override 初始化各种数据
function ModuleManager:initialize()
    self.moduleList = {}
end

function ModuleManager:RegisterModule(module)
    local moduleId = module:GetModuleId()
    self.moduleList[moduleId] = module
end

function ModuleManager:GetModule(moduleId)
    return self.moduleList[moduleId]
end

return ModuleManager