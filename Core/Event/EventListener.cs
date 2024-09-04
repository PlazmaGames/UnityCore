using PlazmaGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlazmaGames.Core.Events
{
    [System.Serializable]
    public class GameEventCallback : UnityEvent<Component, object> { }

    [System.Serializable]
    public class EventListener
    {
        public int priority = 0; 
        public GameEventCallback response = new GameEventCallback();

        internal void Invoke(Component sender, object data)
        {
            response?.Invoke(sender, data);
        }
    }
}
