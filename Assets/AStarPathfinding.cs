using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    private Vector2 _startPos;
    private Vector2 _endPos;
    public Transform endObj;
    
    public LayerMask whatIsBarrier;

    private List<Node> _openNodes = new List<Node>();
    private HashSet<Vector2> _closedPositions = new HashSet<Vector2>();
    private List<Vector2> _adjacentPositions = new List<Vector2>();

    private float _checkRadius = 0.1f;
    
    


    public List<Node> Pathfind()
    {
        _startPos = transform.position;
        _endPos = endObj.position;

        _openNodes.Clear();
        _closedPositions.Clear();

        Node startNode = new Node(0, GetDistance(_startPos, _endPos), _startPos);
        _openNodes.Add(startNode);

        

        while (_openNodes.Count > 0)
        {
            Node currentNode = GetLowestFNode(_openNodes);

            _openNodes.Remove(currentNode);
            _closedPositions.Add(currentNode.pos);
            
            if (currentNode.pos == _endPos)
            {
                _openNodes.Clear();
                _closedPositions.Clear();
                _adjacentPositions.Clear();
                return GetPath(currentNode);
            }

            foreach (Vector2 adjacentPos in GetAdjacentPositions(currentNode.pos))
            {
                if (_closedPositions.Contains(adjacentPos))
                {
                    continue;
                }

                if (Physics2D.OverlapCircle(adjacentPos, _checkRadius, whatIsBarrier))
                {
                    _closedPositions.Add(adjacentPos);
                    continue;
                }

                int newG = currentNode.g + GetDistance(adjacentPos, currentNode.pos);

                Node adjacentNode = _openNodes.Find(node => node.pos == adjacentPos);

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
                    adjacentNode = new Node(newG, GetDistance(adjacentPos, _endPos), adjacentPos, currentNode);
                    _openNodes.Add(adjacentNode);
                }
            }
        }
        
        return null;
    }



    private List<Vector2> GetAdjacentPositions(Vector2 givenPos)
    {
        _adjacentPositions.Clear();

        _adjacentPositions.Add(new Vector2(givenPos.x, givenPos.y + 1));
        _adjacentPositions.Add(new Vector2(givenPos.x, givenPos.y - 1));
        _adjacentPositions.Add(new Vector2(givenPos.x - 1, givenPos.y));
        _adjacentPositions.Add(new Vector2(givenPos.x + 1, givenPos.y));
        
        return _adjacentPositions;
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
            if (givenNodes[i].f() < lowestFNode.f() || givenNodes[i].f() == lowestFNode.f() && _openNodes[i].h < lowestFNode.h)
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
