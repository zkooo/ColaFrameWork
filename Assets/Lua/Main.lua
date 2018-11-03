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
	-- 必须首先注册全局Class,顺序敏感
	define("Class",require("middleclass"))
	define("LuaLogHelper",require("LuaLogHelper"))
	define("CommonUtility",require("CommonUtility"))
	define("ConfigMgr",require("ConfigMgr"))
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

	local text = ConfigMgr:Instance():GetItem("Language",10000).text
	local cfg1 = ConfigMgr:new()
	local cfg2 = ConfigMgr:new()
	print("------->测试读取配置",text)

end

--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	Time.timeSinceLevelLoad = 0
end

function OnApplicationQuit()
end