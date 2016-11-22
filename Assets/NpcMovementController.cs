using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class NpcMovementController : MonoBehaviour
{
    private int _targetIndex;
    private List<Vector3> _waypoints;
    private NpcObject _npcObject;
    private bool _walking;

    public static NpcMovementController CreateComponent(GameObject parentobject, NpcObject npcObject)
    {
        var npcMovementController = parentobject.AddComponent<NpcMovementController>();
        npcMovementController._npcObject = npcObject;
        return npcMovementController;
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful, Vector3 target)
    {
        //if the path is found and is succesfull
        if (pathSuccessful)
        {
            _waypoints = newPath.ToList();
            _waypoints.Add(target);
            _targetIndex = 0;

            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }
  
    public void Stop()
    {
        StopWalking();
        StopCoroutine("FollowPath");
    }

    private IEnumerator FollowPath()
    {
        var moveSpeed = Time.deltaTime * 2.75f;
        const float rotationSpeed = 20.0f;
        if (_waypoints.Count == 0)
        {
            StopCoroutine("FollowPath");
            yield break;
        }
        var currentWaypoint = _waypoints[0];
        while (true)
        {
            if (_npcObject.transform.position == currentWaypoint)
            {
                StopWalking();
                _targetIndex++;
                if (_targetIndex >= _waypoints.Count)
                {
                    StopCoroutine("FollowPath");
                    yield break;
                }
                currentWaypoint = _waypoints[_targetIndex];
            }

            const float angle = 10;
            if (
                Vector3.Angle(_npcObject.transform.forward,
                    currentWaypoint - _npcObject.transform.position) < angle)
            {
                StartWalking();
            }
            else
            {
                StopWalking();
                //check if walking animation stopped. To prevent from ugly movement
                if (!_npcObject.Animator.GetCurrentAnimatorStateInfo(0).IsName("walking"))
                {
                    var direction = currentWaypoint - _npcObject.transform.position;
                    _npcObject.transform.rotation =
                        Quaternion.LookRotation(
                            Vector3.RotateTowards(_npcObject.transform.forward, direction, rotationSpeed, 0)
                        );
                }
            }

            //walk towards the next waypoint
            _npcObject.transform.position = Vector3.MoveTowards(_npcObject.transform.position,
                currentWaypoint,
                moveSpeed);
            yield return null;
        }
    }

    private void StopWalking()
    {
        _npcObject.Animator.SetBool("Walking", false);
        _walking = false;
    }

    private void StartWalking()
    {
        if (!_walking)
        {
            _npcObject.Animator.SetBool("Walking", true);
            _walking = true;
        }
    }
}

                                                                                                                                                                                                                                                                                                                                                                                                  