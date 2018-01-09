using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 资源路径信息类
/// </summary>
[Serializable]
class ResPathData :LocalDataBase
{
    public string resPath;
    public int resType;
    public override void InitWithStr(string strData, char splitChar = ',')
    {
        //去除最后的\r
        if (strData.EndsWith("\r"))
        {
            strData = strData.Substring(0, strData.Length - 1);
        }
        string[] strs = strData.Split(splitChar);
        id = this.GetInt(strs[0]);
        resPath = strs[1];
        resType = this.GetInt(strs[2]);
    }
}


/// <summary>
/// ResPathData的数据集类
/// </summary>
class ResPathDataMap : ILocalDataMapBase
{
    public void SetMapCsv(string[] rows)
    {
        throw new NotImplementedException();
    }
}