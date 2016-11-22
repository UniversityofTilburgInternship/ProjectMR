using System.Collections.Generic;
using Casanova.Prelude;
using UnityEngine;

public class EventObject : MonoBehaviour

{
    public int AmountOfParticipants;
    public int Completeness;
    public int Id;
    public int InterestLevel;
    public int MaxAmountOfParticipants;
    public string AnimationName;
    public string ModelName;
    public string Name;
    public string Sound;
    public float Radius;
    public bool IsPlayerControlled;
    public Vector3 Position;

    public List<int> AssociatedActions = new List<int>();
    public List<Tuple<int, int>> PersonalityMinimums = new List<Tuple<int, int>>();

    public static EventObject Instantiate(Event sourceEvent)
    {
        var gameObjectModelName = sourceEvent.ModelName.Equals("") ? "Person1" : sourceEvent.ModelName;
     
        var eventObject = (Instantiate(
                Resources.Load(gameObjectModelName),
                sourceEvent.Position,
                Quaternion.identity)
            as GameObject).GetComponent<EventObject>();

        eventObject.Id = sourceEvent.Id;
        eventObject.InterestLevel = sourceEvent.InterestLevel;
        eventObject.MaxAmountOfParticipants = sourceEvent.MaxAmountOfParticipants;
        eventObject.AnimationName = sourceEvent.AnimationName;
        eventObject.ModelName = sourceEvent.ModelName;
        eventObject.Name = sourceEvent.Name;
        eventObject.Sound = sourceEvent.Sound;
        eventObject.Radius = sourceEvent.Radius;
        eventObject.IsPlayerControlled = sourceEvent.IsPlayerControlled;
        eventObject.AssociatedActions = sourceEvent.AssociatedActions;
        eventObject.PersonalityMinimums = sourceEvent.PersonalityMinimums;

        return eventObject;
    }

    public void Destroy()
    {
        EventController.ActiveEvents.Remove(Id);
        Destroy(gameObject);
    }
}

                                                                                                                                                                                                                                   