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
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
    
            EditorGUI.PropertyField(position, property, label, true);
    
            EditorGUI.indentLevel = indent;
            GUI.enabled = true;
        }
    }
}
#endif