using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlazmaGames.Core.Events
{
    public class EventManager
    {
        private IDictionary<string, GameEvent> _events = new Dictionary<string, GameEvent>();

        public void Emit(string eventID, Component sender = null, object data = null)
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

        public void AddListener(string eventID, EventResponse listener)
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

        public void RemoveListener(string eventID, EventResponse listener)
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
