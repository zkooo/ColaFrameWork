using LuaInterface;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对ToLua进行拓展，可以在这里手动注册一些方法
/// </summary>
public static class ColaLuaExtension
{
    private static StringBuilder stringBuilder = new StringBuilder();
    public static readonly int PrintTableDepth = 5;

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

        int n = LuaDLL.lua_gettop(l);
        LuaDLL.lua_getglobal(l, "tostring");
        for (int i = 1; i <= n; i++)
        {
            if (mTablePrintDepth > 0 && LuaDLL.lua_type(l, i) == LuaTypes.LUA_TTABLE)
            {
                PrintTable(l, n + 1, i, 0, mStrBuffer);
            }
            else
            {
                LuaDLL.lua_pushvalue(l, -1);
                LuaDLL.lua_pushvalue(l, i);
                LuaDLL.lua_call(l, 1, 1);
                mStrBuffer.Append(LuaDLL.lua_tostring(l, -1));
                LuaDLL.lua_pop(l, 1);

                if (i == n)
                { mStrBuffer.AppendLine(); }
                else
                { mStrBuffer.Append(INDENT_CHAR); }
            }
        }

        if (traceback)
        {
            LuaDLL.lua_getglobal(l, "debug");
            LuaDLL.lua_getfield(l, -1, "traceback");
            LuaDLL.lua_pushnil(l);
            //移除日志打印函数相关的栈
            //Print in C# (level 1) -> logw._logHandler (level 2) -> logw.log (level 3) -> log call (level 4)
            LuaDLL.lua_pushinteger(l, 4);
            LuaDLL.lua_call(l, 2, 1);
            mStrBuffer.AppendLine(LuaDLL.lua_tostring(l, -1));
        }

        switch (logLevel)
        {
            case LL_ERROR:
                LogHandler.Error(mStrBuffer.ToString());
                break;
            case LL_CRITICAL:
                LogHandler.Critial(mStrBuffer.ToString());
                break;
            case LL_WARN:
                LogHandler.Warning(mStrBuffer.ToString());
                break;
            case LL_LOG:
                LogHandler.Log(mStrBuffer.ToString());
                break;
            case LL_DEBUG:
                LogHandler.Debug(mStrBuffer.ToString());
                break;
            default:
                LogHandler.ErrorFormat("Unknown log level {0}: {1}", logLevel, mStrBuffer.ToString());
                break;
        }

        mStrBuffer.Remove(0, mStrBuffer.Length);
        return 0;
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
        var indent = layer > 0 ? new string(INDENT_CHAR, layer * INDENT_CNT) : string.Empty;

        sb.Append(indent).AppendLine("{");
        /* table 放在索引 't' 处 */
        LuaDLL.lua_pushnil(l);  /* 第一个 key */
        while (LuaDLL.lua_next(l, tbIndex) != 0)
        {
            var keyType = LuaDLL.lua_type(l, -2);
            var valType = LuaDLL.lua_type(l, -1);

            LuaDLL.lua_pushvalue(l, tsIndex);//tostring
            LuaDLL.lua_pushvalue(l, -3);//key
            LuaDLL.lua_call(l, 1, 1);//call tostring on key
            if (keyType == LuaTypes.LUA_TNUMBER)
            {
                sb.Append(indent).Append(INDENT_CHAR, INDENT_CNT).AppendFormat("[{0}] = ", LuaDLL.lua_tostring(l, -1));
            }
            else
            {
                sb.Append(indent).Append(INDENT_CHAR, INDENT_CNT).AppendFormat("{0} = ", LuaDLL.lua_tostring(l, -1));
            }
            LuaDLL.lua_pop(l, 1);//remove key str

            if (layer + 1 < mTablePrintDepth && valType == LuaTypes.LUA_TTABLE)
            {
                sb.AppendLine();
                PrintTable(l, tsIndex, LuaDLL.lua_gettop(l), layer + 1, sb);
            }
            else
            {
                LuaDLL.lua_pushvalue(l, tsIndex);//tostring
                LuaDLL.lua_pushvalue(l, -2);//value
                LuaDLL.lua_call(l, 1, 1);//call tostring on value
                sb.AppendLine(LuaDLL.lua_tostring(l, -1));
                LuaDLL.lua_pop(l, 1);//remove value str
            }

            /* 移除 'value' ；保留 'key' 做下一次迭代 */
            LuaDLL.lua_pop(l, 1);
        }
        sb.Append(indent).AppendLine("}");
        return 0;
    }
}
