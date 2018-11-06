---
--- UI助手类
---
local UIUtils = Class("UIUtils")

-- 获取I18N文字
function UIUtils.GetText(id)
    local cfg = ConfigMgr:Instance():GetItem("Language",id)
    if cfg then
        return cfg.text or ""
    end
    return ""
end

return UIUtils


