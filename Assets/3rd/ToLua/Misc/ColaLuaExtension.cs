using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对ToLua进行拓展，可以在这里手动注册一些方法
/// </summary>
public static class ColaLuaExtension
{
    /// <summary>
    /// 供Lua端调用的打印方法
    /// </summary>
    /// <param name="L"></param>
    /// <returns></returns>
    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    private static int LogFunction(IntPtr L)
    {
        try
        {
            if (LuaDLL.lua_gettop(L) > 1 && LuaDLL.lua_isboolean(L, 2))
            {
                var logTag = LuaDLL.lua_tointeger(L, 1);
                LuaDLL.lua_remove(L, 1);

                var traceback = LuaDLL.lua_toboolean(L, 1);
                LuaDLL.lua_remove(L, 1);
                return InnerPrint(L, logTag, traceback);
            }
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    /// <summary>
    /// 内部对lua的日志进行打印
    /// </summary>
    /// <param name="L"></param>
    /// <param name="traceback"></param>
    /// <returns></returns>
    private static int InnerPrint(IntPtr L, int logTag, bool traceback = true)
    {

        return 0;
    }

    /// <summary>
    /// 内部对luaTable进行打印
    /// </summary>
    /// <param name="L"></param>
    /// <param name="traceback"></param>
    /// <returns></returns>
    private static int InnerPrintTable(IntPtr L, int logTag, bool traceback = true)
    {

        return 0;
    }
}
