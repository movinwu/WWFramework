/*------------------------------
 * 脚本名称: EnumFlagsDrawer
 * 创建者: movin
 * 创建日期: 2025/03/18
------------------------------*/

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace WWFramework
{
    [CustomPropertyDrawer(typeof(EnumFlagAttribute))]
    public class EnumFlagDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.intValue = EditorGUI.MaskField(
                position,
                label, 
                property.intValue,
                property.enumNames);
        }
    }
}
#endif
