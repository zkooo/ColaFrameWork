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
    /// 外部调用，统一注册
    /// </summary>
    /// <param name="L"></param>
    public static void Register(LuaState L)
    {
        //内部手动管理LuaState上面的堆栈，并实现功能函数与注册
        L.BeginModule(null);
        L.RegFunction("LogFunction", LogFunction);
        L.EndModule();
    }

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
        Debug.Log("------>打印" + logTag);
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
