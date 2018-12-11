---
--- UIManager UI管理器
---

local UIManager = Class("UIManager")

UIManager._instance = nil

function UIManager:initialize()
    self.uiList = {}  -- 存储打开的UI列表
    self.outTouchList = {} -- 用于存储参与点击其他地方关闭面板管理的UI的列表
    self.removeList = {} -- 存储要进行统一关闭面板的列表
    self.recordList = {} -- 存储统一隐藏/恢复显示的UI列表
end

function UIManager:Instance()
    if nil == UIManager._instance then
        UIManager._instance = UIManager:new()
    end
    return UIManager._instance
end

return