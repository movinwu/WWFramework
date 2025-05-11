/*------------------------------
 * 脚本名称: DelayInvokeHandler
 * 创建者: movin
 * 创建日期: 2025/04/30
------------------------------*/

using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WWFramework
{
    /// <summary>
    /// 延迟调用处理
    /// </summary>
    public class DelayInvokeHandler
    {
        /// <summary>
        /// 延迟调用事件
        /// </summary>
        private Action _action;
        
        /// <summary>
        /// 延迟时间
        /// </summary>
        private float _time;

        /// <summary>
        /// 计时器
        /// </summary>
        private float _timer;

        /// <summary>
        /// 是否正在延迟调用
        /// </summary>
        private bool _isDelaying = false;
        
        /// <summary>
        /// 是否暂停
        /// </summary>
        private bool _isPaused = false;

        /// <summary>
        /// 延迟调用
        /// </summary>
        /// <param name="delayAction">延迟调用的事件</param>
        /// <param name="delayTime">延迟时间</param>
        public void DelayInvoke(Action delayAction, float delayTime)
        {
            _action = delayAction;
            _time = Mathf.Max(delayTime, 0);
            _timer = 0;
            _isPaused = false;
            Delay().Forget();
        }

        /// <summary>
        /// 延迟计时,时间到达时执行延迟调用事件
        /// </summary>
        private async UniTask Delay()
        {
            if (_isDelaying)
            {
                return;
            }

            _isDelaying = true;
            while (_timer < _time)
            {
                if (!_isPaused)
                {
                    _timer += Time.deltaTime;
                }
                await UniTask.DelayFrame(1);
            }
            InvokeImmediately();
            _isDelaying = false;
        }

        /// <summary>
        /// 立即执行延迟调用事件
        /// </summary>
        public void InvokeImmediately()
        {
            _action?.Invoke();
            Cancel();
        }

        /// <summary>
        /// 暂停延迟计时
        /// </summary>
        public void Pause()
        {
            _isPaused  = true;
        }
        
        /// <summary>
        /// 恢复延迟计时
        /// </summary>
        public void Resume()
        {
            _isPaused  = false;   
        }

        /// <summary>
        /// 取消延迟计时及调用
        /// </summary>
        public void Cancel()
        {
            _action = null;
            _time = 0;
            _timer = 0;
            _isPaused = false;
        }
        
        ~DelayInvokeHandler()
        {
            Cancel();
        }
    }
}