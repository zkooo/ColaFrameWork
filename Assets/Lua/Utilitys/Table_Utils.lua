---
--- Lua的table工具类
---

local Table_Utils = Class("Table_Utils")

-- override 初始化各种数据
function Table_Utils.initialize()

end

-- 深拷贝一个table
function Table_Utils.DeepCopy(object)
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

return Table_Utils