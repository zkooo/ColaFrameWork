using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 存储所有的系统
/// </summary>
public class Modules
{
    public List<ModuleBase> AllModules;

    public Modules()
    {
        Init();
    }

    private void Init()
    {
        AllModules = new List<ModuleBase>();
        AllModules.Add(new LoginModule());
    }
}
