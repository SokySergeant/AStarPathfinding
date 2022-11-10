using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int g;
    public int h;

    public int f()
    {
        return g + h;
    }
    
    public Vector2 pos;
    public Node cameFromThisNode;

    public Node(int g, int h, Vector2 pos, Node cameFromThisNode = null)
    {
        this.g = g;
        this.h = h;
        this.pos = pos;
        this.cameFromThisNode = cameFromThisNode;
    }
}
