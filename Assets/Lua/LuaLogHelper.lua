-- Lua端的Log助手

local rawprint = print
local rawerror = error
local logHelper = {}

local isLog = true  -- 是否打印日志
local tablePritDepth = 5 --table最深的打印层次
local logFunc = nil

-- 普通日志
function logHelper.debug(...)
	rawprint("------>函数",logFunc ==nil)
	if logFunc then
		LogFunction(1,true,...)
	end
end

-- 警告
function logHelper.warn(...)

end

-- 错误
function logHelper.error(...)

end

-- 初始化
function logHelper.initialize()
	logFunc = LogFunction
end

-- 函数注册到全局
rawset(_G, "print", logHelper.debug)
rawset(_G, "warn", logHelper.warn)
rawset(_G, "error", logHelper.error)
rawset(_G, "rawprint", rawprint)
rawset(_G, "rawerror", rawerror)

return logHelper