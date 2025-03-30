/*------------------------------
 * 脚本名称: InspectorEditor
 * 创建者: movin
 * 创建日期: 2025/03/30
------------------------------*/

using UnityEditor;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// Inspector编辑器
    /// </summary>
    public class InspectorEditor : Editor
    {
        private void OnEnable()
        {
            // 注册刷新回调
            EditorApplication.update += RepaintIfPlaying;
        }

        private void OnDisable()
        {
            EditorApplication.update -= RepaintIfPlaying;
        }

        private void RepaintIfPlaying()
        {
            if (EditorApplication.isPlaying)
            {
                Repaint();
            }
        }
    }
}