---
--- AutoInject自动依赖注入组件
---

local AutoInject = Class("AutoInject")

-- 扫描gameObject
local function scanAllObjs(gameObject, path_stack, callback)
    path_stack = path_stack or {}
    if not path_stack then
        return
    end

    path_stack[#path_stack + 1] = gameObject.name

    if callback then
        local over = callback(gameObject, path_stack)
        if over then
            return true
        end
    end

    local count = gameObject.transform.childCount

    for i = 0, count - 1 do
        local obj = gameObject.transform.GetChild(i).gameObject
        if scanAllObjs(obj, path_stack, callback) then
            return
        end
    end

    path_stack[#path_stack] = nil
end

-- 检查是否有重名的内容
local function checkSameRoot(rootnow, rootRegist)
    local hasParent = rootRegist ~= nil
    local sameRoot = false
    if hasParent then
        local length = #rootRegist
        for i = 0, length - 1 do
            sameRoot = rootRegist[#rootRegist - i] == rootnow[#rootnow - i]
            if not sameRoot then
                break
            end
        end
    end
    return hasParent, sameRoot
end

-- 按照路径顺序查找
function AutoInject.InjectWithPath(gameObjectRoot, injectObj)
    local options = injectObj.options

    for k, v in ipairs(options) do
        local gameObject = gameObjectRoot:FindChildByPath(k)

        for _, m in pairs(v) do
            local fieldItem = m.fieldItem
            local componentName = m.componentName

            if gameObject then
                if componentName == "GameObject" or nil == componentName then
                    injectObj:add_field(fieldItem, gameObject)
                elseif "Transform" == componentName then
                    injectObj:add_field(fieldItem, gameObject.transform)
                else
                    injectObj:add_field(fieldItem, gameObject:GetComponent(componentName))
                end
            end
        end
    end

    injectObj.options = nil
    injectObj.count = nil
end

--按照gameobject名字遍历查找 参数：gameObject_root - 根节点 , inject_object - 注入设置
function AutoInject.Inject (gameObject_root, inject_object)
    local options = inject_object.options
    local checkrecord = {}
    -- local count = 0
    local function CheckAndFind(_gameobject, path_stack)
        local name = (_gameobject == gameObject_root and "") or _gameobject.name

        if options and options[name] then
            --判断是不是有重名项
            local isSameName = false
            if not checkrecord[name] then
                checkrecord[name] = true
            else
                isSameName = true
            end

            for k, v in pairs(options[name]) do
                --检查是否设置路径，如果设置了路径是否相同
                local hasParent, sameRoot = checkSameRoot(path_stack, v.parentroot)

                --检查是否有同名的引用项，如果有提出警报
                if isSameName and not hasParent then
                    print("发现同名引用项，可能会导致错误..", v.name)
                end

                if (hasParent and sameRoot) or not hasParent then
                    local fieldItem = v.fieldItem
                    local componetname = v.componetname

                    if (componetname == "GameObject" or componetname == nil) then
                        inject_object:add_field(fieldItem, _gameobject)
                    elseif (componetname == "Transform") then
                        inject_object:add_field(fieldItem, _gameobject.transform)
                    else
                        inject_object:add_field(fieldItem, _gameobject:GetComponent(componetname))
                    end
                    --简单校验下，数量够了，就不再找了
                    options[name][k] = nil
                    if next(options[name]) == nil then
                        options[name] = nil
                    end
                end
            end
        end
        --检查注册的项是否都以查询完
        return next(options) == nil
    end
    scanAllObjs(gameObject_root, nil, CheckAndFind)
    -- print(count)

    inject_object.options = nil
    inject_object.count = nil
end

return AutoInject