-- Lua端的Log助手

local rawprint = print
local rawerror = error
local logHelper = {}

local isLog = true  -- 是否打印日志
local tablePritDepth = 5 --table最深的打印层次
local logFunc = nil
local warnFunc = nil
local errorFunc = nil

-- 普通日志
function logHelper.debug(...)
    -- test
    local str = ""
    for _, v in ipairs({ ... }) do
        if v then
            if type(v) == "string" then
                str = str .. v
            elseif type(v) == "number" then
                str = str .. tostring(v)
            elseif type(v) == "table" then
                local tmpStr = ""
                for _,j in ipairs(v) do
                    tmpStr = tmpStr .. tostring(j)
                end
                str = str .. tmpStr
            end
        end
    end
    rawprint(str)
end

-- 警告
function logHelper.warn(...)

end

-- 错误
function logHelper.error(...)

end

-- 初始化
function logHelper.initialize()

end

-- 函数注册到全局
rawset(_G, "print", logHelper.debug)
rawset(_G, "warn", logHelper.warn)
rawset(_G, "error", logHelper.error)
rawset(_G, "rawprint", rawprint)
rawset(_G, "rawerror", rawerror)

return logHelper