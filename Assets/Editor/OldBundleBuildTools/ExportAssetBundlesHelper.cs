#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class ExportAssetBundles
{

    static public BuildTarget CurBuildTarget = BuildTarget.StandaloneWindows;
    static public bool CheckModify = false;
    static public Dictionary<string, bool> ResPathMap = new Dictionary<string, bool>();
    const int min_sep_bundle_size = 1000 * 1024;
    const int min_compress_bundle_size = 2000 * 1024;
    const int min_audio_clip_bundel_size = 256 * 1024;

    [MenuItem("Assets/PNG生成Alpha通道数据")]
    static void PNGGenAlphaTex()
    {
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        foreach (Object sel in selection)
        {
            Texture2D tex = sel as Texture2D;

            if (tex != null)
            {
                Color32[] data = tex.GetPixels32();
                byte[] cont = new byte[data.Length];

                for (int i = 0; i < data.Length; i++)
                {
                    cont[i] = data[i].a;
                }

                string asspath = AssetDatabase.GetAssetPath(tex);
                File.WriteAllBytes(asspath + ".alpha.bytes", cont);
            }
        }
    }

    [MenuItem("Assets/Build AssetBundle From Selection")]
    static void ExportResource()
    {
        CurBuildTarget = EditorUserBuildSettings.activeBuildTarget;
        _ExportResource();
    }

    static void _ExportResource()
    {
        // Bring up save panel
        string path = EditorUtility.SaveFolderPanel("Save Resource", "", "");
        if (path.Length != 0)
        {
            // Build the resource file from the active selection.
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

            foreach (Object sel in selection)
            {
                BuildOne(sel, path);
            }

            Selection.objects = selection;
        }
    }

    [MenuItem("Assets/打成一个包")]
    static void BuildOneBundle()
    {
        ExportResourceToOneBundle();
    }

    static void ExportResourceToOneBundle()
    {
        // Bring up save panel
        string path = EditorUtility.SaveFolderPanel("Save Resource", "", "");
        if (path.Length != 0)
        {
            // Build the resource file from the active selection.
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

            foreach (Object sel in selection)
            {
                BuildToOneBundle(sel, path, true);
            }

            Selection.objects = selection;
        }
    }

    [MenuItem("Assets/Build AssetBundle From Selection In Unity 5")]
    static void ExportResourceInUnity5()
    {
        string path = EditorUtility.SaveFolderPanel("Save Resource", "", "");
        if (path.Length != 0)
        {
            Debug.LogWarning("start_build_time =" + System.DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") ); 
			List<AssetBundleBuildParam> buildParamList = new List<AssetBundleBuildParam>();

            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            foreach (Object sel in selection)
            {
				string assetPath = AssetDatabase.GetAssetPath(sel);
				if (!Directory.Exists(assetPath))
					buildParamList.Add(new AssetBundleBuildParam(assetPath));          
            }


            Debug.LogWarning("build_time count =" + buildParamList.Count.ToString());

            //BuildAssetBundles(path, buildParamList.ToArray());

            List<AssetBundleBuildParam> buildList = PopAssetBundles(buildParamList, 100);
            while (buildList.Count > 0)
            {
                BuildAssetBundles(path, buildList.ToArray());

                buildList = PopAssetBundles(buildParamList, 100);
            }

            Debug.LogWarning("end_build_time =" + System.DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));

            //BuildAssetBundles(path, buildParamList.ToArray());
        }
    }

    static List<AssetBundleBuildParam> PopAssetBundles( List<AssetBundleBuildParam> buildParamList, int count )
    {
        List<AssetBundleBuildParam> popList = new List<AssetBundleBuildParam>();
        for( int i = 0; i < count; i++)
        {
            if( i < buildParamList.Count)
            {
                popList.Add(buildParamList[i]);
            }
        }

        int realCount = popList.Count;
        if( realCount > 0 )
        {
            buildParamList.RemoveRange(0, realCount);
        }

        return popList;
    }

    [MenuItem("Assets/Build AssetBundle To One Bundle From Selection In Unity 5")]
    static void ExportResourceToOneBundleInUnity5()
    {
        string path = EditorUtility.SaveFolderPanel("Save Resource", "", "");
        if (path.Length != 0)
        {
			List<AssetBundleBuildParam> buildParamList = new List<AssetBundleBuildParam>();

            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            foreach (Object sel in selection)
            {
				string assetPath = AssetDatabase.GetAssetPath(sel);
				if (!Directory.Exists(assetPath))
					buildParamList.Add(new AssetBundleBuildParam(assetPath, true));          
            }

			BuildAssetBundles(path, buildParamList.ToArray());
        }
    }
	
	/// <summary>
	/// BuildAssetBundles 所用参数
	/// </summary>
	public class AssetBundleBuildParam
	{
		/// <summary>
		/// 要生成 Asset Bundle 的 asset 文件路径
		/// </summary>
		public string assetPath;
		/// <summary>
		/// 是否生成一个包
		/// </summary>
		public bool buildOneBundle;
		///// <summary>
		///// ECAssetBundleHeader 相关选项
		///// </summary>
		//public ECAssetBundleHeader.BundleOption bundleOption;

		public AssetBundleBuildParam(string assetPath, bool buildOneBundle)
		{
			this.assetPath = assetPath;
			this.buildOneBundle = buildOneBundle;
		}

		public AssetBundleBuildParam(string assetPath) : this(assetPath, false)
		{
		}
	}

	public class AssetBundleBuildParamCollecter
	{
		public List<AssetBundleBuildParam> m_list = new List<AssetBundleBuildParam>();
		private void Add(string assetPath, bool buildOneBundle)
		{
			m_list.Add(new AssetBundleBuildParam(assetPath, buildOneBundle));
		}
		public void Add(string assetPath)
		{
			Add(assetPath, false);
		}
		public void AddBuildOneBundle(string assetPath)
		{
			Add(assetPath, true);
		}

		public AssetBundleBuildParam[] ToArray()
		{
			return m_list.ToArray();
		}
	}

	static string ListToString<T>(IEnumerable<T> list)
	{
		StringBuilder strBuilder = new StringBuilder();
		foreach (var item in list)
		{
			strBuilder.AppendLine(item.ToString());
		}
		return strBuilder.ToString();
	}

	static void SaveAssetBundleOutputFile(ECAssetBundleHeader header, string assetBundleFilePath, string outputFilePath)
	{
		string outputFileDir = Path.GetDirectoryName(outputFilePath);
		Directory.CreateDirectory(outputFileDir);

        var oldOutputFileInfo = new FileInfo(outputFilePath);
		if (oldOutputFileInfo.Exists)
			oldOutputFileInfo.IsReadOnly = false;
        using (FileStream fs = File.Open(outputFilePath, FileMode.Create, FileAccess.Write))
        {
			header.Save(fs);
			byte[] bundleContent = File.ReadAllBytes(assetBundleFilePath);
			fs.Write(bundleContent, 0, bundleContent.Length);
        }
	}

	/// <summary>
	/// 批量生成 Asset Bundle 后清空缓存目录
	/// </summary>
	/// <param name="outputPath">生成路径</param>
	/// <param name="buildParams">要生成哪些文件</param>
	public static void BuildAssetBundles(string outputPath, IEnumerable<AssetBundleBuildParam> buildParams)
	{
		string tempDir = "_assetbundletemp_";
		BuildAssetBundles(outputPath, buildParams, tempDir);
		Directory.Delete(tempDir, true);
	}

	/// <summary>
	/// 批量生成 Asset Bundle，多次生成使用同样的缓存目录可以提高生成效率
	/// </summary>
	/// <param name="outputPath">生成路径</param>
	/// <param name="buildParams">要生成哪些文件</param>
	/// <param name="cacheDir">缓存目录。多次生成使用同样的缓存目录可以提高生成效率</param>
    public static void BuildAssetBundles(string outputPath, IEnumerable<AssetBundleBuildParam> buildParams, string cacheDir)
	{
		//收集需打的包
		Dictionary<string, AssetBundleBuild> buildsMap = new Dictionary<string, AssetBundleBuild>();
		foreach (var buildParam in buildParams)
		{
            if( buildParam.assetPath == "Resources/unity_builtin_extra" )
            {
                continue;
            }
			if (buildParam.buildOneBundle)
				CollectOneAssetBundleBuilds(buildParam.assetPath, buildsMap);
			else
				CollectSeperateAssetBundleBuilds(buildParam.assetPath, buildsMap);
		}
		AssetBundleBuild[] buildsList = new AssetBundleBuild[buildsMap.Count];
		buildsMap.Values.CopyTo(buildsList, 0);
		//按 assetBundleName 排序
		{
			string[] buildsSortKeyList = new string[buildsList.Length];
			for (int i=0; i<buildsList.Length; ++i)
			{
				buildsSortKeyList[i] = buildsList[i].assetBundleName;
			}
			System.Array.Sort(buildsSortKeyList, buildsList);
		}

		//打包非压缩版
		string uncompressedCacheDir = cacheDir + "/uncompressed/assets";
		Directory.CreateDirectory(uncompressedCacheDir);

        AssetBundleManifest uncompressedManifest = BuildPipeline.BuildAssetBundles(uncompressedCacheDir, buildsList, BuildAssetBundleOptions.UncompressedAssetBundle, EditorUserBuildSettings.activeBuildTarget);
		var uncompressedAllBundles = uncompressedManifest.GetAllAssetBundles();

		HashSet<string> needCompressBundleNameSet = new HashSet<string>();

		foreach (string bundleName in uncompressedAllBundles)
		{
			bool needCompress;
			ECAssetBundleHeader header;
			DetermineAssetBundleOption(uncompressedManifest, bundleName, cacheDir, out needCompress, out header);

			if (needCompress)
			{
				needCompressBundleNameSet.Add(bundleName);
				continue;
			}

			string assetBundleFilePath = Path.Combine(uncompressedCacheDir, bundleName);
			string outputFilePath = Path.Combine(outputPath, bundleName+".u3dext");
			SaveAssetBundleOutputFile(header, assetBundleFilePath, outputFilePath);
		}

		//输出非压缩版

        if(needCompressBundleNameSet.Count <= 0 )
        {
            Debug.Log("BuildAssetBundles complete");
            return;
        }

        AssetBundleBuild[] compresseBuildsList = new AssetBundleBuild[needCompressBundleNameSet.Count];
        int j  = 0;
        for (int i = 0; i < buildsList.Length; ++i)
        {
            if (needCompressBundleNameSet.Contains(buildsList[i].assetBundleName))
            {
                if (j >= needCompressBundleNameSet.Count)
                {
                    break;
                }
                compresseBuildsList[j] = buildsList[i];
                j++;
            }
        }

		//打包压缩版
		string compressedCacheDir = cacheDir + "/compressed/assets";
		Directory.CreateDirectory(compressedCacheDir);
        AssetBundleManifest compressedManifest = BuildPipeline.BuildAssetBundles(compressedCacheDir, compresseBuildsList, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
		var compressedAllBundles = compressedManifest.GetAllAssetBundles();

		{	//验证一致性
			bool match = (uncompressedAllBundles.Length == compressedAllBundles.Length);
			if (match)
			{
				for (int i=0; i<uncompressedAllBundles.Length; ++i)
				{
					if (uncompressedAllBundles[i] != compressedAllBundles[i])
					{
						match = false;
						break;
					}
				}
			}
			if (!match)
			{
				Debug.LogErrorFormat("uncompressedManifest and compressedManifest does not match");
				Debug.Log("uncompressed: " + ListToString(uncompressedAllBundles));
				Debug.Log("compressed: " + ListToString(compressedAllBundles));
			}
		}

		//输出压缩版
		foreach (string bundleName in compressedAllBundles)
		{
			if (!needCompressBundleNameSet.Contains(bundleName))
				continue;

			bool needCompress;
			ECAssetBundleHeader header;
			DetermineAssetBundleOption(compressedManifest, bundleName, cacheDir, out needCompress, out header);

			if (!needCompress)
			{
				throw new System.Exception("conflict needCompress value");
			}

			string assetBundleFilePath = Path.Combine(compressedCacheDir, bundleName);
			string outputFilePath = Path.Combine(outputPath, bundleName+".u3dext");
			SaveAssetBundleOutputFile(header, assetBundleFilePath, outputFilePath);
		}

		Debug.Log("BuildAssetBundles complete");
	}

	private static string[] GetDependencies(string assetPath)
	{
		string[] deppaths = AssetDatabase.GetDependencies(new string[] { assetPath });
		
		//排除假依赖
		Object asset = AssetDatabase.LoadMainAssetAtPath(assetPath);
		Object[] depobjs = EditorUtility.CollectDependencies(new Object[] { asset });
		
		// path => is_real_dep
		Dictionary<string, bool> deppathMap = new Dictionary<string, bool>();
		foreach (var deppath in deppaths)
		{
			deppathMap[deppath] = false;
		}
		foreach (var depobj in depobjs)
		{
			var deppath = AssetDatabase.GetAssetPath(depobj);
			if (deppathMap.ContainsKey(deppath))
				deppathMap[deppath] = true;
		}

		List<string> depList = new List<string>();
		foreach (var item in deppathMap)
		{
			if (item.Value)
				depList.Add(item.Key);
		}

		if (deppaths.Length != depList.Count)	//测试用，可删除
		{
			Debug.LogFormat("asset has fake dependencies: {0}", assetPath);
		}

		return depList.ToArray();
	}

    private static string AssetPathToBundleName(string assetpath)
    {
		string assetpathLower = assetpath.ToLower();
        if (assetpathLower == "assets")
            return "";

        if (assetpathLower.StartsWith("assets/"))
            return assetpathLower.Substring("assets/".Length);
		else
			throw new System.Exception("invalid asset path: " + assetpath);
    }


	private static ECAssetBundleHeader.DepInfo[] CalcNormalDeps(string assetBundleName, Object asset, AssetBundleManifest manifest)
	{
		string[] depBundleNames = manifest.GetDirectDependencies(assetBundleName);
		var deps = new ECAssetBundleHeader.DepInfo[depBundleNames.Length];

		for (int n = 0; n < depBundleNames.Length; n++)
		{
			deps[n].name = "";
			deps[n].path = depBundleNames[n]+".u3dext";
		}
		return deps;
	}

	private static ECAssetBundleHeader.DepInfo[] CalcMaterialDeps(string assetBundleName, Material material, AssetBundleManifest manifest)
	{
		string[] depBundleNames = manifest.GetDirectDependencies(assetBundleName);
        if (material.shader != null)
        {
            List<string> pnames = new List<string>();
            int nPCount = ShaderUtil.GetPropertyCount(material.shader);

            for (int n = 0; n < nPCount; n++)
            {
                ShaderUtil.ShaderPropertyType spt = ShaderUtil.GetPropertyType(material.shader, n);

                if (spt == ShaderUtil.ShaderPropertyType.TexEnv)
                {
                    string pn = ShaderUtil.GetPropertyName(material.shader, n);
                    pnames.Add(pn);
                }
            }

            List<ECAssetBundleHeader.DepInfo> deplist = new List<ECAssetBundleHeader.DepInfo>();
            foreach (var depBundleName in depBundleNames)
            {
                bool findtex = false;
                foreach (var texname in pnames)
                {
                    Texture tex = material.GetTexture(texname);

                    if (tex)
                    {
                        string texpath = AssetDatabase.GetAssetPath(tex);

                        if (!string.IsNullOrEmpty(texpath))
                        {
                            string texBundleName = AssetPathToBundleName(texpath);
                            if (texBundleName == depBundleName)
                            {
                                ECAssetBundleHeader.DepInfo info = new ECAssetBundleHeader.DepInfo();
                                info.name = texname;
                                info.path = depBundleName+".u3dext";
                                deplist.Add(info);
                                findtex = true;
                            }
                        }
                    }
                }

                if (!findtex)
                {
                    ECAssetBundleHeader.DepInfo info = new ECAssetBundleHeader.DepInfo();
                    info.name = "";
                    info.path = depBundleName+".u3dext";
                    deplist.Add(info);
                }
            }

            return deplist.ToArray();
		}
		else
		{
			return CalcNormalDeps(assetBundleName, material, manifest);
		}
	}

	private static void DetermineAssetBundleOption(AssetBundleManifest manifest, string assetBundleName, string cacheDir, out bool needCompress, out ECAssetBundleHeader header)
	{
		Object asset = AssetDatabase.LoadMainAssetAtPath("assets/" + assetBundleName);

		needCompress = false;
		header = new ECAssetBundleHeader();

		//确定 needCompress, SepFile
		if (asset is Font)
		{
			header.option |= ECAssetBundleHeader.BundleOption.SepFile;
		}
		else
		{
			FileInfo fi = new FileInfo(cacheDir + "/uncompressed/assets/" + assetBundleName);

			if (asset is AudioClip)
			{
				if (fi.Length > min_audio_clip_bundel_size)
					header.option |= ECAssetBundleHeader.BundleOption.SepFile;
			}
			else
			{
				if (fi.Length > min_compress_bundle_size)
					needCompress = true;
			}
		}

		//确定其他选项
		string[] depBundleNames = manifest.GetDirectDependencies(assetBundleName);
		System.Array.Sort(depBundleNames);
        if (asset is Material)
        {
            header.specialType = ECAssetBundleHeader.BundleSpecialType.Material;

            header.deps = CalcMaterialDeps(assetBundleName, asset as Material, manifest);
        }
		else
		{
			if (asset is Texture)
			{
				header.option |= ECAssetBundleHeader.BundleOption.ManuallyResolve;
				header.specialType = ECAssetBundleHeader.BundleSpecialType.Texture;
			}
			else if (asset is Shader)
			{
				header.option |= ECAssetBundleHeader.BundleOption.DonotRelease;
			}
			else if (asset is MonoScript)
			{
				header.option |= ECAssetBundleHeader.BundleOption.DonotRelease;
			}
			else if (asset is Font)
			{
				header.option |= ECAssetBundleHeader.BundleOption.DonotRelease;
			}

			header.deps = CalcNormalDeps(assetBundleName, asset, manifest);
		}
	}

	private static string UnifyPath(string path)
	{
		return path.ToLower().Replace('\\', '/');
	}

	private static void CollectSeperateAssetBundleBuilds(string assetPath, Dictionary<string, AssetBundleBuild> buildsMap)
	{
		if (!CollectAssetBuildBuildNoDependencies(assetPath, buildsMap))
			return;

		string[] deppaths = GetDependencies(assetPath);
		foreach (var deppath in deppaths)
		{
			CollectAssetBuildBuildNoDependencies(deppath, buildsMap);
		}
	}

	private static void CollectOneAssetBundleBuilds(string assetPath, Dictionary<string, AssetBundleBuild> buildsMap)
	{
		if (!CollectAssetBuildBuildNoDependencies(assetPath, buildsMap))
			return;

		string[] deppaths = GetDependencies(assetPath);
		foreach (var deppath in deppaths)
		{
			if (buildsMap.ContainsKey(UnifyPath(deppath)))	//快速排除
				continue;

			Object dep = AssetDatabase.LoadMainAssetAtPath(deppath);
            if (dep is Texture || dep is Shader || dep is MonoScript || dep is Font)		//这些类型不打到一个包中
            {
				CollectSeperateAssetBundleBuilds(assetPath, buildsMap);
			}
		}
	}

	/// <summary>
	/// 收集单一 Asset 不考虑其依赖
	/// </summary>
	/// <param name="assetPath"></param>
	/// <param name="buildsMap"></param>
	/// <returns>是否需收集，false 表示跳过</returns>
	private static bool CollectAssetBuildBuildNoDependencies(string assetPath, Dictionary<string, AssetBundleBuild> buildsMap)
	{
		string assetPathUnified = UnifyPath(assetPath);
		if (buildsMap.ContainsKey(assetPathUnified))
			return false;

		Object asset = AssetDatabase.LoadMainAssetAtPath(assetPath);
		if (asset is MonoScript || assetPath.EndsWith(".cs"))
			return false;

		AssetBundleBuild build = new AssetBundleBuild();
		build.assetBundleName = AssetPathToBundleName(assetPathUnified);
		build.assetNames = new string[] { assetPath };
		buildsMap[assetPathUnified] = build;

		return true;
	}

    public static void BuildToOneBundle(Object realass, string path, bool recursive)
    {
        if (CheckModify && recursive)
        {
            if (!CheckAssetModify(realass, path, true))
                return;
        }

        int AssetCount = 0;
        List<ECAssetBundleHeader.DepInfo> ls = new List<ECAssetBundleHeader.DepInfo>();
        string asspath = AssetDatabase.GetAssetPath(realass);

        if (recursive)
        {
            string[] deppaths = AssetDatabase.GetDependencies(new string[] { asspath });
            Dictionary<string, bool> realdeps = new Dictionary<string, bool>();
            foreach (string realdep in deppaths)
            {
                realdeps[realdep] = true;
            }

            Object[] depobjs = EditorUtility.CollectDependencies(new Object[] { realass });
            BuildPipeline.PushAssetDependencies();
            AssetCount++;

            for (int i = 0; i < depobjs.Length; i++)
            {
                Object dep = depobjs[i];
                if (ReferenceEquals(dep, realass))
                    continue;

                string texpath = AssetDatabase.GetAssetPath(dep);
                if (!realdeps.ContainsKey(texpath))
                    continue;

                if (dep is Texture || dep is Shader || dep is MonoScript || dep is Font)
                {
                    if (!texpath.ToLower().EndsWith(".asset") && !string.IsNullOrEmpty(texpath))
                    {
                        BuildToOneBundle(dep, path, false);
                        ECAssetBundleHeader.DepInfo info = new ECAssetBundleHeader.DepInfo();
                        info.name = "";
                        string realname = ConvertFileName(texpath) + ".u3dext";
                        info.path = realname.ToLower();
                        ls.Add(info);
                    }
                }
            }

            BuildPipeline.PushAssetDependencies();
            AssetCount++;
        }

        string assetpath = ConvertFileName(Path.GetDirectoryName(asspath));
        string outpath;
        if (asspath != "")
        {
            outpath = path + "/" + assetpath;
        }
        else
            outpath = assetpath;

        Directory.CreateDirectory(outpath);
        string guid = AssetDatabase.AssetPathToGUID(asspath);
        string outfile = outpath + "/" + guid;

        BuildAssetBundleOptions option;
        option = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.CollectDependencies;
        option |= BuildAssetBundleOptions.UncompressedAssetBundle;

        bool suc = BuildPipeline.BuildAssetBundleExplicitAssetNames(new Object[] { realass }, new string[] { "1" }, outfile,
                            option, ExportAssetBundles.CurBuildTarget);

        // do not compress font
        bool ForceSep = realass is Font;
        bool NeedSep = false;
        if (suc && !ForceSep)
        {
            FileInfo fi = new FileInfo(outfile);

            if (realass is AudioClip)
            {
                if (fi.Length > min_audio_clip_bundel_size)
                    NeedSep = true;
            }
            else if (fi.Length > min_compress_bundle_size)
            {
                option = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.CollectDependencies;
                suc = BuildPipeline.BuildAssetBundleExplicitAssetNames(new Object[] { realass }, new string[] { "1" }, outfile,
                                    option, ExportAssetBundles.CurBuildTarget);
                Debug.LogWarning("Big bundle: " + outfile + " Origin Size: " + fi.Length);
            }
        }

        for (int n = 0; n < AssetCount; n++)
            BuildPipeline.PopAssetDependencies();

        if (suc)
        {
            byte[] content = File.ReadAllBytes(outfile);
            string outfile2 = outpath + "/" + Path.GetFileName(asspath) + ".u3dext";
            using (FileStream fs = File.Open(outfile2, FileMode.Create, FileAccess.Write))
            {
                ECAssetBundleHeader bh = new ECAssetBundleHeader();
                if (ForceSep || NeedSep)
                    bh.option |= ECAssetBundleHeader.BundleOption.SepFile;
                if (realass is Shader || realass is MonoScript || realass is Font)
                    bh.option |= ECAssetBundleHeader.BundleOption.DonotRelease;
                bh.deps = ls.ToArray();
                bh.Save(fs);
                fs.Write(content, 0, content.Length);
            }

            File.Delete(outfile);
        }
        else
            Debug.LogError("BuildAssetBundleExplicitAssetNames");
    }

    public static string ConvertFileName(string assetpath)
    {
        if (assetpath == "Assets")
            return "";

        if (assetpath.StartsWith("Assets/"))
            return assetpath.Substring("Assets/".Length);

        return assetpath;
    }

    static bool CheckAssetModify(Object ass, string path, bool isOneBundle)
    {
        string rootpath = AssetDatabase.GetAssetPath(ass);
        string lowpath = rootpath.ToLower();

        if (!ResPathMap.ContainsKey(lowpath))
            ResPathMap[lowpath] = true;
        else
            return false;

        if (string.IsNullOrEmpty(rootpath) || !File.Exists(rootpath))
            return true;

        string outpathroot = path + "/" + ConvertFileName(Path.GetDirectoryName(rootpath));
        string outroot = outpathroot + "/" + Path.GetFileName(rootpath) + ".u3dext";

        if (!File.Exists(outroot))
            return true;

        System.DateTime dtroot = File.GetLastWriteTime(outroot);
        string[] deppaths = AssetDatabase.GetDependencies(new string[] { rootpath });
        Dictionary<string, bool> realdeps = new Dictionary<string, bool>();
        foreach (string realdep in deppaths)
        {
            realdeps[realdep] = true;
        }
        Object[] depobjs = EditorUtility.CollectDependencies(new Object[] { ass });

        foreach (Object depobj in depobjs)
        {
            string deppath = AssetDatabase.GetAssetPath(depobj);

            if (string.IsNullOrEmpty(deppath) || !File.Exists(deppath))
                continue;

            if (!realdeps.ContainsKey(deppath))
                continue;

            string outpath = path + "/" + ConvertFileName(Path.GetDirectoryName(deppath));
            string outfile2 = outpath + "/" + Path.GetFileName(deppath) + ".u3dext";

            string metapath = deppath + ".meta";
            if (!File.Exists(metapath))
                return true;

            System.DateTime dtext;
            if (isOneBundle)
            {
                if (depobj == ass)
                    dtext = dtroot;
                else if (!deppath.ToLower().EndsWith(".asset") && (depobj is Texture || depobj is Font || depobj is Shader || depobj is MonoScript))
                {
                    if (!File.Exists(outfile2))
                        return true;

                    dtext = File.GetLastWriteTime(outfile2);
                }
                else
                    dtext = dtroot;
            }
            else if (!File.Exists(outfile2))
                return true;
            else
            {
                dtext = File.GetLastWriteTime(outfile2);
            }

            System.DateTime dtmeta = File.GetLastWriteTime(metapath);
            System.DateTime dtsrc = File.GetLastWriteTime(deppath);

            if (dtsrc >= dtext || dtmeta >= dtext)
            {
                return true;
            }
            else if (depobj != ass)
            {
                if (dtsrc >= dtroot || dtmeta >= dtroot)
                    return true;
            }
        }

        return false;
    }

    static Dictionary<string, int> _depmap = new Dictionary<string, int>();
    public static Dictionary<string, int> _totaldepmap = new Dictionary<string, int>();
    public static void BuildOne(Object ass, string path)
    {
        string asspath = AssetDatabase.GetAssetPath(ass);

        if (string.IsNullOrEmpty(asspath))
            return;

        if (CheckModify)
        {
            if (!CheckAssetModify(ass, path, false))
                return;
        }

        _depmap.Clear();

        string[] deppaths = AssetDatabase.GetDependencies(new string[] { asspath });

        Dictionary<string, string[]> allobjdeps = new Dictionary<string, string[]>();
        Dictionary<string, List<string>> allobjchild = new Dictionary<string, List<string>>();
        Dictionary<string, int> objlevel = new Dictionary<string, int>();
        allobjdeps[asspath] = deppaths;
        objlevel[asspath] = 0;
        foreach (string dep in deppaths)
        {
            // Debug.Log("ass path: " + dep);

            if (dep == asspath)
                continue;

            string[] subdeps = AssetDatabase.GetDependencies(new string[] { dep });
            allobjdeps[dep] = subdeps;
        }

        foreach (KeyValuePair<string, string[]> kv in allobjdeps)
        {
            string k = kv.Key;
            string[] kdeps = allobjdeps[k];
            objlevel.Clear();
            foreach (string subdep in kdeps)
            {
                if (subdep == k)
                    continue;
                string[] ksubdeps = allobjdeps[subdep];
                foreach (string ksubdep in ksubdeps)
                {
                    if (ksubdep == subdep)
                        continue;

                    objlevel[ksubdep] = 1;
                }
            }
            List<string> list = null;
            allobjchild[k] = null;
            foreach (string subdep in kdeps)
            {
                if (subdep == k || objlevel.ContainsKey(subdep))
                    continue;
                if (list == null)
                {
                    list = new List<string>();
                    allobjchild[k] = list;
                }
                list.Add(subdep);
            }
        }

        {
            Dictionary<string, int> objmark = new Dictionary<string, int>();
            int depcount = 0;

            foreach (KeyValuePair<string, List<string>> kvexp in allobjchild)
            {
                BuildOneAssetBundle(kvexp.Key, objmark, allobjchild, path, ref depcount);
            }

            for (int i = 0; i < depcount; i++)
                BuildPipeline.PopAssetDependencies();
        }

        foreach (KeyValuePair<string, int> k in _depmap)
        {
            int count;
            if (_totaldepmap.TryGetValue(k.Key, out count))
            {
                _totaldepmap[k.Key] = count + 1;
            }
            else
                _totaldepmap[k.Key] = 1;
        }
    }

    static void BuildOneAssetBundle(string expcur, Dictionary<string, int> objmark, Dictionary<string, List<string>> allobjchild, string path, ref int depcount)
    {
        // 假设已经输出则跳过
        if (objmark.ContainsKey(expcur))
            return;

        List<string> subs = allobjchild[expcur];

        if (subs != null)
        {
            foreach (string sub in subs)
            {
                // 假设已经输出则跳过
                if (objmark.ContainsKey(sub))
                    continue;

                BuildOneAssetBundle(sub, objmark, allobjchild, path, ref depcount);
            }
        }

        objmark[expcur] = 1;

        string asspath = expcur;

        if (string.IsNullOrEmpty(asspath))
            return;

        string guid = AssetDatabase.AssetPathToGUID(asspath);
        string assetpath = ConvertFileName(Path.GetDirectoryName(asspath));
        string outpath;
        if (asspath != "")
        {
            outpath = path + "/" + assetpath;
        }
        else
            outpath = assetpath;

        Directory.CreateDirectory(outpath);

        //string outfile = Path.Combine(outpath, Path.GetFileNameWithoutExtension(asspath));
        string outfile = outpath + "/" + guid;

        BuildPipeline.PushAssetDependencies();
        depcount++;
        Object realass = AssetDatabase.LoadMainAssetAtPath(expcur);

        if (realass == null)
            return;

        Object[] depobjs = EditorUtility.CollectDependencies(new Object[] { realass });
        Dictionary<string, int> deppathmap = new Dictionary<string, int>();

        foreach (Object depobj in depobjs)
        {
            string deppath = AssetDatabase.GetAssetPath(depobj);

            if (string.IsNullOrEmpty(deppath))
                continue;

            deppathmap[deppath] = 1;
        }

        List<string> realsubs = new List<string>();
        if (subs != null)
        {
            foreach (string sub in subs)
            {
                if (deppathmap.ContainsKey(sub))
                {
                    string realname = ConvertFileName(sub) + ".u3dext";
                    realsubs.Add(realname.ToLower());
                }
            }
        }

        BuildAssetBundleOptions option;
        option = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.CollectDependencies;
        option |= BuildAssetBundleOptions.UncompressedAssetBundle;

        bool suc = BuildPipeline.BuildAssetBundleExplicitAssetNames(new Object[] { realass }, new string[] { "1" }, outfile,
                            option, ExportAssetBundles.CurBuildTarget);

        Debug.Log("src file: " + asspath + " " + depcount);

        if (/*realass is MonoScript || */!deppathmap.ContainsKey(expcur))
        {
            File.Delete(outfile);
        }
        else if (suc)
        {
            // do not compress font
            bool ForceSep = realass is Font;
            bool NeedSep = false;
            if (!ForceSep)
            {
                FileInfo fi = new FileInfo(outfile);

                if (realass is AudioClip)
                {
                    if (fi.Length > min_audio_clip_bundel_size)
                        NeedSep = true;
                }
                else if (fi.Length > min_compress_bundle_size)
                {
                    option = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.CollectDependencies;
                    suc = BuildPipeline.BuildAssetBundleExplicitAssetNames(new Object[] { realass }, new string[] { "1" }, outfile,
                                        option, ExportAssetBundles.CurBuildTarget);
                    Debug.LogWarning("Big bundle: " + outfile + " Origin Size: " + fi.Length);
                }
            }

            byte[] content = File.ReadAllBytes(outfile);
            string outfile2 = outpath + "/" + Path.GetFileName(asspath) + ".u3dext";
            _depmap[outfile2.ToLower()] = 1;
			var oldFileInfo = new FileInfo(outfile2);
			if (oldFileInfo.Exists)
				oldFileInfo.IsReadOnly = false;
            using (FileStream fs = File.Open(outfile2, FileMode.Create, FileAccess.Write))
            {
                ECAssetBundleHeader bh = new ECAssetBundleHeader();
                bool bDefault = true;
                if (ForceSep || NeedSep)
                    bh.option |= ECAssetBundleHeader.BundleOption.SepFile;

                if (realass is Material)
                {
                    bh.specialType = ECAssetBundleHeader.BundleSpecialType.Material;

                    Material mt = realass as Material;

                    if (mt.shader != null)
                    {
                        bDefault = false;
                        List<string> pnames = new List<string>();
                        int nPCount = ShaderUtil.GetPropertyCount(mt.shader);

                        for (int n = 0; n < nPCount; n++)
                        {
                            ShaderUtil.ShaderPropertyType spt = ShaderUtil.GetPropertyType(mt.shader, n);

                            if (spt == ShaderUtil.ShaderPropertyType.TexEnv)
                            {
                                string pn = ShaderUtil.GetPropertyName(mt.shader, n);
                                pnames.Add(pn);
                            }
                        }

                        List<ECAssetBundleHeader.DepInfo> deplist = new List<ECAssetBundleHeader.DepInfo>();
                        foreach (var realsub in realsubs)
                        {
                            bool findtex = false;
                            foreach (var texname in pnames)
                            {
                                Texture tex = mt.GetTexture(texname);

                                if (tex)
                                {
                                    string texpath = AssetDatabase.GetAssetPath(tex);

                                    if (!string.IsNullOrEmpty(texpath))
                                    {
                                        string realpath = ConvertFileName(texpath) + ".u3dext";
                                        realpath = realpath.ToLower();

                                        if (realpath == realsub)
                                        {
                                            ECAssetBundleHeader.DepInfo info = new ECAssetBundleHeader.DepInfo();
                                            info.name = texname;
                                            info.path = realsub;
                                            deplist.Add(info);
                                            findtex = true;
                                        }
                                    }
                                }
                            }

                            if (!findtex)
                            {
                                ECAssetBundleHeader.DepInfo info = new ECAssetBundleHeader.DepInfo();
                                info.name = "";
                                info.path = realsub;
                                deplist.Add(info);
                            }
                        }

                        bh.deps = deplist.ToArray();
                    }
                }
                else if (realass is Texture)
                {
                    bh.option |= ECAssetBundleHeader.BundleOption.ManuallyResolve;
                    bh.specialType = ECAssetBundleHeader.BundleSpecialType.Texture;
                }
                else if (realass is Shader)
                {
                    bh.option |= ECAssetBundleHeader.BundleOption.DonotRelease;
                }
                else if (realass is MonoScript)
                {
                    bh.option |= ECAssetBundleHeader.BundleOption.DonotRelease;
                }
                else if (realass is Font)
                {
                    bh.option |= ECAssetBundleHeader.BundleOption.DonotRelease;
                }

                if (bDefault)
                {
                    bh.deps = new ECAssetBundleHeader.DepInfo[realsubs.Count];

                    for (int n = 0; n < realsubs.Count; n++)
                    {
                        bh.deps[n].name = "";
                        bh.deps[n].path = realsubs[n];
                    }
                }

                bh.Save(fs);
                fs.Write(content, 0, content.Length);
            }

            File.Delete(outfile);
        }
    }

    [MenuItem("Assets/输出装备文件")]
    static void ExportEquip()
    {
        _ExportEquip();
    }

    [MenuItem("Assets/输出动作文件")]
    static void ExportAnim()
    {
        string targetPath = Application.dataPath + "/AnimationClip";
        if (!Directory.Exists(targetPath))
        {
            Directory.CreateDirectory(targetPath);
        }

        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.Unfiltered);

        foreach (Object Asset in selection)
        {
            AnimationClip newClip = new AnimationClip();
            EditorUtility.CopySerialized(Asset, newClip);
            AssetDatabase.CreateAsset(newClip, "Assets/AnimationClip/" + Asset.name + ".anim");
        }
    }

    static void _ExportEquip()
    {
        // Bring up save panel
        string path = EditorUtility.SaveFolderPanel("Save Resource", "", "");
        if (path.Length != 0)
        {
            // Build the resource file from the active selection.
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

			AssetBundleBuildParamCollecter bundleCollector = new AssetBundleBuildParamCollecter();
            foreach (Object sel in selection)
            {
                BuildOneEquip(sel, path, bundleCollector);
            }
			BuildAssetBundles(path, bundleCollector.ToArray());
            Selection.objects = selection;
        }
    }


    public static void BuildOneEquipByPath( string asspath, string path, AssetBundleBuildParamCollecter bundleCollector )
    {
        string meshassetname = asspath + ".asset";
        Object meshasset = AssetDatabase.LoadMainAssetAtPath(asspath);
        if (meshasset == null)
        {
            return;
        }

        GameObject inst = (GameObject)GameObject.Instantiate(meshasset);
        SkinnedMeshRenderer smr = inst.GetComponentInChildren<SkinnedMeshRenderer>();

        if (smr == null)
        {
            Object.DestroyImmediate(inst);
            return;
        }

        if (smr.sharedMesh == null)
        {
            Object.DestroyImmediate(inst);
            return;
        }


        bundleCollector.Add(meshassetname);

        string str;
        str = "return {\n";
        str += "File = {\n";
        string strTemp = ConvertFileName(meshassetname) + ".u3dext";
        str += "\"" + strTemp + "\",\n";

        Material[] mats = smr.sharedMaterials;
        if (mats != null)
        {
            foreach (Material mat in mats)
            {
                if (mat)
                {
                    string matAssetPath = AssetDatabase.GetAssetPath(mat);
                    bundleCollector.Add(matAssetPath);
                    strTemp = matAssetPath;
                    strTemp = ConvertFileName(strTemp) + ".u3dext";
                    str += "\"" + strTemp + "\",\n";
                }
            }
        }
        str += "},\n";

        str += "Bones = {\n";
        for (int i = 0; i < smr.bones.Length; i++)
        {
            GameObject go = smr.bones[i].gameObject;
            str += "\"" + go.name + "\",\n";
        }
        str += "}\n";

        str += "}";
        Object.DestroyImmediate(inst);
        strTemp = ConvertFileName(asspath);
        strTemp = path + "/" + strTemp + ".lua";
        Directory.CreateDirectory(Path.GetDirectoryName(strTemp));
        File.WriteAllBytes(strTemp, System.Text.Encoding.UTF8.GetBytes(str));
    }

    public static void BuildOneEquip(Object ass, string path, AssetBundleBuildParamCollecter bundleCollector)
    {
        GameObject obj = ass as GameObject;

        if (obj == null)
            return;

        GameObject inst = (GameObject)GameObject.Instantiate(obj);
        SkinnedMeshRenderer smr = inst.GetComponentInChildren<SkinnedMeshRenderer>();

        if (smr == null)
        {
            Object.DestroyImmediate(inst);
            return;
        }

        if (smr.sharedMesh == null)
        {
            Object.DestroyImmediate(inst);
            return;
        }

        string asspath = AssetDatabase.GetAssetPath(ass);
        string meshassetname = asspath + ".asset";
        Object meshasset = AssetDatabase.LoadMainAssetAtPath(meshassetname);

        if (meshasset == null)
        {
            Object.DestroyImmediate(inst);
            return;
        }

		bundleCollector.Add(meshassetname);

        string lowMeshAssertName = meshassetname.Replace("_m_", "_l_");

        string str;
        str = "return {\n";
        str += "File = {\n";
        string strTemp = ConvertFileName(meshassetname) + ".u3dext";
        str += "\"" + strTemp + "\",\n";



        Material[] mats = smr.sharedMaterials;
        if (mats != null)
        {
            foreach (Material mat in mats)
            {
                if (mat)
                {
					string matAssetPath = AssetDatabase.GetAssetPath(mat);
					bundleCollector.Add(matAssetPath);
                    strTemp = matAssetPath;
                    strTemp = ConvertFileName(strTemp) + ".u3dext";
                    str += "\"" + strTemp + "\",\n";
                }
            }
        }
        str += "},\n";

        str += "LowMesh = {\n";
        str += "\"" + ConvertFileName(lowMeshAssertName) + ".u3dext" + "\"";
        str += "},\n";

        str += "Bones = {\n";
        for (int i = 0; i < smr.bones.Length; i++)
        {
            GameObject go = smr.bones[i].gameObject;
            str += "\"" + go.name + "\",\n";
        }
        str += "}\n";

        str += "}";
        Object.DestroyImmediate(inst);
        strTemp = ConvertFileName(asspath);
        strTemp = path + "/" + strTemp + ".lua";
		Directory.CreateDirectory(Path.GetDirectoryName(strTemp));
        File.WriteAllBytes(strTemp, System.Text.Encoding.UTF8.GetBytes(str));
    }
}

#endif
