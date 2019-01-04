---
--- UI工具类
---
local UI_Utils = Class("UI_Utils")

local COMMON_COLORS = {
    Red = Color(225, 0, 0),
    Green = Color(0, 225, 0),
    Blue = Color(0, 0, 225),
    White = Color(240, 244, 244),
    Black = Color(17, 37, 69),
    Gray = Color(91, 93, 112),
    Yellow = Color(227, 147, 7),
}
local chineseNumIndex = 1

-- 获取I18N文字
function UI_Utils.GetText(id)
    local cfg = ConfigMgr:Instance():GetItem("Language",id)
    if cfg then
        return cfg.text or ""
    end
    return ""
end

function UI_Utils.GetColor(name)
    return COMMON_COLORS[name] or COMMON_COLORS.White
end

function UI_Utils.GetChineseNumber(number)
    if number >= 0 and number <= 9 then
        return UI_Utils.GetText(chineseNumIndex + number)
    else
        warn(string.format("输入的数字%d不在0~9范围内！",number))
        return ""
    end
end

return UI_Utils


