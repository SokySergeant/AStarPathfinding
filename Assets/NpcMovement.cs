using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class NpcMovement : MonoBehaviour
{
    public AStarPathfinding pathfinder;
    public Transform endObj;
    
    private List<Node> gottenNodes = new List<Node>();
    private bool canMove = true;

    public LayerMask whatIsBarrier;
    public LayerMask whatIsGrid;
    
    public float waitTime;
    private bool smoothMove = false;
    public Toggle smoothMoveToggle;



    private void Update()
    {
        GetPath();
    }



    private void GetPath()
    {
        if (canMove && Input.GetMouseButtonDown(0) && !IsBarrier() && IsInGrid())
        {
            canMove = false;
            
            endObj.position = MouseToGridPos();
            gottenNodes = pathfinder.Pathfind();
            if (gottenNodes != null)
            {
                MoveNpc();
            }
            else
            {
                Debug.Log("No path found");
            }

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
            if (smoothMove)
            {
                while (new Vector2(transform.position.x, transform.position.y) != gottenNodes[i].pos)
                {
                    transform.position = Vector3.Lerp(transform.position, gottenNodes[i].pos, 0.5f);
                    yield return new WaitForFixedUpdate();
                }
                transform.position = gottenNodes[i].pos;
            }
            else
            {
                transform.position = gottenNodes[i].pos;
                yield return new WaitForSeconds(waitTime);
            }
            
        }

        canMove = true;
    }



    private Vector2 MouseToGridPos()
    {
        return new Vector2(Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), Mathf.Round(Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
    }



    private bool IsBarrier()
    {
        return Physics2D.OverlapCircle(MouseToGridPos(), 0.1f, whatIsBarrier) != null;
    }



    private bool IsInGrid()
    {
        return Physics2D.OverlapCircle(MouseToGridPos(), 0.1f, whatIsGrid) != null;
    }



    public void SetSmoothMove()
    {
        smoothMove = smoothMoveToggle.isOn;
    }


    private void OnDrawGizmos()
    {
        if (gottenNodes == null)
            return;
        
        Handles.color = Color.blue;
        
        for (int i = 0; i < gottenNodes.Count; i++)
        {
            if (i + 1 != gottenNodes.Count)
            {
                Handles.DrawLine(gottenNodes[i].pos, gottenNodes[i+1].pos, 2f);
            }
        }
    }
}
