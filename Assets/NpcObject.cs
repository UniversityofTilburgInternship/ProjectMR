﻿﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
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

    public bool IsInteractionTarget;
    public NpcObject CurrentInteractionTarget;
    public NpcObject InteractionSender;
    public Dictionary<int, NpcObject> InteractionRequesters = new Dictionary<int, NpcObject>();

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
        npcObject.GraphTraveler = new GraphTraveler(npcObject);
        npcObject.MovementController = NpcMovementController.CreateComponent(npcObject.gameObject, npcObject);
        npcObject.Animator = npcObject.gameObject.GetComponent<Animator>();
        npcObject.gameObject.AddComponent<NavMeshAgent>();
        npcObject.gameObject.GetComponent<NavMeshAgent>().stoppingDistance = 2.2f;
        npcObject.gameObject.GetComponent<NavMeshAgent>().speed = 2.0f;
        npcObject.gameObject.GetComponent<NavMeshAgent>().angularSpeed = 12.0f;


        npcObject.gameObject.AddComponent<AudioSource>();
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
            transform.LookAt(position);
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
        INTERACTION SENDING
    */

    public void FreeInteractionTarget()
    {
        CurrentInteractionTarget.InteractionRequesters.Remove(this.Id);
        CurrentInteractionTarget.IsInteractionTarget = false;
        CurrentInteractionTarget.InteractionSender = null;
    }

    public bool InteractionAvailable()
    {
        if (GetNearbyIdleNpcId() != null && !IsInteractionTarget)
        {
            var nearestNpc = GetNearbyIdleNpcId();
            return HandleInteraction(nearestNpc);
        }
        else
            return false;
    }

    private bool HandleInteraction(NpcObject npc)
    {
        //not sure about the else
        if (!AlreadySentRequest(npc))
            npc.InteractionRequesters.Add(this.Id, this);

        CurrentInteractionTarget = npc;

        if (CurrentInteractionTarget.WantsToInteractWith(this.Id))
        {
            CurrentInteractionTarget.InteractionSender = this;
            CurrentInteractionTarget.IsInteractionTarget = true;
            return true;
        }
        else
            return false;
    }

    public void SetActionPositions(NpcObject npcObject, Vector3 NewPosition)
    {
        foreach (var gameAction in npcObject.CurrentNodesCollection.Values)
        {
            gameAction.Position = NewPosition;
        }
    }

    private NpcObject GetNearbyIdleNpcId()
    {
        var nearbyNpcs = new Dictionary<NpcObject, float>();

        foreach (var npc in AllPersons)
        {
            if (npc.Id != this.Id && !(npc.InteractionRequesters.Count > 1))
            {
                var distance = Vector3.Distance(transform.position, npc.transform.position);
                nearbyNpcs.Add(npc, distance);
            }
        }

        if (nearbyNpcs.Count > 1)
            return nearbyNpcs.FirstOrDefault(x => x.Value == nearbyNpcs.Values.Min()).Key;
        else
            return null;
    }

    private bool IsIntrovert()
    {
        const int INTROVERSION_INDEX = 3;
        var biggestPoint = AccumulatedValues.BiggestPoint();
        return AccumulatedValues.Points.IndexOf(biggestPoint) == INTROVERSION_INDEX;
    }


    private bool AlreadySentRequest(NpcObject receiver)
    {
        return receiver.InteractionRequesters.ContainsKey(this.Id)
               || this.InteractionRequesters.ContainsKey(receiver.Id);
    }


    /*
        INTERACTION RECEIVING / HELPERS
    */

    private bool WantsToInteractWith(int senderId)
    {
        if (InteractionRequesters.ContainsKey(senderId))
        {
            var bestRequester = GetBestInteractionSender();
            return bestRequester.Id == senderId;
        }
        else
            return false;
    }

    //wait until requests is big
    private NpcObject GetBestInteractionSender()
    {
        var IDsAndWeights = new Dictionary<int, float>();

        foreach (var requester in InteractionRequesters.Values)
        {
            float weight = GenericVector.DotProduct(this.AccumulatedValues, requester.AccumulatedValues);
            IDsAndWeights.Add(requester.Id, weight);
        }
        var idOfBiggestWeightRequester =
            (from x in IDsAndWeights where x.Value == IDsAndWeights.Max(v => v.Value) select x.Key).FirstOrDefault();
        return AllPersons.First(x => x.Id == idOfBiggestWeightRequester);
    }


    /* HELPERS */

    //make action positions be the interaciton position
    public void ChangeActionPositions(Vector3 newPosition)
    {
        foreach (var action in CurrentNodesCollection)
        {
            action.Value.Position = newPosition;
        }
    }

    public Vector3 GetVectorForInteraction(string type)
    {
        if (type.Equals("InteractionSender"))
            return (transform.position + CurrentInteractionTarget.transform.position).normalized;
        else
            return (transform.position + InteractionSender.transform.position).normalized;
    }

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
                                                                                                                                         