using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 endPos;
    public Transform endObj;

    public AStarGrid grid;

    private List<Node> openNodes = new List<Node>();
    private List<Node> closedNodes = new List<Node>();


    public List<Node> Pathfind()
    {
        startPos = transform.position;
        endPos = endObj.position;

        openNodes.Clear();
        closedNodes.Clear();

        Node startNode = new Node(0, GetDistance(startPos, endPos), startPos);
        openNodes.Add(startNode);

        

        while (openNodes.Count > 0)
        {
            Node currentNode = GetLowestFNode(openNodes);

            if (currentNode.pos == endPos)
            {
                return GetPath(currentNode);
            }

            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            foreach (Node adjacentNode in GetAdjacentNodes(currentNode))
            {
                if (closedNodes.Contains(adjacentNode))
                {
                    continue;
                }
                
                if (currentNode.g < adjacentNode.g)
                {
                    adjacentNode.cameFromThisNode = currentNode;
                    if (!openNodes.Contains(adjacentNode))
                    {
                        openNodes.Add(adjacentNode);
                    }
                }
            }
        }
        
        return null;
    }



    private List<Node> GetAdjacentNodes(Node givenNode)
    {
        List<Node> tempNodes = new List<Node>();

        Vector2 upPos = new Vector2(givenNode.pos.x, givenNode.pos.y + 1);
        Vector2 downPos = new Vector2(givenNode.pos.x, givenNode.pos.y - 1);
        Vector2 leftPos = new Vector2(givenNode.pos.x - 1, givenNode.pos.y);
        Vector2 rightPos = new Vector2(givenNode.pos.x + 1, givenNode.pos.y);

        for (int i = 0; i < grid.gridArr.Length; i++)
        {
            Vector2 currentGridPos = new Vector2(grid.gridArr[i].transform.position.x, grid.gridArr[i].transform.position.y);

            if (upPos == currentGridPos)
            {
                tempNodes.Add(new Node(GetDistance(startPos, upPos), GetDistance(upPos, endPos), upPos));
            }

            if (downPos == currentGridPos)
            {
                tempNodes.Add(new Node(GetDistance(startPos, downPos), GetDistance(downPos, endPos), downPos));
            }
            
            if (leftPos == currentGridPos)
            {
                tempNodes.Add(new Node(GetDistance(startPos, leftPos), GetDistance(leftPos, endPos), leftPos));
            }
            
            if (rightPos == currentGridPos)
            {
                tempNodes.Add(new Node(GetDistance(startPos, rightPos), GetDistance(rightPos, endPos), rightPos));
            }
        }
        
        return tempNodes;
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
            if (givenNodes[i].f() < lowestFNode.f())
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
}
