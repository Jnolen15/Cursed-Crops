using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> PathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager instance;
    PathFinding pathFinding;

    bool isProcessingPath;
    private void Awake()
    {
        instance = this;
        pathFinding = GetComponent<PathFinding>();
    }
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.PathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if (!isProcessingPath && PathRequestQueue.Count > 0) 
        {
            currentPathRequest = PathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathFinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }
    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}
