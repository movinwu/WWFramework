/*------------------------------
 * 脚本名称: BuildAnalyzeInfo
 * 创建者: movin
 * 创建日期: 2025/04/10
------------------------------*/

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// AB包分析信息
    /// </summary>
    public class AssetBundleBuildAnalyzeInfo
    {
        /// <summary>
        /// 所有文件列表及被引用计数
        /// </summary>
        private Dictionary<string, int> _allFileDic
            = new Dictionary<string, int>();

        /// <summary>
        /// 文件收集类型
        /// </summary>
        private ECollectorInfoType _collectorInfoType;

        /// <summary>
        /// 是否是初始就存在的文件信息
        /// <para> 非初始存在的文件信息(在分析AB包过程中添加的文件信息),如果文件的被引用计数没有达到阈值,不会被打包 </para>
        /// </summary>
        private bool _initialInfo;

        /// <summary>
        /// 包路径
        /// </summary>
        private string _bundlePath;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="collectorInfoType">收集类型</param>
        /// <param name="initialInfo">初始化信息</param>
        /// <param name="bundlePath">包路径</param>
        public AssetBundleBuildAnalyzeInfo(ECollectorInfoType collectorInfoType, bool initialInfo, string bundlePath)
        {
            _initialInfo = initialInfo;
            _collectorInfoType = collectorInfoType;
            _bundlePath = bundlePath;
        }
        
        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="fileGuid">新文件</param>
        /// <param name="count">文件引用计数</param>
        public void AddFile(string fileGuid, int count = 1)
        {
            _allFileDic.TryAdd(fileGuid, 0);
            _allFileDic[fileGuid] += count;
        }

        /// <summary>
        /// 检查文件是否需要分裂
        /// </summary>
        /// <param name="fileGuid">文件guid</param>
        /// <param name="newInfoLimit">新文件分裂阈值</param>
        /// <returns>新的构建信息</returns>
        public AssetBundleBuildAnalyzeInfo CheckFile(string fileGuid, int newInfoLimit)
        {
            // 从整个文件夹全部打包类型的信息中分裂出被频繁引用的单个文件单独打包
            if (this._collectorInfoType == ECollectorInfoType.WholeFolderWithAnalyze)
            {
                if (_allFileDic.TryGetValue(fileGuid, out int count))
                {
                    if (count >= newInfoLimit)
                    {
                        // 创建新的构建信息
                        AssetBundleBuildAnalyzeInfo newInfo
                            = new AssetBundleBuildAnalyzeInfo(
                                ECollectorInfoType.EachFile, 
                                _initialInfo, 
                                AssetDatabase.GUIDToAssetPath(fileGuid));
                        // 添加文件
                        newInfo.AddFile(fileGuid, count);
                        // 从当前文件列表中移除文件
                        _allFileDic.Remove(fileGuid);
                        // 返回新的构建信息
                        return newInfo;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 获取所有文件信息
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllFileList()
        {
            return _allFileDic.Keys.ToList();
        }

        /// <summary>
        /// 获取构建信息
        /// </summary>
        /// <param name="assetBundleLimit">构建ab包被引用次数阈值</param>
        /// <param name="build"></param>
        /// <returns></returns>
        public bool TryGetAssetBundleBuild(int assetBundleLimit, out AssetBundleBuild build)
        {
            build = new AssetBundleBuild();
            // 对于初始化的文件信息,无需任何判断直接添加
            if (_initialInfo)
            {
                // 不是拷贝文件,才需要构建
                if (_collectorInfoType != ECollectorInfoType.CopyFiles)
                {
                    GenerateBuild(ref build);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // 对于非初始化的文件信息,需要判断文件引用计数是否达到阈值
            var limit = assetBundleLimit;
            foreach (var info in _allFileDic)
            {
                limit -= info.Value - 1;
            }

            if (limit <= 0)
            {
                GenerateBuild(ref build);
                return true;
            }

            return false;

            // 装配ab构建信息
            void GenerateBuild(ref AssetBundleBuild build)
            {
                // 包名(路径名作为包名)
                build.assetBundleName = ResourceHelper.PathToAssetBundleName(_bundlePath);
                // 后缀
                build.assetBundleVariant = string.Empty;
                // 文件列表
                var files = GetAllFileList()
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .ToArray();
                // 添加文件
                build.assetNames = files.ToArray();
                build.addressableNames = files.ToArray();
            }
        }
    }
}