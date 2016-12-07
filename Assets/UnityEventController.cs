﻿using System.Collections.Generic;
using System.Linq;

public class UnityEventController
{
    public static bool IsEventReady(int eventId)
    {
        return EventController.IsEventReady(eventId);
    }

    public static bool IsEventAvailable()
    {
        return EventController.IsEventAvailable();
    }

    public static void SpawnAllPlayerEvents()
    {
        EventController.SpawnAllPlayerEvents();
    }

    public List<EventObject> PlayerEventsList
    {
        get { return EventController.PlayerEvents.Values.ToList(); }
    }
}
                   