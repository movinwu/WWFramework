/*------------------------------
 * 脚本名称: ResourceHelp
 * 创建者: movin
 * 创建日期: 2025/04/30
------------------------------*/

namespace WWFramework
{
    /// <summary>
    /// 资源辅助类
    /// </summary>
    public static class ResourceHelper
    {
        /// <summary>
        /// 文件或文件夹路径转AssetBundle路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string PathToAssetBundlePath(string path)
        {
            return path
                .Replace("Assets/", string.Empty)
                .Replace('.', '+')
                .Replace('/', '-')
                .Replace('\\', '-');
        }

        /// <summary>
        /// AssetBundle路径转文件或文件夹路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string AssetBundlePathToPath(string path)
        {
            return path
                .Replace("+", ".")
                .Replace('-', '/');
        }
    }
}