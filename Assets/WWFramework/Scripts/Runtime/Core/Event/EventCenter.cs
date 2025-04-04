/*------------------------------
 * 脚本名称: EventCenter
 * 创建者: movin
 * 创建日期: 2025/03/30
------------------------------*/

using System;
using System.Collections.Generic;

namespace WWFramework
{
    /// <summary>
    /// 事件中心,负责事件的订阅与发布
    /// </summary>
    public class EventCenter
    {
        /// <summary>
        /// 所有事件订阅者
        /// </summary>
        private readonly Dictionary<ulong, IEventSubscriber> _eventSubscribers =
            new Dictionary<ulong, IEventSubscriber>();

        /// <summary>
        /// 最大帧发布时间(毫秒数)
        /// </summary>
        internal const float MAX_FRAME_PUBLISH_TIME = 10;
        
        /// <summary>
        /// 帧发布计时时间
        /// </summary>
        internal static float framePublishTimer;

        /// <summary>
        /// 帧数
        /// </summary>
        internal static int frame = -1;
        
        /// <summary>
        /// 初始化事件中心
        /// </summary>
        protected void InitEventCenter()
        {
            _eventSubscribers.Clear();
        }
        
        /// <summary>
        /// 释放事件中心
        /// </summary>
        protected void ReleaseEventCenter()
        {
            _eventSubscribers.Clear();
        }
        
        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="eventId">事件id</param>
        /// <param name="action">订阅事件</param>
        public void Subscribe(ulong eventId, Action action)
        {
            if (null == action)
            {
                Log.LogError(s =>
                {
                    s.Append("订阅事件为空, ");
                    s.Append($"事件id:");
                    s.Append(eventId);
                }, ELogType.Event);
                return;
            }
            
            // 检查事件订阅者id是否存在
            if (EventSubscriber<Action>.SubscriberId == 0)
            {
                EventSubscriber<Action>.SubscriberId = UniqueIDGenerator.ULongID;
            }

            // 检查事件订阅者是否存在
            if (!_eventSubscribers.TryGetValue(EventSubscriber<Action>.SubscriberId, out var subscriber))
            {
                subscriber = new EventSubscriber<Action>();
                _eventSubscribers.Add(EventSubscriber<Action>.SubscriberId, subscriber);
            }

            if (subscriber is EventSubscriber<Action> actionSubscriber)
            {
                actionSubscriber.Subscribe(eventId, action);
            }
        }

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        public void Unsubscribe(ulong eventId, Action action)
        {
            if (null == action)
            {
                Log.LogError(s =>
                {
                    s.Append("取消订阅事件为空, ");
                    s.Append($"事件id:");
                    s.Append(eventId);
                }, ELogType.Event);
                return;
            }
            
            // 检查事件订阅者id是否存在
            if (EventSubscriber<Action>.SubscriberId == 0)
            {
                EventSubscriber<Action>.SubscriberId = UniqueIDGenerator.ULongID;
            }
            
            if (!_eventSubscribers.TryGetValue(EventSubscriber<Action>.SubscriberId, out var subscriber))
            {
                Log.LogWarning(s =>
                {
                    s.Append("事件没有订阅但是试图取消订阅, ");
                    s.Append($"事件id:");
                    s.Append(eventId);
                }, ELogType.Event);
                return;
            }
            
            if (subscriber is EventSubscriber<Action> actionSubscriber)
            {
                actionSubscriber.Unsubscribe(eventId, action);
            }
        }

        /// <summary>
        /// 发布事件
        /// </summary>
        public void Publish(ulong eventId)
        {
            // 检查事件订阅者id是否存在
            if (EventSubscriber<Action>.SubscriberId == 0)
            {
                EventSubscriber<Action>.SubscriberId = UniqueIDGenerator.ULongID;
            }
            
            if (!_eventSubscribers.TryGetValue(EventSubscriber<Action>.SubscriberId, out var subscriber))
            {
                return;
            }

            if (subscriber is EventSubscriber<Action> actionSubscriber)
            {
                actionSubscriber.Publish(eventId, action => action());
            }
        }
        
