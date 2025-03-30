/*------------------------------
 * 脚本名称: MainProcedureModuleEditor
 * 创建者: movin
 * 创建日期: 2025/03/22
------------------------------*/

using UnityEditor;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// MainProcedureModule 的 Inspector 编辑器
    /// </summary>
    [CustomEditor(typeof(MainProcedureModule))]
    public class MainProcedureModuleEditor : InspectorEditor
    {
        private bool _showProcedures = true;
        private Vector2 _scrollPos;

        public override void OnInspectorGUI()
        {
            // 绘制默认Inspector
            base.OnInspectorGUI();

            var module = (MainProcedureModule)target;

            // 运行时调试信息
            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("运行时流程调试信息", MessageType.Info);

                // 流程树状结构展示
                _showProcedures = EditorGUILayout.Foldout(_showProcedures, "当前流程结构");
                if (_showProcedures)
                {
                    OnProcedureInspectorGUI(module.MainProcedure);

                    foreach (var procedure in module.GUIProcedures)
                    {
                        OnProcedureInspectorGUI(procedure);
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox("进入运行模式查看流程执行状态", MessageType.Warning);
            }
        }

        /// <summary>
        /// 单个流程进度展示
        /// </summary>
        /// <param name="procedure"></param>
        private void OnProcedureInspectorGUI(ProcedureBase procedure)
        {
            // 流程进度展示
            if (null != procedure)
            {
                var progress = procedure.Progress;
                EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(),
                    progress.current / progress.total,
                    $"流程进度 ({progress.current:0.0}/{progress.total:0.0})");
                    
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
                DrawProcedureTree(procedure);
                EditorGUILayout.EndScrollView();
            }
        }

        /// <summary>
        /// 绘制流程树状结构
        /// </summary>
        /// <param name="procedure">流程</param>
        /// <param name="indentLevel">缩进</param>
        private void DrawProcedureTree(ProcedureBase procedure, int indentLevel = 0)
        {
            // 缩进处理
            GUILayout.BeginHorizontal();
            GUILayout.Space(indentLevel * 15);

            // 状态颜色标记
            var statusColor = procedure.IsFinished ? Color.gray : Color.green;
            EditorGUILayout.LabelField("■", GUILayout.Width(20), GUILayout.Height(15));
            GUI.color = statusColor;

            // 显示流程信息
            var progress = procedure.Progress;
            EditorGUILayout.LabelField($"{procedure.GetType().Name} -- {progress.current:0.0}/{progress.total:0.0}");
            GUI.color = Color.white;

            GUILayout.EndHorizontal();

            // 递归绘制子流程
            if (procedure is SequenceProcedure sequence)
            {
                foreach (var child in sequence.Procedures)
                {
                    DrawProcedureTree(child, indentLevel + 1);
                }
            }
            else if (procedure is ParallelProcedure parallel)
            {
                foreach (var child in parallel.Procedures)
                {
                    DrawProcedureTree(child, indentLevel + 1);
                }
            }
        }
    }
}