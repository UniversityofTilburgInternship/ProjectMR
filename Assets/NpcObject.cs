﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets;
using Casanova.Prelude;
using UnityEngine;

public class NpcObject : MonoBehaviour
{
    public int Id;
    public GameAction CurrentlyActiveAction;
    public bool InEventRadius;
    public bool IsInEvent;
    public bool FirstAction;
    public Animator Animator;
    public int CurrentInteractionTarget;
    public Dictionary<int, Interaction> RequestedInteractions;
    public Dictionary<int, Interaction> InteractionActions;
    public Dictionary<int, GameAction> CurrentNodesCollection;
    public EventObject MyEvent;
    public GenericVector AccumulatedValues;
    public GenericVector PersonalityValuesGenericVector;
    public GraphTraveler GraphTraveler;
    public NpcMovementController MovementController;
    //This is set to a dummy value in order to avoid NPCs freezing on start.
    public Vector3 CurrentActionPosition = new Vector3(-1.0f, -1.0f, -1.0f);

    public static List<NpcObject> AllPersons = new List<NpcObject>();

    public static NpcObject Instantiate(List<Tuple<int, int>> personalityValues, string modelName)
    {
        if (modelName.Equals("random"))
        {
            var randomIndexForModelName = Random.Range(0, SettingsParser.ModelNames.Count);
            modelName = SettingsParser.ModelNames[randomIndexForModelName];
        }
        var randomIndexForPosition = Random.Range(0, SettingsParser.Spawnpoints.Count);
        var randomPosition = SettingsParser.Spawnpoints[randomIndexForPosition];

        var npcObject = (Instantiate(
                Resources.Load(modelName),
                randomPosition,
                Quaternion.identity)
            as GameObject).GetComponent<NpcObject>();

        npcObject.Id = AllPersons.Count;
        npcObject.PersonalityValuesGenericVector = personalityValues.SetUpGenericVector();
        npcObject.AccumulatedValues = new GenericVector(personalityValues.Count);
        npcObject.CurrentNodesCollection = ActionsParser.NormalActions;
        npcObject.InteractionActions = ActionsParser.Interactions;
        npcObject.GraphTraveler = new GraphTraveler(npcObject);
        npcObject.MovementController = NpcMovementController.CreateComponent(npcObject.gameObject, npcObject);
        npcObject.Animator = npcObject.gameObject.GetComponent<Animator>();
        npcObject.gameObject.AddComponent<NavMeshAgent>();
        npcObject.gameObject.AddComponent<AudioSource>();
        npcObject.gameObject.GetComponent<NavMeshAgent>().stoppingDistance = 2.2f;

        var audio = npcObject.gameObject.GetComponent<AudioSource>();
        audio.clip = Resources.Load<AudioClip>("Footstep01");
        audio.playOnAwake = false;
        audio.loop = true;
        audio.rolloffMode = AudioRolloffMode.Linear;
        audio.maxDistance = 5.0f;
        audio.minDistance = 0.0f;
        audio.spatialBlend = 1.0f;
        AllPersons.Add(npcObject);

        return npcObject;
    }

    public float PlayAnimation(int actionId)
    {
        var action = CurrentNodesCollection[actionId];
        Debug.Log(action.ToString());
        var animationName = action.AnimationName;
        var time = 0.0f;
        //Get Animator controller
        var ac = Animator.runtimeAnimatorController;

        foreach (var animationClip in ac.animationClips)
        {
            //If it has the same name as your clip
            if (animationClip.name == animationName)
            {
                time = animationClip.length;
            }
        }
        Animator.SetBool(animationName, true);
        StartCoroutine(StopAnimation(animationName, time));
        return time + 0.1f;
    }

    public bool IsInterestedInEvent()
    {
        if (!(EventController.ActiveEvents.Count > 0))
            return false;

        var eventsInRadius = EventController.ActiveEvents.Values
            .Where(ev => Vector3.Distance(transform.position, ev.Position) <= ev.Radius);

        var eventsInterested = new List<EventObject>();

        var isInterestedEvent = false;

        foreach (var ev in eventsInRadius)
        {
            foreach (var minimum in ev.PersonalityMinimums)
            {
                if (minimum.Item2 > PersonalityValuesGenericVector.Points[minimum.Item1])
                {
                    isInterestedEvent = false;
                    break;
                }
                isInterestedEvent = true;
            }

            if (isInterestedEvent)
                eventsInterested.Add(ev);
        }

        if (!eventsInterested.Any()) return false;
        MyEvent = eventsInterested[Random.Range(0, eventsInterested.Count)];
        return true;
    }

