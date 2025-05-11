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
        /// 文件或文件夹路径转AssetBundle名称
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string PathToAssetBundleName(string path)
        {
            return path
                .Replace("Assets/", string.Empty)
                .Replace('.', '+')
                .Replace('/', '-')
                .Replace('\\', '-');
        }

        /// <summary>
        /// AssetBundle名称转文件或文件夹路径
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string AssetBundleNameToPath(string name)
        {
            return name
                .Replace("+", ".")
                .Replace('-', '/');
        }

        /// <summary>
        /// 获取资源扩展字符串
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string GetExtension(EResourceExtension extension)
        {
            switch (extension)
            {
                case EResourceExtension.png:
                    return ".png";
                case EResourceExtension.txt:
                    return ".txt";
                case EResourceExtension.json:
                    return ".json";
                case EResourceExtension.xml:
                    return ".xml";
                case EResourceExtension.mat:
                    return ".mat";
                case EResourceExtension.audio:
                    return ".mp3";
                case EResourceExtension.texture:
                    return ".png";
                case EResourceExtension.prefab:
                    return ".prefab";
            }
            return string.Empty;
        }
    }
}