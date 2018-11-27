---
--- AutoInjectOption 自动注册Option
---

local AutoInjectOption = Class("AutoInjectOption")

local function split(s, p)
    local rt = {}
    string.gsub(s, '[^' .. p .. ']+', function(w)
        table.insert(rt, w)
    end)
    return rt
end

-- 注册
function AutoInjectOption:regist(fieldItem, objectName, compoentname, parent)
    self.options = self.options or {}
    objectName = objectName or ""
    local parentstack = nil
    if parent then
        parentstack = split(parent, ".")
    end
    local item = { objectName = objectName, fieldItem = fieldItem, compoentname = compoentname, parentRoot = parentstack }
    self.options[objectName] = self.options[objectName] or {}
    table.insert(self.options[objectName], item)
    self.count = (self.count == nil and 1) or self.count + 1
end

function AutoInjectOption:addField(fieldItem, item)
    self[fieldItem] = item
end

return AutoInjectOption