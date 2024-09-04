using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlazmaGames.Core.Events
{
    public class GameEvent
    {
        private IList<EventListener> _listeners = new List<EventListener>();

        public int ListenerCount { get => _listeners.Count; }

        public void Invoke(Component sender, object data)
        {
            foreach (EventListener listener in _listeners.OrderBy(e => -e.priority)) listener.Invoke(sender, data);
        }

        public void AddListener(EventListener listener)
        {
            _listeners.Add(listener);
        }

        public void RemoveListener(EventListener listener)
        {
            if (_listeners.Contains(listener))
            {
                _listeners.Remove(listener);
            }
        }
    }
}
