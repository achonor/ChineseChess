using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Achonor
{
    //定时器回调函数
    public delegate void SchedulerCallback();

    public class Scheduler : SingleInstance<Scheduler>
    {
        private static Dictionary<string, SchedulerData> mSchedulers = new Dictionary<string, SchedulerData>();

        private static long mAddDelayCount = 0;

        private static SchedulerCallback mMainThreadRunCallback = null;

        //关闭定时器
        public static void Stop(string name)
        {
            if (null == Instance) {
                return;
            }
            SchedulerData scheduler;
            if (mSchedulers.TryGetValue(name, out scheduler))
            {
                if (null != scheduler.handle)
                {
                    Instance.StopCoroutine(scheduler.handle);
                }
                //移除
                mSchedulers.Remove(name);
            }
        }

        /// <summary>
        /// 创建定时器
        /// </summary>
        /// <param name="name"></param>
        /// <param name="_startInterval"></param>
        /// <param name="_runCount">小于等于0表示不限制次数</param>
        /// <param name="_timeInterval"></param>
        /// <param name="_callback"></param>
        /// <returns></returns>
        public static SchedulerData CreateScheduler(string name, float _startInterval = 0, int _runCount = 0, float _timeInterval = 1.0f, SchedulerCallback _callback = null)
        {
            if (null == Instance) {
                return null;
            }
            if (string.IsNullOrEmpty(name))
            {
                Debug.LogError("The name is illegal");
                return null;
            }
            if (mSchedulers.ContainsKey(name))
            {
                //关闭同名定时器
                Stop(name);
            }
            SchedulerData scheduler = new SchedulerData(name, _startInterval, _runCount, _timeInterval, _callback);
            scheduler.handle = scheduler.RunFunction();
            Instance.StartCoroutine(scheduler.handle);
            //保存
            mSchedulers.Add(name, scheduler);
            return scheduler;
        }

        public static void AddDelay(float delay, Action callback) {
            string name = string.Format("Scheduler.AddDelay_{0}", mAddDelayCount++);
            CreateScheduler(name, delay, 1, 0, () => {
                try {
                    callback?.Invoke();
                } catch (Exception e) {
                    Debug.LogError(e);
                };
            });
        }

        public static void MainThreadRun(SchedulerCallback callback) {
            mMainThreadRunCallback += callback;
        }

        private void Update() {
            if (null != mMainThreadRunCallback) {
                mMainThreadRunCallback.Invoke();
            }
            mMainThreadRunCallback = null;
        }
    }
    public class SchedulerData
    {
        private string name;
        private float startInterval;        //多少秒后启动
        private int runCount;               //运行次数
        private float timeInterval;         //每两次运行的时间间隔
        private SchedulerCallback callback; //回调
        private object param;               //回调参数

        private bool isPause = false;
        public IEnumerator handle;          //用于关闭定时器

        public SchedulerData(string _name, float _startInterval = 0, int _runCount = 0, float _timeInterval = 1.0f, SchedulerCallback _callback = null, object _param = null)
        {
            name = _name;
            startInterval = _startInterval;
            runCount = _runCount;
            timeInterval = _timeInterval;
            callback = _callback;
            param = _param;
        }
        public IEnumerator RunFunction()
        {
            yield return new WaitForSeconds(startInterval);
            int count = 0;
            while (true)
            {
                if (0 < runCount && runCount <= count)
                {
                    break;
                }
                if (isPause)
                {
                    continue;
                }
                if (null != callback)
                {
                    try
                    {
                        callback();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
                count++;
                yield return new WaitForSeconds(timeInterval);
            }
            Scheduler.Stop(name);
        }
    }
}