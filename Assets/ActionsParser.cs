﻿﻿﻿﻿﻿using System.Collections.Generic;
﻿using System.CodeDom;
using System.Linq;
using System.Runtime.InteropServices;
using Assets;
using Casanova.Prelude;
 using UnityEngine;


public static class ActionsParser
{
    private const string REACTIONS = "reactions";
    private const string INTERACTIONS = "interactions";
    private const string ACTIONS = "actions";
    private const string EVENTACTIONS = "eventActions";
    private const string EVENTS = "events";

    public static Dictionary<int, Event> Events = new Dictionary<int, Event>();
    public static Dictionary<int, Event> PlayerEvents = new Dictionary<int, Event>();
    public static Dictionary<int, GameAction> EventActions = new Dictionary<int, GameAction>();
    public static Dictionary<int, GameAction> NormalActions = new Dictionary<int, GameAction>();
    public static Dictionary<int, GameAction> Interactions = new Dictionary<int, GameAction>();
    public static Dictionary<int, GameAction> Reactions = new Dictionary<int, GameAction>();


    public static void ParseReactions()
    {
        var reactions = XmlNodule.Load(REACTIONS);
        foreach (var reaction in reactions)
        {
            var reactionInstance = new Reaction
            {
                Id = reaction.Get("id").ToInt(),
                Position = reaction.Get("position").ToVector3(),
                ActionName = reaction.Get("name").ToString(),
                AnimationName = reaction.Get("animationname").ToString(),
                PersonalityModifiers = GetNodePersonalityModifiers(reaction)
            };
            Reactions.Add(reactionInstance.Id, reactionInstance);
        }
    }

    public static void ParseInteractions()
    {
        var interactions = XmlNodule.Load(INTERACTIONS);
        foreach (var interaction in interactions)
        {
            var interactionInstance = new InteractionAction
            {
                Id = interaction.Get("id").ToInt(),
                Position = interaction.Get("position").ToVector3(),
                ActionName = interaction.Get("name").ToString(),
                AnimationName = interaction.Get("animationname").ToString(),
                PersonalityModifiers = GetNodePersonalityModifiers(interaction)
            };
            Interactions.Add(interactionInstance.Id, interactionInstance);
        }
    }

    public static void ParseNormalActions()
    {
        var actions = XmlNodule.Load(ACTIONS);
        foreach (var action in actions)
        {
            var actionInstance = new NormalAction
            {
                Id = action.Get("actionId").ToInt(),
                Position = action.Get("position").ToVector3(),
                ActionName = action.Get("actionname").ToString(),
                AnimationName = action.Get("animationname").ToString(),
                PersonalityModifiers = GetNodePersonalityModifiers(action)
            };

            foreach (var neighbour in action.Get("neighbours"))
            {
                if (neighbour.ToString() != "")
                    actionInstance.NeighbourIds.Add(neighbour.ToInt());
            }
            NormalActions.Add(actionInstance.Id, actionInstance);
        }
    }

    public static void ParseEventsActions()
    {
        var eventActionsNodule = XmlNodule.Load(EVENTACTIONS);

        foreach (var eventAction in eventActionsNodule)
        {
            var eventActionObject = new EventAction
            {
                Id = eventAction.Get("id").ToInt(),
                ActionName = eventAction.Get("name").ToString(),
                AnimationName = eventAction.Get("animationname").ToString(),
                Position = eventAction.Get("position").ToVector3(),
                PersonalityModifiers = GetNodePersonalityModifiers(eventAction)
            };

            EventActions.Add(eventActionObject.Id, eventActionObject);
        }
    }

    public static void ParseEvents()
    {
        var eventsNodule = XmlNodule.Load(EVENTS);

        foreach (var Event in eventsNodule)
        {
            var eventObject = new Event
            {
                Id = Event.Get("id").ToInt(),
                Name = Event.Get("name").ToString(),
                ModelName = Event.Get("modelname").ToString(),
                IsPlayerControlled = Event.Get("playercontrolled").ToBool(),
                TriggerKey = Event.Get("triggerkey").ToString(),
                Radius = Event.Get("radius").ToInt(),
                AnimationName = Event.Get("animationname").ToString(),
                Position = Event.Get("position").ToVector3(),
                InterestLevel = Event.Get("interestlevel").ToInt(),
                MaxAmountOfParticipants = Event.Get("maxamountofparticipants").ToInt()
            };

            var personalityMinimums = new List<Tuple<int, int>>();
            foreach (var minimum in Event.Get("personalityMinimums"))
            {
                var personalityId = minimum.Get("id").ToInt();
                var personalityMinimum = minimum.Get("value").ToInt();
                var newMinimumTuple = new Tuple<int, int>(personalityId, personalityMinimum);
                personalityMinimums.Add(newMinimumTuple);
            }
            eventObject.PersonalityMinimums = personalityMinimums;

            var associatedActionIds = Event.Get("associatedactions").Select(actionId => actionId.ToInt()).ToList();
            eventObject.AssociatedActions = associatedActionIds;

            if (eventObject.IsPlayerControlled)
                PlayerEvents.Add(eventObject.Id, eventObject);
            else
                Events.Add(eventObject.Id, eventObject);
        }
    }

    private static Dictionary<int, int> GetNodePersonalityModifiers(XmlNodule nodule)
    {
        return nodule.Get("modifiers")
            .ToDictionary(modifier => modifier.Get("id").ToInt(), modifier => modifier.Get("value").ToInt());
    }
}
                                                                                                                                         