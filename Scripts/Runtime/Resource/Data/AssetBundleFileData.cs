/*------------------------------
 * 脚本名称: AssetBundleFileData
 * 创建者: movin
 * 创建日期: 2025/04/30
------------------------------*/

using Cysharp.Threading.Tasks;

namespace WWFramework
{
    /// <summary>
    /// 单个资源文件数据
    /// </summary>
    public class AssetBundleFileData : AssetFileData
    {
        /// <summary>
        /// AB包数据
        /// </summary>
        private readonly AssetBundleData _assetBundleData;
        
        public AssetBundleFileData(AssetBundleData assetBundleData, string assetPath) : base(assetPath)
        {
            _assetBundleData = assetBundleData;
        }

        /// <inheritdoc />
        protected override async UniTask<T> RealLoadAsset<T>()
        {
            return await _assetBundleData.AssetBundle.LoadAssetAsync(AssetPath, typeof(T)) as T;
        }
    }
}