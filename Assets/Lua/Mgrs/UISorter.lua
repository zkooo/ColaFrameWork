---
--- uiSorter UI界面层级排序
---

local uiSorter = Class("uiSorter")

function uiSorter:initialize()
    self.minSortIndex = 0
    self.maxSortIndex = 0
    self.uiSortList = {}
    self.canvasSortList = {}
end

function uiSorter.Create(minSortIndex, maxSortIndex)
    local uiSorter = uiSorter:new()
    uiSorter.minSortIndex = minSortIndex
    uiSorter.maxSortIndex = maxSortIndex
    return uiSorter
end

return uiSorter