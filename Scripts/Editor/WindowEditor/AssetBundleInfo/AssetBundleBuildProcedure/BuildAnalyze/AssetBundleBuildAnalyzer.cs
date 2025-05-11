/*------------------------------
 * 脚本名称: AssetBundleBuildAnalyze
 * 创建者: movin
 * 创建日期: 2025/04/13
------------------------------*/

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// AB包构建分析器
    /// </summary>
    public class AssetBundleBuildAnalyzer
    {
        /// <summary>
        /// 所有文件和所属构建信息的对应关系
        /// </summary>
        private Dictionary<string, AssetBundleBuildAnalyzeInfo> _fileAnalyzeInfoDic
            = new Dictionary<string, AssetBundleBuildAnalyzeInfo>();

        /// <summary>
        /// 构建分析器
        /// </summary>
        /// <param name="fileCollectors"></param>
        public AssetBundleBuildAnalyzer(List<AssetBundleInfoCollector> fileCollectors)
        {
            _fileAnalyzeInfoDic.Clear();
            foreach (var collector in fileCollectors)
            {
                if (collector.infoType == ECollectorInfoType.EachFile
                    || collector.infoType == ECollectorInfoType.CopyFiles)
                {
                    foreach (var file in collector.collectedFiles)
                    {
                        var guid = AssetDatabase.AssetPathToGUID(file);
                        if (!_fileAnalyzeInfoDic.ContainsKey(guid))
                        {
                            var info = new AssetBundleBuildAnalyzeInfo(collector.infoType, true, file);
                            info.AddFile(guid);
                            _fileAnalyzeInfoDic.Add(guid, info);
                        }
                    }
                }
                else if (collector.infoType == ECollectorInfoType.WholeFolderWithAnalyze
                         || collector.infoType == ECollectorInfoType.WholeFolderWithoutAnalyze)
                {
                    var info = new AssetBundleBuildAnalyzeInfo(collector.infoType, true, collector.folderPath);
                    foreach (var file in collector.collectedFiles)
                    {
                        var guid = AssetDatabase.AssetPathToGUID(file);
                        _fileAnalyzeInfoDic.TryAdd(guid, info);
                        info.AddFile(guid);
                    }
                }
            }
            
            // 添加Shader变体材质
            var materialPath = GameEntry.GlobalGameConfig.resourceConfig.extraShaderVariantMaterialPath;
            var guids = AssetDatabase.FindAssets("t:Material", new[] {materialPath});
            foreach (var guid in guids)
            {
                if (!_fileAnalyzeInfoDic.ContainsKey(guid))
                {
                    var filePath = AssetDatabase.GUIDToAssetPath(guid);
                    var info = new AssetBundleBuildAnalyzeInfo(ECollectorInfoType.EachFile, true, filePath);
                    info.AddFile(guid);
                    _fileAnalyzeInfoDic.Add(guid, info);
                }
            }
        }

        /// <summary>
        /// 分析
        /// </summary>
        /// <param name="newInfoLimit">新的引用信息的引用数量阈值</param>
        public void Analyze(int newInfoLimit)
        {
            var files = _fileAnalyzeInfoDic.Values
                .ToHashSet()    // 去重处理
                .SelectMany(x => x.GetAllFileList())
                .ToArray();
            foreach (var file in files)
            {
                // 获取所有引用信息
                var referencedFiles = AssetDatabase.GetDependencies(AssetDatabase.GUIDToAssetPath(file), true);
                foreach (var referencedFile in referencedFiles)
                {
                    var referencedFileGuid = AssetDatabase.AssetPathToGUID(referencedFile);
                    // 代码文件或文件本身,跳过
                    if (referencedFile.EndsWith(".cs") || file.Equals(referencedFileGuid))
                    {
                        continue;
                    }
                    if (!_fileAnalyzeInfoDic.TryGetValue(referencedFileGuid, out var fileInfo))
                    {
                        fileInfo = new AssetBundleBuildAnalyzeInfo(
                            ECollectorInfoType.EachFile, 
                            false, 
                            referencedFile);
                        _fileAnalyzeInfoDic.Add(referencedFileGuid, fileInfo);
                    }
                    fileInfo.AddFile(referencedFileGuid);
                    // 分析文件,并修改文件引用信息
                    var newInfo = fileInfo.CheckFile(referencedFileGuid, newInfoLimit);
                    if (null != newInfo)
                    {
                        _fileAnalyzeInfoDic[referencedFileGuid] = newInfo;
                    }
                }
            }
        }
        
        /// <summary>
        /// 获取构建列表
        /// </summary>
        /// <param name="assetBundleLimit">构建数量限制</param>
        /// <returns></returns>
        public AssetBundleBuild[] GetBuildArray(int assetBundleLimit)
        {
            var result = new List<AssetBundleBuild>();
            var allInfos = _fileAnalyzeInfoDic.Values
                .ToHashSet();
            foreach (var info in allInfos)
            {
                if (info.TryGetAssetBundleBuild(assetBundleLimit - 1, out var build))
                {
                    result.Add(build);
                }
            }
            return result.ToArray();
        }
    }
}