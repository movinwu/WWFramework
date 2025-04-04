/*------------------------------
 * 脚本名称: Log
 * 创建者: movin
 * 创建日期: 2025/03/18
------------------------------*/

using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 日志类
    /// </summary>
    public static class Log
    {
        #region 日志打印
        
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
        public static void Init()
        {
            _enableLogType = GameEntry.GlobalGameConfig.enableLogType;
            _logLevel = (
                GameEntry.GlobalGameConfig.enableLogDebug, 
                GameEntry.GlobalGameConfig.enableLogWarning, 
                GameEntry.GlobalGameConfig.enableLogError);
        }
        
        /// <summary>
        /// log日志
        /// </summary>
        /// <param name="logBuild"></param>
        /// <param name="logType"></param>
        public static void LogDebug(System.Action<StringBuilder> logBuild, ELogType logType = ELogType.Common)
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
                UnityEngine.Debug.LogWarning($"{nameof(Log)}:{nameof(LogDebug)}:{nameof(logBuild)}为空");
                return;
            }

            LOGInfoBuilder.Clear();
            LOGInfoBuilder.Append("[");
            LOGInfoBuilder.Append(Time.realtimeSinceStartup);
            LOGInfoBuilder.Append("] ");
            LOGInfoBuilder.Append(" ");
            LOGInfoBuilder.Append("[");
            LOGInfoBuilder.Append(logType);
            LOGInfoBuilder.Append("]");
            LOGInfoBuilder.Append(" ");
            LOGInfoBuilder.Append("[");
            LOGInfoBuilder.Append(Time.frameCount);
            LOGInfoBuilder.Append("帧]");
            LOGInfoBuilder.Append(" ");
            LOGInfoBuilder.Append("[");
            LOGInfoBuilder.Append(Mathf.RoundToInt(Time.deltaTime * 1000f));
            LOGInfoBuilder.Append("ms]");
            LOGInfoBuilder.Append(" ");
            LOGInfoBuilder.Append("[×");
            LOGInfoBuilder.Append(Time.timeScale.ToString("F2"));
            LOGInfoBuilder.Append("]");
            AppendColor();
            logBuild(LOGInfoBuilder);
            LOGInfoBuilder.Append("</color>");
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
                UnityEngine.Debug.LogWarning($"{nameof(Log)}:{nameof(LogDebug)}:{nameof(logBuild)}为空");
                return;
            }

            LOGInfoBuilder.Clear();
            LOGInfoBuilder.Append("[");
            LOGInfoBuilder.Append(Time.realtimeSinceStartup);
            LOGInfoBuilder.Append("] ");
            LOGInfoBuilder.Append(" ");
            LOGInfoBuilder.Append("[");
            LOGInfoBuilder.Append(logType);
            LOGInfoBuilder.Append("]");
            LOGInfoBuilder.Append(" ");
            LOGInfoBuilder.Append("[");
            LOGInfoBuilder.Append(Time.frameCount);
            LOGInfoBuilder.Append("帧]");
            LOGInfoBuilder.Append(" ");
            LOGInfoBuilder.Append("[");
            LOGInfoBuilder.Append(Mathf.RoundToInt(Time.deltaTime * 1000f));
            LOGInfoBuilder.Append("ms]");
            LOGInfoBuilder.Append(" ");
            LOGInfoBuilder.Append("[×");
            LOGInfoBuilder.Append(Time.timeScale.ToString("F2"));
            LOGInfoBuilder.Append("]");
            AppendColor();
            logBuild(LOGInfoBuilder);
            LOGInfoBuilder.Append("</color>");
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
                UnityEngine.Debug.LogWarning($"{nameof(Log)}:{nameof(LogDebug)}:{nameof(logBuild)}为空");
                return;
            }

            LOGInfoBuilder.Clear();
            LOGInfoBuilder.Append("[");
            LOGInfoBuilder.Append(Time.realtimeSinceStartup);
            LOGInfoBuilder.Append("] ");
            LOGInfoBuilder.Append(" ");
            LOGInfoBuilder.Append("[");
            LOGInfoBuilder.Append(logType);
            LOGInfoBuilder.Append("]");
            LOGInfoBuilder.Append(" ");
            LOGInfoBuilder.Append("[");
            LOGInfoBuilder.Append(Time.frameCount);
            LOGInfoBuilder.Append("帧]");
            LOGInfoBuilder.Append(" ");
            LOGInfoBuilder.Append("[");
            LOGInfoBuilder.Append(Mathf.RoundToInt(Time.deltaTime * 1000f));
            LOGInfoBuilder.Append("ms]");
            LOGInfoBuilder.Append(" ");
            LOGInfoBuilder.Append("[×");
            LOGInfoBuilder.Append(Time.timeScale.ToString("F2"));
            LOGInfoBuilder.Append("]");
            AppendColor();
            logBuild(LOGInfoBuilder);
            LOGInfoBuilder.Append("</color>");
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

        #endregion 日志打印

        #region 函数运行时间
        
        /// <summary>
        /// 时间运行监控(System监控)
        /// </summary>
        private static readonly Stopwatch Stopwatch = new Stopwatch();

        /// <summary>
        /// 时间运行监控(UnityRealTime监控)
        /// </summary>
        private static float _unityRealTimeCache = 0f;
        
        /// <summary>
        /// 启动时间监控
        /// </summary>
        /// <param name="monitorType">监控类型</param>
        public static void StartTimeMonitor(EMonitorType monitorType = EMonitorType.System)
        {
            switch (monitorType)
            {
                case EMonitorType.System:
                    Stopwatch.Restart();
                    break;
                case EMonitorType.UnityRealTime:
                    _unityRealTimeCache = Time.realtimeSinceStartup;
                    break;
            }
        }
        
        /// <summary>
        /// 结束时间监控
        /// </summary>
        /// <param name="monitorType">监控类型</param>
        /// <param name="logType">日志类型</param>
        /// <param name="logLevel">日志级别</param>
        public static void EndTimeMonitor(
            EMonitorType monitorType = EMonitorType.System,
            ELogType logType = ELogType.Common,
            LogType logLevel = LogType.Log)
        {
            switch (monitorType)
            {
                case EMonitorType.System:
                    Stopwatch.Stop();

                    void SystemLogBuild(StringBuilder logBuild)
                    {
                        logBuild.Append("[System]");
                        logBuild.Append(Stopwatch.ElapsedMilliseconds);
                        logBuild.Append("-ms");
                    }

                    switch (logLevel)
                    {
                        case LogType.Log:
                            Log.LogDebug(SystemLogBuild, logType);
                            break;
                        case LogType.Warning:
                            Log.LogWarning(SystemLogBuild, logType);
                            break;
                        case LogType.Error:
                            Log.LogError(SystemLogBuild, logType);
                            break;
                        default:
                            Log.LogError(SystemLogBuild, logType);
                            break;
                    }
                    break;
                case EMonitorType.UnityRealTime:
                    var realTime = Time.realtimeSinceStartup - _unityRealTimeCache;

                    void RealTimeLogBuild(StringBuilder logBuild)
                    {
                        logBuild.Append("[UnityRealTime]");
                        logBuild.Append(Mathf.RoundToInt(realTime * 1000f));
                        logBuild.Append("-ms");
                    }

                    switch (logLevel)
                    {
                        case LogType.Log:
                            Log.LogDebug(RealTimeLogBuild, logType);
                            break;
                        case LogType.Warning:
                            Log.LogWarning(RealTimeLogBuild, logType);
                            break;
                        case LogType.Error:
                            Log.LogError(RealTimeLogBuild, logType);
                            break;
                        default:
                            Log.LogError(RealTimeLogBuild, logType);
                            break;
                    }
                    break;
            }
        }

        #endregion 函数运行时间
    }
}