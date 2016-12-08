﻿using System.Collections.Generic;
using System.Linq;
using Assets;
using UnityEngine;
using Random = UnityEngine.Random;

public static class EventController
{
    private static readonly HashSet<int> PreviouslyUsedIndexes = new HashSet<int>();
    private static readonly HashSet<int> ActiveEventIds = new HashSet<int>();
    public static Dictionary<int, EventObject> ActiveEvents = new Dictionary<int, EventObject>();
    public static Dictionary<int, EventObject> SpawnedPlayerEvents = new Dictionary<int, EventObject>();
    public static readonly Dictionary<int, EventObject> PlayerEvents = new Dictionary<int, EventObject>();

    public static EventObject GetPlayerEvent()
    {
        while (true)
        {
            var randomIndex = Random.Range(0, PlayerEvents.Count - 1);
            var potentialPlayerEvent = PlayerEvents.ElementAt(randomIndex).Value;

            //might run out
            if (!SpawnedPlayerEvents.ContainsValue(potentialPlayerEvent))
            {
                SpawnedPlayerEvents.Add(randomIndex, potentialPlayerEvent);
                return potentialPlayerEvent;
            }
        }
    }

    public static EventObject SpawnRandomEvent()
    {
        var randomEventIndex = GetRandomUniqueEventIndex();
        var Event = ActionsParser.Events[randomEventIndex];

        var eventObject = EventObject.Instantiate(Event);

        if (!ActiveEvents.ContainsKey(eventObject.Id))
        {
            ActiveEvents.Add(eventObject.Id, eventObject);
            ActiveEvents[eventObject.Id].IsReady = true;
            EventPlayer.PlayEventAmbience(eventObject);
        }
        ActiveEventIds.Add(eventObject.Id);

        return eventObject;
    }

    public static void SpawnAllPlayerEvents()
    {
        foreach (var playerEvent in ActionsParser.PlayerEvents.Values)
        {
            var eventObject = EventObject.Instantiate(playerEvent);
            PlayerEvents.Add(eventObject.Id, eventObject);
        }
    }

    //PlayerEvents are only ready for having their completion level upped when they are activated by the player.
    //!!!!!!Activeevents event id not ready on normal event
    public static bool IsEventReady(int eventId)
    {
        return ActiveEvents.ContainsKey(eventId) && ActiveEvents[eventId].IsReady;
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

                             