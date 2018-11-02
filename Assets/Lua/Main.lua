--主入口函数。从这里开始lua逻辑
local rawset = rawset

-- 全局函数
-- 用于声明全局变量
function define(name,value)
	rawset(_G,name,value)
end

local function initialize()
	LuaLogHelper.initialize()
end

-- 在此处定义注册一些全局变量
local function gloablDefine()
	define("LuaLogHelper",require("LuaLogHelper"))
end

-- 初始化一些参数
local function initParam()
	-- 初始化随机种子
	math.randomseed(tostring(os.time()):reverse():sub(1, 6))
end

function Main()
	gloablDefine()
	initParam()
	initialize()
end

--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	Time.timeSinceLevelLoad = 0
end

function OnApplicationQuit()
end