using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Dictionary<int, GameAction> CurrentNodesCollection;
    public EventObject MyEvent;
    public GenericVector AccumulatedValues;
    public GenericVector PersonalityValuesGenericVector;
    public GraphTraveler GraphTraveler;
    public NpcMovementController MovementController;
    //This is set to a dummy value in order to avoid NPCs freezing on start.
    public Vector3 CurrentActionPosition = new Vector3(-1.0f, -1.0f, -1.0f);

    public static List<NpcObject> AllPersons = new List<NpcObject>();

    public static NpcObject Instantiate(List<Tuple<int, int>> personalityValues)
    {
        var randomIndexForModelName = Random.Range(0, SettingsParser.ModelNames.Count);
        var randomModelname = SettingsParser.ModelNames[randomIndexForModelName];

        var randomIndexForPosition = Random.Range(0, SettingsParser.Spawnpoints.Count);
        var randomPosition = SettingsParser.Spawnpoints[randomIndexForPosition];

        var npcObject = (Instantiate(
                Resources.Load(randomModelname),
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
        return time + 2;
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
            CurrentActionPosition = position;
            //then we request a new path
            PathRequestManager.RequestPath(transform.position, CurrentActionPosition,
                (newPath, pathSuccessful, target) => MovementController.OnPathFound(newPath, pathSuccessful, target));
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
                                                                                                                                                  