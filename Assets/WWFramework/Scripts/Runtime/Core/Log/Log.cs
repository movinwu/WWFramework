/*------------------------------
 * 脚本名称: Log
 * 创建者: movin
 * 创建日期: 2025/03/18
------------------------------*/

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using PlasticGui.Configuration.CloudEdition.Welcome;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 日志类
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// 内部使用的StringBuilder,用于构造日志
        /// </summary>
        private static readonly StringBuilder LOGInfoBuilder = new StringBuilder();

        /// <summary>
        /// 允许打印的日志类型
        /// </summary>
        private static ELogType _enableLogType;

        /// <summary>
        /// 允许打印的日志级别
        /// </summary>
        private static (bool enableDebug, bool enableWarning, bool enableError) _logLevel;

        /// <summary>
        /// 初始化日志
        /// </summary>
        /// <param name="enableLogType">允许的日志类型</param>
        /// <param name="enableDebug">是否允许debug日志</param>
        /// <param name="enableWarning">是否允许warning日志</param>
        /// <param name="enableError">是否允许error日志</param>
        public static void Init(ELogType enableLogType, bool enableDebug, bool enableWarning, bool enableError)
        {
            _enableLogType = enableLogType;
            _logLevel = (enableDebug, enableWarning, enableError);
        }
        
        /// <summary>
        /// log日志
        /// </summary>
        /// <param name="logBuild"></param>
        /// <param name="logType"></param>
        public static void Debug(System.Action<StringBuilder> logBuild, ELogType logType = ELogType.Common)
        {
            // 游戏运行中进行日志类型判断和日志级别判断
            if (Application.isPlaying)
            {
                if (!_logLevel.enableDebug)
                {
                    return;
                }
                if ((_enableLogType & logType) == 0)
                {
                    return;
                }
            }

            if (logBuild == null)
            {
                UnityEngine.Debug.LogWarning($"{nameof(Log)}:{nameof(Debug)}:{nameof(logBuild)}为空");
                return;
            }

            LOGInfoBuilder.Clear();
            // 打印当前运行时间
            LOGInfoBuilder.Append("[当前运行时间");
            LOGInfoBuilder.Append(Time.realtimeSinceStartup);
            LOGInfoBuilder.Append("] ");
            LOGInfoBuilder.Append(" ");
            LOGInfoBuilder.Append("[打印类型");
            LOGInfoBuilder.Append(logType);
            LOGInfoBuilder.Append("]");
            LOGInfoBuilder.Append(" ");
            AppendColor();
            logBuild(LOGInfoBuilder);
            LOGInfoBuilder.Append("</color>");
            // 换行打印当前帧数等信息
            LOGInfoBuilder.AppendLine();
            LOGInfoBuilder.Append("[第");
            LOGInfoBuilder.Append(Time.frameCount);
            LOGInfoBuilder.Append("帧]");
            LOGInfoBuilder.Append(" ");
            LOGInfoBuilder.Append("[帧耗时");
            LOGInfoBuilder.Append(Mathf.RoundToInt(Time.deltaTime * 1000f));
            LOGInfoBuilder.Append("ms]");
            LOGInfoBuilder.Append(" ");
            LOGInfoBuilder.Append("[游戏时间倍率");
            LOGInfoBuilder.Append(Time.timeScale.ToString("F2"));
            LOGInfoBuilder.Append("]");
            UnityEngine.Debug.Log(LOGInfoBuilder.ToString());

            void AppendColor()
            {
                switch (logType)
                {
                    case ELogType.Common:
                        LOGInfoBuilder.Append("<color=#FFFFFF>");
                        break;
                    case ELogType.Resource:
                        LOGInfoBuilder.Append("<color=#FFD700>");
                        break;
                    case ELogType.Config:
                        LOGInfoBuilder.Append("<color=#FFA500>");
                        break;
                    case ELogType.DataTable:
                        LOGInfoBuilder.Append("<color=#FF0000>");
                        break;
                    default:
                        LOGInfoBuilder.Append("<color=#FFFFFF>");
                        break;
                }
            }
        }
        
        /// <summary>
        /// warning日志
        /// </summary>
        /// <param name="logBuild"></param>
        /// <param name="logType"></param>
        public static void LogWarning(System.Action<StringBuilder> logBuild, ELogType logType = ELogType.Common)
        {
            // 游戏运行中进行日志类型判断和日志级别判断
            if (Application.isPlaying)
            {
                if (!_logLevel.enableWarning)
                {
                    return;
                }
                if ((_enableLogType & logType) == 0)
                {
                    return;
                }
            }

            if (logBuild == null)
            {
                UnityEngine.Debug.LogWarning($"{nameof(Log)}:{nameof(Debug)}:{nameof(logBuild)}为空");
                return;
            }

            LOGInfoBuilder.Clear();
            // 打印当前运行时间
            LOGInfoBuilder.Append("[当前运行时间");
            LOGInfoBuilder.Append(Time.realtimeSinceStartup);
            LOGInfoBuilder.Append("] ");
            LOGInfoBuilder.Append(" ");
            LOGInfoBuilder.Append("[打印类型");
            LOGInfoBuilder.Append(logType);
            LOGInfoBuilder.Append("]");
            LOGInfoBuilder.Append(" ");
            AppendColor();
            logBuild(LOGInfoBuilder);
            LOGInfoBuilder.Append("</color>");
            // 换行打印当前帧数等信息
            LOGInfoBuilder.AppendLine();
            LOGInfoBuilder.Append("[第");
            LOGInfoBuilder.Append(Time.frameCount);
            LOGInfoBuilder.Append("帧]");
            LOGInfoBuilder.Append(" ");
            LOGInfoBuilder.Append("[帧耗时");
            LOGInfoBuilder.Append(Mathf.RoundToInt(Time.deltaTime * 1000f));
            LOGInfoBuilder.Append("ms]");
            LOGInfoBuilder.Append(" ");
            LOGInfoBuilder.Append("[游戏时间倍率");
            LOGInfoBuilder.Append(Time.timeScale.ToString("F2"));
            LOGInfoBuilder.Append("]");
            UnityEngine.Debug.LogWarning(LOGInfoBuilder.ToString());

            void AppendColor()
            {
                switch (logType)
                {
                    case ELogType.Common:
                        LOGInfoBuilder.Append("<color=#FFFFFF>");
                        break;
                    case ELogType.Resource:
                        LOGInfoBuilder.Append("<color=#FFD700>");
                        break;
                    case ELogType.Config:
                        LOGInfoBuilder.Append("<color=#FFA500>");
                        break;
                    case ELogType.DataTable:
                        LOGInfoBuilder.Append("<color=#FF0000>");
                        break;
                    default:
                        LOGInfoBuilder.Append("<color=#FFFFFF>");
                        break;
                }
            }
        }
        
        /// <summary>
        /// error 日志
        /// </summary>
        /// <param name="logBuild"></param>
        /// <param name="logType"></param>
        public static void LogError(System.Action<StringBuilder> logBuild, ELogType logType = ELogType.Common)
        {
            // 游戏运行中进行日志类型判断和日志级别判断
            if (Application.isPlaying)
            {
                if (!_logLevel.enableError)
                {
                    return;
                }
                if ((_enableLogType & logType) == 0)
                {
                    return;
                }
            }

            if (logBuild == null)
            {
                UnityEngine.Debug.LogWarning($"{nameof(Log)}:{nameof(Debug)}:{nameof(logBuild)}为空");
                return;
            }

            LOGInfoBuilder.Clear();
            // 打印当前运行时间
            LOGInfoBuilder.Append("[当前运行时间");
            LOGInfoBuilder.Append(Time.realtimeSinceStartup);
            LOGInfoBuilder.Append("] ");
            LOGInfoBuilder.Append(" ");
            LOGInfoBuilder.Append("[打印类型");
            LOGInfoBuilder.Append(logType);
            LOGInfoBuilder.Append("]");
            LOGInfoBuilder.Append(" ");
            AppendColor();
            logBuild(LOGInfoBuilder);
            LOGInfoBuilder.Append("</color>");
            // 换行打印当前帧数等信息
            LOGInfoBuilder.AppendLine();
            LOGInfoBuilder.Append("[第");
            LOGInfoBuilder.Append(Time.frameCount);
            LOGInfoBuilder.Append("帧]");
            LOGInfoBuilder.Append(" ");
            LOGInfoBuilder.Append("[帧耗时");
            LOGInfoBuilder.Append(Mathf.RoundToInt(Time.deltaTime * 1000f));
            LOGInfoBuilder.Append("ms]");
            LOGInfoBuilder.Append(" ");
            LOGInfoBuilder.Append("[游戏时间倍率");
            LOGInfoBuilder.Append(Time.timeScale.ToString("F2"));
            LOGInfoBuilder.Append("]");
            UnityEngine.Debug.LogError(LOGInfoBuilder.ToString());

            void AppendColor()
            {
                switch (logType)
                {
                    case ELogType.Common:
                        LOGInfoBuilder.Append("<color=#FFFFFF>");
                        break;
                    case ELogType.Resource:
                        LOGInfoBuilder.Append("<color=#FFD700>");
                        break;
                    case ELogType.Config:
                        LOGInfoBuilder.Append("<color=#FFA500>");
                        break;
                    case ELogType.DataTable:
                        LOGInfoBuilder.Append("<color=#FF0000>");
                        break;
                    default:
                        LOGInfoBuilder.Append("<color=#FFFFFF>");
                        break;
                }
            }
        }
    }
}