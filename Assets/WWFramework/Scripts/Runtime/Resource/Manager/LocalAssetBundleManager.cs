/*------------------------------
 * 脚本名称: LocalAssetBundleManager
 * 创建者: movin
 * 创建日期: 2025/05/02
------------------------------*/

using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace WWFramework
{
    /// <summary>
    /// 本地AB包管理器
    /// </summary>
    public class LocalAssetBundleManager : AssetBundleManager
    {
        protected override async UniTask<AssetBundle> LoadAssetBundle(AssetBundleData assetBundleData)
        {
            // 从StreamingAssets路径下加载
            var name = assetBundleData.Name;
            // 拼接路径 TODO 根据不同平台进行适配
            var path = Path.Combine(Application.streamingAssetsPath, GameEntry.GlobalGameConfig.resourceConfig.abLocalPath, name);
            // 处理Android平台特殊路径
#if UNITY_ANDROID && !UNITY_EDITOR
            path = "file://" + path;
#endif
            // 使用UnityWebRequest加载StreamingAssets下的AB包
            var request = UnityWebRequestAssetBundle.GetAssetBundle(path);
            await request.SendWebRequest();
            var assetBundle = DownloadHandlerAssetBundle.GetContent(request);
            return assetBundle;
        }
    }
}