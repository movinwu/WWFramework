/*------------------------------
 * 脚本名称: RemoteAssetBundleManager
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
    /// 远端AB资源管理器
    /// </summary>
    public class RemoteAssetBundleManager : AssetBundleManager
    {
        protected override async UniTask<AssetBundle> LoadAssetBundle(AssetBundleData assetBundleData)
        {
            // 本地持久化路径（可序列化文件夹）
            string localPath = Path.Combine(
                Application.persistentDataPath, 
                GameEntry.GlobalGameConfig.resourceConfig.abLocalPath,
                assetBundleData.Name);
            
            // 优先检查本地缓存
            if (File.Exists(localPath))
            {
                var request = AssetBundle.LoadFromFileAsync(localPath);
                await UniTask.WaitUntil(() => request.isDone);
                if (null != request.assetBundle)
                {
                    return request.assetBundle;
                }
            }

            // 从远端加载
            string remoteUrl = $"http://your-server.com/{assetBundleData.Name}";
            using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(remoteUrl))
            {
                await request.SendWebRequest();
                
                if (request.result == UnityWebRequest.Result.Success)
                {
                    // 缓存到本地
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
                    if (null != bundle)
                    {
                        return bundle;
                    }
                }
            }

            return null;
        }
    }
}