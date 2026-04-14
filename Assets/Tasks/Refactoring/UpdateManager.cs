using System.Collections.Generic;
using UnityEngine;

namespace EvgeniiMaklaev.Refactoring
{
    public enum UpdateMode
    {
        EveryFrame, // every frame
        FrameInterval, // every x frames
        TimeInterval // every x seconds
    }

    public class UpdateManager : MonoBehaviour
    {
        public static UpdateManager Instance;

        private readonly List<IUpdateable> _everyFrame = new();
        private readonly Dictionary<int, List<IUpdateable>> _frameBuckets = new();
        private readonly List<int> _frameKeys = new();
        private readonly Dictionary<float, List<IUpdateable>> _timeBuckets = new();
        private readonly Dictionary<float, float> _timers = new();
        private readonly List<float> _timeKeys = new();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Register(IUpdateable item, UpdateMode mode, float value = 0)
        {
            switch (mode)
            {
                case UpdateMode.EveryFrame:
                    if (!_everyFrame.Contains(item)) _everyFrame.Add(item);
                    break;
                case UpdateMode.FrameInterval:
                    int frameInterval = (int)value;
                    if (!_frameBuckets.TryGetValue(frameInterval, out var frameList))
                    {
                        frameList = new List<IUpdateable>();
                        _frameBuckets[frameInterval] = frameList;
                        _frameKeys.Add(frameInterval);
                    }
                    if (!frameList.Contains(item)) frameList.Add(item);
                    break;
                case UpdateMode.TimeInterval:
                    if (!_timeBuckets.TryGetValue(value, out var timeList))
                    {
                        timeList = new List<IUpdateable>();
                        _timeBuckets[value] = timeList;
                        _timers[value] = 0f;
                        _timeKeys.Add(value);
                    }
                    if (!timeList.Contains(item)) timeList.Add(item);
                    break;
            }
        }

        public void Unregister(IUpdateable item)
        {
            _everyFrame.Remove(item);
            for (int i = 0; i < _frameKeys.Count; i++) _frameBuckets[_frameKeys[i]].Remove(item);
            for (int i = 0; i < _timeKeys.Count; i++) _timeBuckets[_timeKeys[i]].Remove(item);
        }

        private void Update()
        {
            for (int i = _everyFrame.Count - 1; i >= 0; i--)
            {
                _everyFrame[i].OnUpdate();
            }

            for (int i = 0; i < _frameKeys.Count; i++)
            {
                int interval = _frameKeys[i];
                if (Time.frameCount % interval == 0)
                {
                    var list = _frameBuckets[interval];
                    for (int j = list.Count - 1; j >= 0; j--)
                    {
                        list[j].OnUpdate();
                    }
                }
            }

            for (int i = 0; i < _timeKeys.Count; i++)
            {
                float interval = _timeKeys[i];
                _timers[interval] += Time.deltaTime;

                if (_timers[interval] >= interval)
                {
                    _timers[interval] = 0;
                    var list = _timeBuckets[interval];
                    for (int j = list.Count - 1; j >= 0; j--)
                    {
                        list[j].OnUpdate();
                    }
                }
            }
        }
    }
}