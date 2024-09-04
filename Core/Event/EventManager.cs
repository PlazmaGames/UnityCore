using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlazmaGames.Core.Events
{
    public class EventManager
    {
        private IDictionary<int, GameEvent> _events = new Dictionary<int, GameEvent>();

        public void Emit(int eventID, Component sender = null, object data = null)
        {
            try
            {
                if (_events.ContainsKey(eventID))
                {
                    _events[eventID].Invoke(sender, data);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void AddListener(int eventID, EventListener listener)
        {
            if (!_events.ContainsKey(eventID))
            {
                GameEvent e = new GameEvent();
                e.AddListener(listener);
                _events.Add(eventID, e);
            }
            else
            {
                _events[eventID].AddListener(listener);
            }
        }

        public void RemoveListener(int eventID, EventListener listener)
        {
            if (_events.ContainsKey(eventID)) 
            { 

                GameEvent e = _events[eventID];
                if (e.ListenerCount > 0)
                {
                    e.RemoveListener(listener);
                }
                else
                {
                    _events.Remove(eventID);
                }
            }
        }
    }
}