        /// <summary>
        /// 延迟发布事件
        /// </summary>
        /// <param name="eventId">事件id</param>
        public void PublishDelay(ulong eventId)
        {
            // 检查事件订阅者id是否存在
            if (EventSubscriber<Action>.SubscriberId == 0)
            {
                EventSubscriber<Action>.SubscriberId = UniqueIDGenerator.ULongID;
            }
            
            if (!_eventSubscribers.TryGetValue(EventSubscriber<Action>.SubscriberId, out var subscriber))
            {
                return;
            }

            if (subscriber is EventSubscriber<Action> actionSubscriber)
            {
                actionSubscriber.PublishDelay(eventId, action => action());
            }
        }

        /// <summary>
        /// 订阅事件(1个参数)
        /// </summary>
        public void Subscribe<T1>(ulong eventId, Action<T1> action)
        {
            if (null == action)
            {
                Log.LogError(s =>
                {
                    s.Append("订阅事件为空, ");
                    s.Append($"事件id:");
                    s.Append(eventId);
                }, ELogType.Event);
                return;
            }

            if (EventSubscriber<Action<T1>>.SubscriberId == 0)
            {
                EventSubscriber<Action<T1>>.SubscriberId = UniqueIDGenerator.ULongID;
            }

            if (!_eventSubscribers.TryGetValue(EventSubscriber<Action<T1>>.SubscriberId, out var subscriber))
            {
                subscriber = new EventSubscriber<Action<T1>>();
                _eventSubscribers.Add(EventSubscriber<Action<T1>>.SubscriberId, subscriber);
            }

            if (subscriber is EventSubscriber<Action<T1>> actionSubscriber)
            {
                actionSubscriber.Subscribe(eventId, action);
            }
        }

        /// <summary>
        /// 取消订阅事件(1个参数)
        /// </summary>
        public void Unsubscribe<T1>(ulong eventId, Action<T1> action)
        {
            if (null == action)
            {
                Log.LogError(s =>
                {
                    s.Append("取消订阅事件为空, ");
                    s.Append($"事件id:");
                    s.Append(eventId);
                }, ELogType.Event);
                return;
            }

            if (EventSubscriber<Action<T1>>.SubscriberId == 0)
            {
                EventSubscriber<Action<T1>>.SubscriberId = UniqueIDGenerator.ULongID;
            }

            if (!_eventSubscribers.TryGetValue(EventSubscriber<Action<T1>>.SubscriberId, out var subscriber))
            {
                Log.LogWarning(s =>
                {
                    s.Append("事件没有订阅但是试图取消订阅, ");
                    s.Append($"事件id:");
                    s.Append(eventId);
                }, ELogType.Event);
                return;
            }

            if (subscriber is EventSubscriber<Action<T1>> actionSubscriber)
            {
                actionSubscriber.Unsubscribe(eventId, action);
            }
        }

        /// <summary>
        /// 发布事件(1个参数)
        /// </summary>
        public void Publish<T1>(ulong eventId, T1 arg1)
        {
            if (EventSubscriber<Action<T1>>.SubscriberId == 0)
            {
                EventSubscriber<Action<T1>>.SubscriberId = UniqueIDGenerator.ULongID;
            }

            if (!_eventSubscribers.TryGetValue(EventSubscriber<Action<T1>>.SubscriberId, out var subscriber))
            {
                return;
            }

            if (subscriber is EventSubscriber<Action<T1>> actionSubscriber)
            {
                actionSubscriber.Publish(eventId, action => action(arg1));
            }
        }

        /// <summary>
        /// 延迟发布事件(1个参数)
        /// </summary>
        public void PublishDelay<T1>(ulong eventId, T1 arg1)
        {
            if (EventSubscriber<Action<T1>>.SubscriberId == 0)
            {
                EventSubscriber<Action<T1>>.SubscriberId = UniqueIDGenerator.ULongID;
            }

            if (!_eventSubscribers.TryGetValue(EventSubscriber<Action<T1>>.SubscriberId, out var subscriber))
            {
                return;
            }

            if (subscriber is EventSubscriber<Action<T1>> actionSubscriber)
            {
                actionSubscriber.PublishDelay(eventId, action => action(arg1));
            }
        }

