using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public static class EventController
{
    private static readonly HashSet<int> PreviouslyUsedIndexes = new HashSet<int>();
    private static readonly HashSet<int> ActiveEventIds = new HashSet<int>();
    public static Dictionary<int, EventObject> ActiveEvents = new Dictionary<int, EventObject>();
    public static readonly Dictionary<int, EventObject> PlayerEvents = new Dictionary<int, EventObject>();



    public static EventObject SpawnRandomEvent()
    {
        var randomEventIndex = GetRandomUniqueEventIndex();
        var Event = ActionsParser.Events[randomEventIndex];

        var eventObject = EventObject.Instantiate(Event);

        if (!ActiveEvents.ContainsKey(eventObject.Id))
            ActiveEvents.Add(eventObject.Id, eventObject);

        ActiveEventIds.Add(eventObject.Id);

        return eventObject;
    }

    public static void SpawnAllPlayerEvents()
    {
        var playerEvents = ActionsParser.Events.Values.Where(x => x.IsPlayerControlled).ToList();
        foreach (var playerEvent in playerEvents)
        {
            var eventObject = EventObject.Instantiate(playerEvent);
            PlayerEvents.Add(eventObject.Id, eventObject);
        }
    }

    public static bool IsEventAvailable()
    {
        var possibleActions = new HashSet<int>(ActionsParser.Events.Keys);
        possibleActions.ExceptWith(PreviouslyUsedIndexes.Skip(System.Math.Max(0, PreviouslyUsedIndexes.Count - 5)));

        return possibleActions.Any();
    }

    private static int GetRandomUniqueEventIndex()
    {
        var events = ActionsParser.Events
            .Where(x => !x.Value.IsPlayerControlled)
            .Select(x => x.Key)
            .ToList();

        var possibleActions = new HashSet<int>(events);
        possibleActions.ExceptWith(PreviouslyUsedIndexes.Skip(System.Math.Max(0, PreviouslyUsedIndexes.Count - 5)));

        if (!possibleActions.Any())
            return Random.Range(0, events.Count);

        return possibleActions.ElementAt(Random.Range(0, possibleActions.Count));
    }
}
                        