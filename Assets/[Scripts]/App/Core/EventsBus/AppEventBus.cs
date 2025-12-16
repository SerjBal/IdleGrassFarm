using System;
using System.Collections.Generic;

namespace Serjbal.Core
{
    public class AppEventBus<TEvent> : IEventBus<TEvent> where TEvent : class
    {
        private readonly Dictionary<Type, List<Delegate>> _handlers = new Dictionary<Type, List<Delegate>>();

        public void Subscribe<T>(Action<T> handler) where T : TEvent
        {
            var type = typeof(T);
            if (!_handlers.ContainsKey(type))
            {
                _handlers[type] = new List<Delegate>();
            }
            _handlers[type].Add(handler);
        }

        public void Unsubscribe<T>(Action<T> handler) where T : TEvent
        {
            var type = typeof(T);
            if (_handlers.ContainsKey(type))
            {
                _handlers[type].Remove(handler);
                if (_handlers[type].Count == 0)
                {
                    _handlers.Remove(type);
                }
            }
        }

        public void Raise<T>(T @event) where T : TEvent
        {
            var type = typeof(T);
            if (_handlers.ContainsKey(type))
            {
                // Копируем список для безопасной итерации
                var handlers = _handlers[type].ToArray();
                foreach (var handler in handlers)
                {
                    (handler as Action<T>)?.Invoke(@event);
                }
            }
        }
    }
}

