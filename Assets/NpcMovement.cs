using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMovement : MonoBehaviour
{
    public AStarPathfinding pathfinder;
    public AStarGrid grid;
    public Transform endObj;
    
    private List<Node> gottenNodes = new List<Node>();
    private bool canMove = true;
    


    private void Update()
    {
        GetPath();
    }



    private void GetPath()
    {
        if (canMove && Input.GetMouseButtonDown(0) && ContainsSquare())
        {
            canMove = false;
            
            endObj.position = MouseToGridPos();
            gottenNodes = pathfinder.Pathfind();
            MoveNpc();
        }
    }



    private void MoveNpc()
    {
        if (gottenNodes != null)
        {
            StartCoroutine(MoveNpcRoutine());
        }
    }
    
    private IEnumerator MoveNpcRoutine()
    {
        for (int i = 0; i < gottenNodes.Count; i++)
        {
            while (new Vector2(transform.position.x, transform.position.y) != gottenNodes[i].pos)
            {
                transform.position = Vector3.Slerp(transform.position, gottenNodes[i].pos, 0.5f);
                yield return new WaitForFixedUpdate();
            }
            transform.position = gottenNodes[i].pos;
        }

        canMove = true;
    }



    private Vector2 MouseToGridPos()
    {
        return new Vector2(Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
    }



    private bool ContainsSquare()
    {
        for (int i = 0; i < grid.gridArr.Length; i++)
        {
            Vector2 currentGridPos = new Vector2(grid.gridArr[i].transform.position.x, grid.gridArr[i].transform.position.y);
            if (MouseToGridPos() == currentGridPos)
            {
                return true;
            }
        }

        return false;
    }
    
    
    
}
