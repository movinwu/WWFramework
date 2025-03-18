/*------------------------------
 * 脚本名称: ReadOnlyDrawer
 * 创建者: movin
 * 创建日期: 2025/03/02
------------------------------*/

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 只读属性绘制器
    /// </summary>
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // 禁用GUI，使字段变为只读
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}
#endif