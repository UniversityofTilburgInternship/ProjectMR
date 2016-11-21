using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnityEventController
{

    public static bool IsEventAvailable()
    {
        return EventController.IsEventAvailable();
    }

    public static void SpawnAllPlayerEvents()
    {
        EventController.SpawnAllPlayerEvents();
    }

    public static void RemoveFromActiveEvents(int eventId)
    {
        EventController.RemoveFromActiveEvents(eventId);
    }

    public List<EventObject> PlayerEventsList
    {
        get { return EventController.PlayerEvents.Values.ToList(); }
    }
}
                                                                                                                                                                                                                                                                                                                                                                    