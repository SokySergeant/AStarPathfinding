using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarGrid : MonoBehaviour
{
    public int gridSize;
    public GameObject[] gridArr = new GameObject[0];
    public GameObject gridSquare;

    [ContextMenu("Create Grid")]
    public void CreateGrid()
    {
        for (int i = 0; i < gridArr.Length; i++)
        {
            Destroy(gridArr[i]);
        }
        
        gridArr = new GameObject[gridSize * gridSize];

        int x = -gridSize / 2;
        int y = gridSize / 2;
        for (int i = 0; i < gridArr.Length; i++)
        {
            x++;
            if (i % gridSize == 0)
            {
                x = -gridSize / 2;
                y--;
            }
            GameObject tempGridSquare = Instantiate(gridSquare, new Vector2(x, y), Quaternion.identity, transform);
            gridArr[i] = tempGridSquare;
        }
    }



    public void SetBarrier(Vector2 location)
    {
        //gridArr.Contains()
    }

    public void SetPath(Vector2 location)
    {
        
    }
}
