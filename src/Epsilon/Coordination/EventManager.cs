using System;
using System.Collections.Generic;

namespace Epsilon.Coordination
{
    public class EventManager
    {
        private readonly Dictionary<EventType, List<Action>> _subscriptions;

        private readonly Dictionary<EventType, List<Action<object>>> _parameterisedSubscriptions;

        public EventManager()
        {
            _subscriptions = new Dictionary<EventType, List<Action>>();
            _parameterisedSubscriptions = new Dictionary<EventType, List<Action<object>>>();

            var eventTypes = (EventType[]) Enum.GetValues(typeof(EventType));

            foreach (var eventType in eventTypes)
            {
                _subscriptions.Add(eventType, new List<Action>());
                _parameterisedSubscriptions.Add(eventType, new List<Action<object>>());
            }
        }

        public void AddSubscription(EventType eventType, Action action)
        {
            _subscriptions[eventType].Add(action);
        }

        public void AddSubscription(EventType eventType, Action<object> action)
        {
            _parameterisedSubscriptions[eventType].Add(action);
        }

        public void RaiseEvent(EventType eventType)
        {
            foreach (var action in _subscriptions[eventType])
            {
                action.Invoke();
            }
        }

        public void RaiseEvent(EventType eventType, object parameters)
        {
            foreach (var action in _parameterisedSubscriptions[eventType])
            {
                action.Invoke(parameters);
            }
        }
    }
}