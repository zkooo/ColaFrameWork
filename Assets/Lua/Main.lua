--主入口函数。从这里开始lua逻辑
local luaLogHelper = require("LuaLogHelper")

local function initialize()
	luaLogHelper.initialize()
end

function Main()
	initialize()
	print("logic start")
	local t = {"a",'b','c'}
	local tt = {}
	tt["key"] = 123
	tt["value"] = 456
	local ttt = {t,tt}
	print("------>Test","abd",{5,4,3,2,1,t,tt,ttt})
	warn("------->warnTest",debug.traceback())
	error("---------->ErrorTest")

	local UIView = require("UIView").Instance()
	local result = UIView:TestFunc(123)
	print("------->结果",result)
end

--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	Time.timeSinceLevelLoad = 0
end

function OnApplicationQuit()
end