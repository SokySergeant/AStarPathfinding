using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 endPos;
    public Transform endObj;
    
    public LayerMask whatIsBarrier;

    private List<Node> openNodes = new List<Node>();
    private List<Vector2> closedPos = new List<Vector2>();
    private List<Vector2> adjacentPos = new List<Vector2>();

    private List<Vector2> testList = new List<Vector2>();

    private float checkRadius = 0.1f;
    
    


    public List<Node> Pathfind()
    {
        startPos = transform.position;
        endPos = endObj.position;

        openNodes.Clear();
        closedPos.Clear();
        testList.Clear();

        Node startNode = new Node(0, GetDistance(startPos, endPos), startPos);
        openNodes.Add(startNode);

        

        while (openNodes.Count > 0)
        {
            Node currentNode = GetLowestFNode(openNodes);

            openNodes.Remove(currentNode);
            closedPos.Add(currentNode.pos);
            
            if (currentNode.pos == endPos)
            {
                openNodes.Clear();
                closedPos.Clear();
                adjacentPos.Clear();
                return GetPath(currentNode);
            }

            foreach (Vector2 adjacentPos in GetAdjacentPositions(currentNode.pos))
            {
                if (closedPos.Contains(adjacentPos))
                {
                    continue;
                }

                if (Physics2D.OverlapCircle(adjacentPos, checkRadius, whatIsBarrier))
                {
                    closedPos.Add(adjacentPos);
                    continue;
                }

                int newG = currentNode.g + GetDistance(adjacentPos, currentNode.pos);

                Node adjacentNode = openNodes.Find(node => node.pos == adjacentPos);

                if (adjacentNode != null)
                {
                    if (newG < adjacentNode.g)
                    {
                        adjacentNode.g = newG;
                        adjacentNode.cameFromThisNode = currentNode;
                    }
                }
                else
                {
                    adjacentNode = new Node(newG, GetDistance(adjacentPos, endPos), adjacentPos, currentNode);
                    openNodes.Add(adjacentNode);
                }
            }
        }
        
        return null;
    }



    private List<Vector2> GetAdjacentPositions(Vector2 givenPos)
    {
        adjacentPos.Clear();

        adjacentPos.Add(new Vector2(givenPos.x, givenPos.y + 1));
        adjacentPos.Add(new Vector2(givenPos.x, givenPos.y - 1));
        adjacentPos.Add(new Vector2(givenPos.x - 1, givenPos.y));
        adjacentPos.Add(new Vector2(givenPos.x + 1, givenPos.y));
        
        return adjacentPos;
    }


    
    private int GetDistance(Vector2 start, Vector2 end)
    {
        return Mathf.Abs((int)end.x - (int)start.x) + Mathf.Abs((int)end.y - (int)start.y);
    }



    private Node GetLowestFNode(List<Node> givenNodes)
    {
        Node lowestFNode = givenNodes[0];
        
        for (int i = 0; i < givenNodes.Count; i++)
        {
            if (givenNodes[i].f() < lowestFNode.f() || givenNodes[i].f() == lowestFNode.f() && openNodes[i].h < lowestFNode.h)
            {
                lowestFNode = givenNodes[i];
            }
        }

        return lowestFNode;
    }

    

    private List<Node> GetPath(Node lastNode)
    {
        List<Node> path = new List<Node>();
        
        path.Add(lastNode);

        Node previousNode = lastNode;

        while (previousNode.cameFromThisNode != null)
        {
            path.Add(previousNode.cameFromThisNode);
            previousNode = previousNode.cameFromThisNode;
        }

        path.Reverse();
        
        return path;
    }


    private void OnDrawGizmos()
    {
        for (int i = 0; i < testList.Count; i++)
        {
            Gizmos.DrawSphere(testList[i], 0.1f);
        }
    }
}
