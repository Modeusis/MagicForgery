using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Utilities
{
    public class EventBus : IDisposable
    {
        private Dictionary<Type, List<Delegate>> _events;

        private Dictionary<Type, int> _thisFrameEvents;

        public EventBus()
        {
            _events = new Dictionary<Type, List<Delegate>>();
            
            _thisFrameEvents = new Dictionary<Type, int>();
        }

        public void Subscribe<T>(Action<T> action)
        {
            if (!_events.TryGetValue(typeof(T), out var events))
            {
                events = new List<Delegate>();
                _events[typeof(T)] = events;
            }
            
            events.Add(action);
        }

        public void Unsubscribe<T>(Action<T> action)
        {
            if (_events.TryGetValue(typeof(T), out var events))
            {
                events.Remove(action);

                if (events.Count == 0)
                {
                    _events.Remove(typeof(T));
                }
            }
        }

        public void Publish<T>(T eventData)
        {
            if (_events.TryGetValue(typeof(T), out var list))
            {
                _thisFrameEvents[typeof(T)] = Time.frameCount;
                
                var listeners = new List<Delegate>(list);
                
                foreach (var listener in listeners)
                {
                    try
                    {
                        ((Action<T>)listener).Invoke(eventData);
                    }
                    catch (Exception exception)
                    {
                        UnityEngine.Debug.LogError($"EventBus: exception in event handler {typeof(T)}: {exception}");
                    }
                }
            }
        }

        public bool WasInvokedThisFrame<T>()
        {
            _thisFrameEvents.TryGetValue(typeof(T), out var frameCount);
            
            if (frameCount == Time.frameCount)
            {
                return true;
            }
            
            return false;
        }
        
        public void Dispose()
        {
            _events.Clear();
            
            _thisFrameEvents.Clear();
        }

        // Необходимая реализация для оптимизации нужно посоветоваться
        // private void ClearThisFrameEvents()
        // {
        //     if (_thisFrameEvents.Count == 0)
        //     {
        //         return;
        //     }
        //
        //     _thisFrameEvents.Clear();
        // }
    }
}