---
--- Lua的table助手类
---

local TableHelper = Class("TableHelper")

-- 深拷贝一个table
function TableHelper.DeepCopy(object)
    local SearchTable = {}

    local function Func(object)
        if type(object) ~= "table" then
            return object
        end
        local NewTable = {}
        SearchTable[object] = NewTable
        for k, v in pairs(object) do
            NewTable[Func(k)] = Func(v)
        end

        return setmetatable(NewTable, getmetatable(object))
    end
    return Func(object)
end

return TableHelper