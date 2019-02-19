---
--- UISorter UI界面层级排序
---

local UISorter = Class("UISorter")

function UISorter:initialize()
    self.minSortIndex = 0
    self.maxSortIndex = 0
    self.uiSortList = {}
    self.canvasSortList = {}
end

return UISorter