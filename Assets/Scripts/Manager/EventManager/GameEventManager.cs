using System;
using System.Collections.Generic;
using UnityEngine;

namespace Castle.CustomUtil.EventManager
{
    public enum GameEvent
    {
        SellHero,
        UpgradeHero,
        Combat,
        HeroDead,
        MonsterDead,
        //Ability
        Hurt,
        StartOfTurn,
        Faint
    }

    /// <summary>
    /// Simple Event Manager to drive event flow.
    /// </summary>
    public class GameEventManager : MonoBehaviour
    {
        private Dictionary<GameEvent, Action<Dictionary<string, object>>> eventDictionary = new Dictionary<GameEvent, Action<Dictionary<string, object>>>();

        public void AddListener(GameEvent eventName, Action<Dictionary<string, object>> listener)
        {
            if (eventDictionary.TryGetValue(eventName, out var newEvent))
            {
                newEvent += listener;
                eventDictionary[eventName] = newEvent;
            }
            else
            {
                newEvent += listener;
                eventDictionary.Add(eventName, newEvent);
            }
        }

        public void RemoveListener(GameEvent eventName, Action<Dictionary<string, object>> listener)
        {
            if (eventDictionary.TryGetValue(eventName, out var lastEvent))
            {
                lastEvent -= listener;
                eventDictionary[eventName] = lastEvent;
            }
        }

        public void Dispatch(GameEvent eventName, Dictionary<string, object> message = null)
        {
            if (eventDictionary.TryGetValue(eventName, out var targetEvent))
            {
                targetEvent.Invoke(message);
            }
        }
    }
}