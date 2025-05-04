/*------------------------------
 * 脚本名称: EResourceExtension
 * 创建者: movin
 * 创建日期: 2025/05/03
------------------------------*/

namespace WWFramework
{
    /// <summary>
    /// 资源后缀类型
    /// </summary>
    public enum EResourceExtension : int
    {
        png = 1 << 0,
        
        txt = 1 << 1,
        
        json = 1 << 2,
        
        xml = 1 << 3,
        
        mat = 1 << 4,
        
        audio = 1 << 5,
        
        texture = 1 << 6,
        
        prefab = 1 << 7,
    }
}