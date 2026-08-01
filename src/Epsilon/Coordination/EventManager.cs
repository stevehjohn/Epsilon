using System;
using System.Collections.Generic;

namespace Epsilon.Coordination;

public class EventManager
{
    private readonly Dictionary<EventType, List<Action>> _subscriptions;

    public EventManager()
    {
        _subscriptions = new Dictionary<EventType, List<Action>>();

        var eventTypes = Enum.GetValues<EventType>();

        foreach (var eventType in eventTypes)
        {
            _subscriptions.Add(eventType, []);
        }
    }

    public void AddSubscription(EventType eventType, Action action)
    {
        _subscriptions[eventType].Add(action);
    }

    public void RaiseEvent(EventType eventType)
    {
        foreach (var action in _subscriptions[eventType])
        {
            action.Invoke();
        }
    }
}