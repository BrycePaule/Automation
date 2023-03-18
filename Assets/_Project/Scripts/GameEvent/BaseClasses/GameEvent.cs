using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    [System.Serializable]
    public class GameEvent<T> : ScriptableObject
    {
        public List<GameEventListener<T>> listeners = new List<GameEventListener<T>>();
        public bool LogToConsole;

        public virtual void Raise(T t)
        {
            // LOGGING
            if (LogToConsole) { Debug.Log(this.ToString() + " was raised.", this); }
            if (listeners.Count == 0) { Debug.Log(this.ToString() + " was raised with no listeners.", this);}

            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(t);
            }
        }

        public virtual void RegisterListener(GameEventListener<T> listener) => listeners.Add(listener);
        public virtual void UnregisterListener(GameEventListener<T> listener) => listeners.Remove(listener);

        public void ClearAllListeners() { listeners = new List<GameEventListener<T>>(); }
    }
}