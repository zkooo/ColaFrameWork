using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;

public class LuaEngine : LuaClient {

    protected override LuaFileUtils InitLoader()
    {
        return base.InitLoader();
    }

    protected override void LoadLuaFiles()
    {
#if UNITY_EDITOR
        luaState.AddSearchPath(Application.dataPath + "/lua");
#endif
        base.LoadLuaFiles();
    }
}
