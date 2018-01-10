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
    public Dictionary<int,ResPathData> resPathDataList = new Dictionary<int, ResPathData>();

    public void SetMapCsv(string[] rows)
    {
        resPathDataList.Clear();

        for (int i = 3; i < rows.Length; i++)
        {
            ResPathData data = new ResPathData();
            data.InitWithStr(rows[i]);
            resPathDataList.Add(data.id,data);
        }
    }

    public ResPathData GetDataById(int id)
    {
        ResPathData data = null;
        if (null != resPathDataList)
        {
            resPathDataList.TryGetValue(id, out data);
        }
        return data;
    }
}