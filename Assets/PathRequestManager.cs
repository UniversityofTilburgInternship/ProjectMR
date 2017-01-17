﻿﻿﻿using UnityEngine;
using System.Collections.Generic;
using System;

public class PathRequestManager : MonoBehaviour
{
    private bool _isProcessingPath;
    private Pathfinding _pathfinding;
    private PathRequest _currentPathRequest;
    private static PathRequestManager _instance;

    private readonly Queue<PathRequest> _pathRequestQueue = new Queue<PathRequest>();

    void Awake()
    {
        _instance = this;
        _pathfinding = GetComponent<Pathfinding>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool, Vector3> callback)
    {
        var newRequest = new PathRequest(pathStart, pathEnd, callback);
        _instance._pathRequestQueue.Enqueue(newRequest);
        _instance.TryProcessNext();
    }

    public void FinishedProcessingPath(Vector3[] path, bool success, Vector3 target)
    {
        _currentPathRequest.Callback(path, success, target);
        _isProcessingPath = false;
        TryProcessNext();
    }

    private void TryProcessNext()
    {
        if (!_isProcessingPath && _pathRequestQueue.Count > 0)
        {
            _currentPathRequest = _pathRequestQueue.Dequeue();
            _isProcessingPath = true;
            _pathfinding.StartFindPath(_currentPathRequest.PathStart, _currentPathRequest.PathEnd);
        }
    }

    private struct PathRequest
    {
        public readonly Vector3 PathStart;
        public readonly Vector3 PathEnd;
        public readonly Action<Vector3[], bool, Vector3> Callback;

        public PathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool, Vector3> callback)
        {
            PathStart = start;
            PathEnd = end;
            Callback = callback;
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             