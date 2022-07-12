using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class PathRequestManager : MonoBehaviour
{
    //Queue<PathRequest> PathRequestQueue = new Queue<PathRequest>();
    //PathRequest currentPathRequest;
    public int goodPathing = 0;

    Queue<PathResult> results = new Queue<PathResult>();

    static PathRequestManager instance;
    PathFinding pathFinding;

    bool isProcessingPath;
    private void Awake()
    {
        instance = this;
        pathFinding = GetComponent<PathFinding>();
    }

    private void FixedUpdate()
    {
        if(results.Count > 0)
        {
            int itemsqueue = results.Count;
            lock (results)
            {
                for(int i = 0; i < itemsqueue; i++)
                {
                    PathResult result = results.Dequeue();
                    result.callback(result.path, result.success);
                }
            }
        }
    }
    //public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    public static void RequestPath(PathRequest request, int goodPath)
    {
        ThreadStart threadStart = delegate
        {
            instance.pathFinding.FindPath (request, instance.FinishedProcessingPath, goodPath);
        };
        threadStart.Invoke();
        //PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        //instance.PathRequestQueue.Enqueue(newRequest);
        //instance.TryProcessNext();
    }

    /*void TryProcessNext()
    {
        if (!isProcessingPath && PathRequestQueue.Count > 0) 
        {
            currentPathRequest = PathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathFinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }*/

    //public void FinishedProcessingPath(Vector3[] path, bool success, PathRequest originalRequest)
    public void FinishedProcessingPath(PathResult result)
    {
        //PathResult result = new PathResult(path, success, originalRequest.callback);
        lock (results)
        {
            results.Enqueue(result);
        }
        //currentPathRequest.callback(path, success);
        //isProcessingPath = false;
        //TryProcessNext();
    }

    
    
}

public struct PathResult
{
    public Vector3[] path;
    public bool success;
    public Action<Vector3[], bool> callback;

    public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
    {
        this.path = path;
        this.success = success;
        this.callback = callback;

    }

}
public struct PathRequest
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
