---
---  UIView 测试类
---

local ColaPlus = require("ColaPlus")
local UIView = ColaPlus.Class("UIView")

local def = UIView.define

def.field("number").testNum = 1001
def.field("string").testString = "1002"

local _instance = nil

def.static("=>",UIView).Instance = function()
    if _instance == nil then
        _instance = UIView()
    end
    return _instance
end

def.method("number","=>","string").TestFunc = function(self,param)
    local ss = string.format("%d  %d   %s",param,self.testNum,self.testString)
    return ss
end

return UIView.Commit()