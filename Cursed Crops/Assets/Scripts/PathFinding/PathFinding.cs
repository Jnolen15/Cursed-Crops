using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class PathFinding : MonoBehaviour
{
    PathRequestManager requestManager;
    AGrid grid;
    private void Awake()
    {
        //requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<AGrid>();
    }

    //public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    //{
        //StartCoroutine(FindPath(startPos, targetPos));
    //}
    public void FindPath(PathRequest request, Action<PathResult> callback, int choosePath)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;
        Node startNode = grid.NodeFromWorldPoint(request.pathStart);
        Node targetNode = grid.NodeFromWorldPoint(request.pathEnd);
        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);
        if (startNode.walkable && targetNode.walkable)
        {
            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;

                    break;
                }
                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }
                    int newMovementCost = currentNode.gCost + getDistance(currentNode, neighbour, choosePath);
                    if (newMovementCost < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCost;
                        neighbour.hCost = getDistance(neighbour, targetNode, choosePath);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
        }
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        //requestManager.FinishedProcessingPath(waypoints, pathSuccess);
        callback(new PathResult(waypoints, pathSuccess, request.callback));
    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        if (path.Count != 1)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector2 directionNew = new Vector2(path[i].gridX - path[i + 1].gridX, path[i].gridY - path[i + 1].gridY);
                if (directionNew != directionOld)
                {
                    waypoints.Add(path[i].worldPosition);
                }
                directionOld = directionNew;
            }
        }
        else
        {
            waypoints.Add(path[0].worldPosition);
        }

        return waypoints.ToArray();
    }
    int getDistance(Node nodeA, Node nodeB, int whatPath)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX) / 2;
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY) / 2;
        if (whatPath == 0)
        {
            int dstX2 = (Mathf.Abs(nodeA.gridX - nodeB.gridX)) ^ 2;
            int dstY2 = (Mathf.Abs(nodeA.gridY - nodeB.gridY)) ^ 2;
            if (dstX2 > dstY2)
            {
                return (10 * dstY2 + 14 * (dstX2 - dstY2)) / 2;
            }
            return (14 * dstX2 + 10 * (dstY2 - dstX2)) / 2;
        }
        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);

    }
}
