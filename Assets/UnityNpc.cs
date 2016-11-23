﻿﻿using System.Collections.Generic;
using System.Linq;
using Casanova.Prelude;
using UnityEngine;

public class UnityNpc : MonoBehaviour
{
    private NpcObject _npcObject;

    public int Id
    {
        get { return _npcObject.Id; }
    }

    public bool IsInEventRadius
    {
        get { return _npcObject.InEventRadius; }
        set { _npcObject.InEventRadius = value; }
    }

    public Vector3 Position
    {
        get { return _npcObject.transform.position; }
        set { _npcObject.transform.position = value; }
    }

    public List<int> ActionsToPerform
    {
        get
        {
            _npcObject.GraphTraveler.Actions = _npcObject.CurrentNodesCollection;
            var path = _npcObject.GraphTraveler.GetBestPath();
            return path;
        }
    }

    public bool IsInEvent
    {
        get { return _npcObject.IsInEvent; }
        set { _npcObject.IsInEvent = value; }
    }

    public Vector3 CurrentActionPosition
    {
        get { return _npcObject.CurrentActionPosition; }
    }

    public bool IsInterestedInEvent()
    {
        return _npcObject.IsInterestedInEvent();
    }

    public void StopMovement()
    {
       // _npcObject.gameObject.GetComponent<NavMeshAgent>().Stop();
        //_npcObject.MovementController.Stop();
    }

    public static UnityNpc Spawn(List<Tuple<int, int>> personalityValues, string prefabname)
    {
        var unityNpc = new UnityNpc {_npcObject = NpcObject.Instantiate(personalityValues, prefabname)};
        return unityNpc;
    }

    public void MoveTo(int actionId)
    {
        _npcObject.MoveTo(actionId);
    }

    public bool IsPositionAvailable(int actionId)
    {
        return _npcObject.IsPositionAvailable(actionId);
    }

    public void ClaimPosition(int actionId)
    {
        _npcObject.ClaimPosition(actionId);
    }

    public Vector3 GetClaimedPosition(int actionId)
    {
        return _npcObject.GetClaimedPosition(actionId);
    }

    public void RemoveClaimToPosition(int actionId)
    {
        _npcObject.RemoveClaimToPosition(actionId);
    }


    public void UpdateAccumulatedValues(int actionId)
    {
        _npcObject.UpdateAccumulatedValues(actionId);  
    }

    public float PlayAnimation(int actionId)
    {
        return _npcObject.PlayAnimation(actionId);
    }

    public void RemoveClaimToAllPositions()
    {
        foreach (var action in _npcObject.CurrentNodesCollection)
        {
            RemoveClaimToPosition(action.Value.Id);
        }
    }

    public void UpdateCurrentNodesCollection()
    {
        _npcObject.CurrentNodesCollection = _npcObject.IsInEvent
            ? GetAssociatedActionsForEventId(_npcObject.MyEvent.Id)
            : ActionsParser.NormalActions;
    }

    private static Dictionary<int, GameAction> GetAssociatedActionsForEventId(int eventId)
    {
        var currentEvent = ActionsParser.Events[eventId];

        return
            currentEvent.AssociatedActions.ToDictionary(x => x, x => ActionsParser.EventActions[x]);
    }
}                                                                                          