        /// <summary>
        /// 订阅事件(2个参数)
        /// </summary>
        public void Subscribe<T1, T2>(ulong eventId, Action<T1, T2> action)
        {
            if (null == action)
            {
                Log.LogError(s =>
                {
                    s.Append("订阅事件为空, ");
                    s.Append($"事件id:");
                    s.Append(eventId);
                }, ELogType.Event);
                return;
            }

            if (EventSubscriber<Action<T1, T2>>.SubscriberId == 0)
            {
                EventSubscriber<Action<T1, T2>>.SubscriberId = UniqueIDGenerator.ULongID;
            }

            if (!_eventSubscribers.TryGetValue(EventSubscriber<Action<T1, T2>>.SubscriberId, out var subscriber))
            {
                subscriber = new EventSubscriber<Action<T1, T2>>();
                _eventSubscribers.Add(EventSubscriber<Action<T1, T2>>.SubscriberId, subscriber);
            }

            if (subscriber is EventSubscriber<Action<T1, T2>> actionSubscriber)
            {
                actionSubscriber.Subscribe(eventId, action);
            }
        }

        /// <summary>
        /// 取消订阅事件(2个参数)
        /// </summary>
        public void Unsubscribe<T1, T2>(ulong eventId, Action<T1, T2> action)
        {
            if (null == action)
            {
                Log.LogError(s =>
                {
                    s.Append("取消订阅事件为空, ");
                    s.Append($"事件id:");
                    s.Append(eventId);
                }, ELogType.Event);
                return;
            }

            if (EventSubscriber<Action<T1, T2>>.SubscriberId == 0)
            {
                EventSubscriber<Action<T1, T2>>.SubscriberId = UniqueIDGenerator.ULongID;
            }

            if (!_eventSubscribers.TryGetValue(EventSubscriber<Action<T1, T2>>.SubscriberId, out var subscriber))
            {
                Log.LogWarning(s =>
                {
                    s.Append("事件没有订阅但是试图取消订阅, ");
                    s.Append($"事件id:");
                    s.Append(eventId);
                }, ELogType.Event);
                return;
            }

            if (subscriber is EventSubscriber<Action<T1, T2>> actionSubscriber)
            {
                actionSubscriber.Unsubscribe(eventId, action);
            }
        }

        /// <summary>
        /// 发布事件(2个参数)
        /// </summary>
        public void Publish<T1, T2>(ulong eventId, T1 arg1, T2 arg2)
        {
            if (EventSubscriber<Action<T1, T2>>.SubscriberId == 0)
            {
                EventSubscriber<Action<T1, T2>>.SubscriberId = UniqueIDGenerator.ULongID;
            }

            if (!_eventSubscribers.TryGetValue(EventSubscriber<Action<T1, T2>>.SubscriberId, out var subscriber))
            {
                return;
            }

            if (subscriber is EventSubscriber<Action<T1, T2>> actionSubscriber)
            {
                actionSubscriber.Publish(eventId, action => action(arg1, arg2));
            }
        }

        /// <summary>
        /// 延迟发布事件(2个参数)
        /// </summary>
        public void PublishDelay<T1, T2>(ulong eventId, T1 arg1, T2 arg2)
        {
            if (EventSubscriber<Action<T1, T2>>.SubscriberId == 0)
            {
                EventSubscriber<Action<T1, T2>>.SubscriberId = UniqueIDGenerator.ULongID;
            }

            if (!_eventSubscribers.TryGetValue(EventSubscriber<Action<T1, T2>>.SubscriberId, out var subscriber))
            {
                return;
            }

            if (subscriber is EventSubscriber<Action<T1, T2>> actionSubscriber)
            {
                actionSubscriber.PublishDelay(eventId, action => action(arg1, arg2));
            }
        }

