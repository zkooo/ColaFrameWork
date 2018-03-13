using System;
using System.IO;

public class ECAssetBundleHeader
{
	[Flags]
    public enum BundleOption
    {
		/// <summary>
		/// 手动解决对此资源的依赖
		/// 不靠 AssetBundle 自动解决依赖，因此不需保持 AssetBundle 打开
		/// </summary>
		ManuallyResolve = 0x1 << 0,
		/// <summary>
		/// 不释放此资源 (用于Shader等)
		/// </summary>
		DonotRelease = 0x1 << 1,
        /// <summary>
        /// 分离文件
        /// </summary>
        SepFile = 0x1 << 2,
    }

	/// <summary>
	/// 标志资源类型。用于在实际加载前就知道资源类型。多数资源不需要此信息，只需标记特定类型资源
	/// </summary>
	public enum BundleSpecialType
	{
		None = 0,
		Material = 1,
		Texture = 2,
	}

    public struct DepInfo
    {
        public string name;
        public string path;
    }

    const int cur_ver = 1;
	public BundleSpecialType specialType = BundleSpecialType.None;
    public BundleOption option = 0;
    public DepInfo[] deps;

    public void Save(Stream s)
    {
        BinaryWriter bw = new BinaryWriter(s);
        bw.Write(cur_ver);
		bw.Write((Int32)specialType);
        bw.Write((Int32)option);
        bw.Write(deps.Length);

        for (int i = 0; i < deps.Length; i++)
        {
            bw.Write(deps[i].name);
            bw.Write(deps[i].path);
        }
    }

    public void Load(Stream s)
    {
        BinaryReader br = new BinaryReader(s);
        int ver = br.ReadInt32();
		specialType = (BundleSpecialType)br.ReadInt32();
        option = (BundleOption)br.ReadInt32();
        int count = br.ReadInt32();
        deps = new DepInfo[count];

        for (int i = 0; i < count; i++)
        {
            deps[i].name = br.ReadString();
            deps[i].path = br.ReadString();
        }
    }
}
