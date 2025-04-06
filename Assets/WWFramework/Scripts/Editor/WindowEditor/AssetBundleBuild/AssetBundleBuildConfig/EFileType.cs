/*------------------------------
 * 脚本名称: EFileType
 * 创建者: movin
 * 创建日期: 2025/04/06
------------------------------*/

using System;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 文件类型
    /// </summary>
    [Flags]
    public enum EFileType
    {
        Prefab = 1 << 0,
        
        Texture = 1 << 1,
        
        Audio = 1 << 2,
        
        Font = 1 << 3,
        
        Material = 1 << 4,
        
        Model = 1 << 5,
        
        Text = 1 << 6,
        
        Byte = 1 << 7,
        
        ScriptableObject = 1 << 8,
        
        Shader = 1 << 9,
        
        Animation = 1 << 10,
        
        Video = 1 << 11,
    }
}