        /// <summary>
        /// 订阅事件(3个参数)
        /// </summary>
        public void Subscribe<T1, T2, T3>(ulong eventId, Action<T1, T2, T3> action)
        {
            if (null == action)
            {
                Log.LogError(s =>
                {
                    s.Append("订阅事件为空, ");
                    s.Append($"事件id:");
                    s.Append(eventId);
                }, ELogType.Event);
                return;
            }

            if (EventSubscriber<Action<T1, T2, T3>>.SubscriberId == 0)
            {
                EventSubscriber<Action<T1, T2, T3>>.SubscriberId = UniqueIDGenerator.ULongID;
            }

            if (!_eventSubscribers.TryGetValue(EventSubscriber<Action<T1, T2, T3>>.SubscriberId, out var subscriber))
            {
                subscriber = new EventSubscriber<Action<T1, T2, T3>>();
                _eventSubscribers.Add(EventSubscriber<Action<T1, T2, T3>>.SubscriberId, subscriber);
            }

            if (subscriber is EventSubscriber<Action<T1, T2, T3>> actionSubscriber)
            {
                actionSubscriber.Subscribe(eventId, action);
            }
        }

        /// <summary>
        /// 取消订阅事件(3个参数)
        /// </summary>
        public void Unsubscribe<T1, T2, T3>(ulong eventId, Action<T1, T2, T3> action)
        {
            if (null == action)
            {
                Log.LogError(s =>
                {
                    s.Append("取消订阅事件为空, ");
                    s.Append($"事件id:");
                    s.Append(eventId);
                }, ELogType.Event);
                return;
            }

            if (EventSubscriber<Action<T1, T2, T3>>.SubscriberId == 0)
            {
                EventSubscriber<Action<T1, T2, T3>>.SubscriberId = UniqueIDGenerator.ULongID;
            }

            if (!_eventSubscribers.TryGetValue(EventSubscriber<Action<T1, T2, T3>>.SubscriberId, out var subscriber))
            {
                Log.LogWarning(s =>
                {
                    s.Append("事件没有订阅但是试图取消订阅, ");
                    s.Append($"事件id:");
                    s.Append(eventId);
                }, ELogType.Event);
                return;
            }

            if (subscriber is EventSubscriber<Action<T1, T2, T3>> actionSubscriber)
            {
                actionSubscriber.Unsubscribe(eventId, action);
            }
        }

        /// <summary>
        /// 发布事件(3个参数)
        /// </summary>
        public void Publish<T1, T2, T3>(ulong eventId, T1 arg1, T2 arg2, T3 arg3)
        {
            if (EventSubscriber<Action<T1, T2, T3>>.SubscriberId == 0)
            {
                EventSubscriber<Action<T1, T2, T3>>.SubscriberId = UniqueIDGenerator.ULongID;
            }

            if (!_eventSubscribers.TryGetValue(EventSubscriber<Action<T1, T2, T3>>.SubscriberId, out var subscriber))
            {
                return;
            }

            if (subscriber is EventSubscriber<Action<T1, T2, T3>> actionSubscriber)
            {
                actionSubscriber.Publish(eventId, action => action(arg1, arg2, arg3));
            }
        }

        /// <summary>
        /// 延迟发布事件(3个参数)
        /// </summary>
        public void PublishDelay<T1, T2, T3>(ulong eventId, T1 arg1, T2 arg2, T3 arg3)
        {
            if (EventSubscriber<Action<T1, T2, T3>>.SubscriberId == 0)
            {
                EventSubscriber<Action<T1, T2, T3>>.SubscriberId = UniqueIDGenerator.ULongID;
            }

            if (!_eventSubscribers.TryGetValue(EventSubscriber<Action<T1, T2, T3>>.SubscriberId, out var subscriber))
            {
                return;
            }

            if (subscriber is EventSubscriber<Action<T1, T2, T3>> actionSubscriber)
            {
                actionSubscriber.PublishDelay(eventId, action => action(arg1, arg2, arg3));
            }
        }

    }
}