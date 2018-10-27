--主入口函数。从这里开始lua逻辑
local luaLogHelper = require("LuaLogHelper")

local function initialize()
	luaLogHelper.initialize()
end

function Main()
	initialize()
	print("logic start")
	print("------>Test","abd",{1,2,3,4,5})
	warn("------->warnTest",debug.traceback())
	error("---------->ErrorTest")
end

--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	Time.timeSinceLevelLoad = 0
end

function OnApplicationQuit()
end