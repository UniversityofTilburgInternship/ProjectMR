﻿﻿﻿﻿﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

    void Update()
    {
        var audio = _npcObject.GetComponent<AudioSource>();
        if (Vector3.Distance(_npcObject.gameObject.GetComponent<NavMeshAgent>().destination, transform.position) <= 2.2)
        {
            StopWalking();
            audio.Stop();
        }

        else
        {
            StartWalking();
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
            var audio = _npcObject.GetComponent<AudioSource>();
            audio.Play();

        }
    }
}
                                                                                                                                                                                                                                                                                     