    public void UpdateAccumulatedValues(int actionId)
    {
        AccumulatedValues.Sum(
            CurrentNodesCollection[actionId].PersonalityModifiers.ToGenericVector());
    }

    public void MoveTo(int actionId)
    {
        var action = CurrentNodesCollection[actionId];
        var position = action.ClaimablePositions[Id];

        //if the action is different from the currentaction
        if (CurrentActionPosition != position)
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = position;
        }
    }

    public bool IsPositionAvailable(int actionId)
    {
        var action = _GetAction(actionId);
        return action.IsPositionAvailable(Id);
    }

    public void ClaimPosition(int actionId)
    {
        var action = _GetAction(actionId);
        if (action.ClaimablePositions.ContainsKey(Id))
        {
            RemoveClaimToPosition(actionId);
        }

        action.CreatePosition(Id);
    }

    public Vector3 GetClaimedPosition(int actionId)
    {
        var action = _GetAction(actionId);
        return action.ClaimablePositions[Id];
    }

    public void RemoveClaimToPosition(int actionId)
    {
        if (ActionExists(actionId))
        {
            var action = _GetAction(actionId);
            action.ClaimablePositions.Remove(Id);
        }
    }

    /*
	XML: Store interaction-actions seperately or with bool (which means we separate
	them later)

    CNV: If near and not reacting or acting: Do Routine

    C#:
        If extravert and not receipient:
            get the Npc that is near from cnv
                  send request to npc
                  get response (one call?)
                  do interact anim
                  remove interaction request

              If reponse = good
                  Call moveTo etc

              If received request
                  get best request via genetic algorithm, only one best request.
                  set response / do these 2 in one method: getresponse
    */

    /*
        INTERACTION SENDING
    */

    public int GetNearbyIdleNpcId()
    {
        var nearbyNpcs = new Dictionary<float, NpcObject>();
        foreach (var npc in AllPersons)
        {
            var distance = Vector3.Distance(transform.position, npc.transform.position);
            if (distance < 8.0f)
            {
                nearbyNpcs.Add(distance, npc);
            }
        }

        if (nearbyNpcs.Count > 1)
            return nearbyNpcs.FirstOrDefault(x => x.Key == nearbyNpcs.Keys.Max()).Value.Id;
        else
            return -1;
    }

    public int TrySendInteraction()
    {
        Debug.Log("SENT INTERACTION");
        //CurrentInteractionTarget is set in the UnityNpc proxy
        var receiver = AllPersons[CurrentInteractionTarget];

        if (IsExtravert() && !AlreadySentRequest(receiver))
        {
            var receiverAccumulatedValues = receiver.AccumulatedValues;

            var interaction = GetInteraction();
            interaction.Weight = GenericVector.GetAngle(AccumulatedValues, receiverAccumulatedValues);

            receiver.RequestedInteractions.Add(this.Id, interaction);

            if (receiver.WantsToInteract(this.Id))
                return interaction.Id;
            else
                return -1;
        }
        else
            return -1;
    }

    private bool IsExtravert()
    {
        const int EXTRAVERSION_INDEX = 0;
        var biggestPoint = AccumulatedValues.BiggestPoint();
        Debug.Log("Extraversion AccValues = " + AccumulatedValues.Points[AccumulatedValues.Points.IndexOf(biggestPoint)]);
        return AccumulatedValues.Points.IndexOf(biggestPoint) == EXTRAVERSION_INDEX;
    }

    private bool AlreadySentRequest(NpcObject receiver)
    {
        return receiver.RequestedInteractions.ContainsKey(this.Id);
    }


    /*
        INTERACTION RECEIVING
    */

    //Change this to get it using the genetic algorithm (?)
    private Interaction GetInteraction()
    {
        return InteractionActions[0];
    }

    public bool WantsToInteract(int senderId)
    {
        var requestWithBiggestWeight = GetRequestWithBiggestWeight();
        return requestWithBiggestWeight.Sender.Id == senderId;
    }

    private Interaction GetRequestWithBiggestWeight()
    {
        var allRequestWeights = RequestedInteractions.Select(x => x.Value.Weight).ToList();
        var biggestWeight = allRequestWeights.Max();
        return RequestedInteractions[allRequestWeights.IndexOf(biggestWeight)];
    }


    /* HELPERS */

    private GameAction _GetAction(int actionId)
    {
        var actionForId = CurrentNodesCollection[actionId];
        return actionForId;
    }

    private bool ActionExists(int actionId)
    {
        return CurrentNodesCollection.ContainsKey(actionId);
    }

    private IEnumerator StopAnimation(string animationName, float time)
    {
        yield return new WaitForSeconds(time);
        Animator.SetBool(animationName, false);
    }
}                 