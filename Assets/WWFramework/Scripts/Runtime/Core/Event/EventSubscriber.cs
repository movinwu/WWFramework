/*------------------------------
 * 脚本名称: EventSubscriber
 * 创建者: movin
 * 创建日期: 2025/04/04
------------------------------*/

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WWFramework
{

    /// <summary>
    /// 事件订阅者
    /// </summary>
    internal sealed class EventSubscriber<T> : IEventSubscriber where T : Delegate
    {
        /// <summary>
        /// 订阅者id
        /// </summary>
        internal static ulong SubscriberId;

        /// <summary>
        /// 所有订阅事件
        /// </summary>
        private readonly Dictionary<ulong, HashSet<T>> _actions = new();
        
        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="eventId">事件id</param>
        /// <param name="action">事件</param>
        internal void Subscribe(ulong eventId, T action)
        {
            if (!_actions.TryGetValue(eventId, out var hashSet))
            {
                hashSet = new HashSet<T>();
                _actions.Add(eventId, hashSet);
            }
            if (!hashSet.Add(action))
            {
                Log.LogWarning(s =>
                {
                    s.Append("事件重复订阅, ");
                    s.Append($"事件id:");
                    s.Append(eventId);
                }, ELogType.Event);
            }
        }
        
        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <param name="eventId">事件id</param>
        /// <param name="action">事件</param>
        internal void Unsubscribe(ulong eventId, T action)
        {
            if (!_actions.TryGetValue(eventId, out var hashSet)
                || !hashSet.Remove(action))
            {
                Log.LogWarning(s =>
                {
                    s.Append("事件没有订阅但是试图取消订阅, ");
                    s.Append($"事件id:");
                    s.Append(eventId);
                }, ELogType.Event);
            }
        }

        /// <summary>
        /// 发布事件,立即发布
        /// <param name="eventId">事件id</param>
        /// <param name="invokeAction">事件执行方法</param>
        /// </summary>
        internal void Publish(ulong eventId, Action<T> invokeAction)
        {
            CheckFramePublish(eventId, invokeAction, true).Forget();
        }

        /// <summary>
        /// 发布事件,延迟发布
        /// </summary>
        /// <param name="eventId">事件id</param>
        /// <param name="invokeAction">事件执行方法</param>
        internal void PublishDelay(ulong eventId, Action<T> invokeAction)
        {
            CheckFramePublish(eventId, invokeAction, false).Forget();
        }

        /// <summary>
        /// 检查帧数,确定是否发布事件
        /// <param name="eventId">事件id</param>
        /// <param name="invokeAction">事件执行方法</param>
        /// <param name="force">是否强制发布</param>
        /// </summary>
        private async UniTask CheckFramePublish(ulong eventId, Action<T> invokeAction, bool force = true)
        {
            var frame = Time.frameCount;
            if (EventCenter.frame != frame)
            {
                EventCenter.frame = frame;
                EventCenter.framePublishTimer = 0;
                return;
            }
            
            if (EventCenter.framePublishTimer <= EventCenter.MAX_FRAME_PUBLISH_TIME || force)
            {
                var timer = Time.realtimeSinceStartup;
                if (!_actions.TryGetValue(eventId, out var hashSet)) return;
                foreach (var action in hashSet)
                {
                    invokeAction(action);
                }
                EventCenter.framePublishTimer += (Time.realtimeSinceStartup - timer) * 1000f;
            }
            else
            {
                // 延迟一帧,重新检查
                await UniTask.DelayFrame(1);
                CheckFramePublish(eventId, invokeAction, force).Forget();
            }
        }
    }
}