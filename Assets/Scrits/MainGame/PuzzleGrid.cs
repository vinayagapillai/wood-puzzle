using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGrid : MonoBehaviour
{
    public int Rows = 10;
    public int Columns = 10;

    //public float SquareGap;
    public GameObject GridSquarePrefab;
    public Vector2 StartPostion;
    public float SquareScale;
    public float EverySquareOffset;

    private Vector2 _offset;
    private List<GameObject> _gridSquares = new List<GameObject>();

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetGridSquarePostions();
    }

    private void SpawnGridSquares()
    {
        for(int i= 0; i < Rows; i++)
        {
            for(int j = 0; j < Columns; j++)
            {
                GameObject gridSquare = Instantiate(GridSquarePrefab) as GameObject;
                _gridSquares.Add(gridSquare);
                gridSquare.transform.SetParent(this.transform);
                gridSquare.transform.localScale = Vector3.one * SquareScale;
            }
        }
    }

    private void SetGridSquarePostions()
    {
        int columnNumber = 0;
        int rowNumber = 0;

        RectTransform squareRect = _gridSquares[0].GetComponent<RectTransform>();

        _offset.x = squareRect.rect.width * squareRect.transform.localScale.x + EverySquareOffset;
        _offset.y = squareRect.rect.height * squareRect.transform.localScale.y + EverySquareOffset;

        foreach(GameObject square in _gridSquares)
        {
            if(columnNumber + 1 > Columns)
            {
                columnNumber = 0;
                rowNumber++;
            }

            float pos_x_offset = (_offset.x * columnNumber);
            float pos_y_offset = (_offset.y * rowNumber);

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(StartPostion.x + pos_x_offset, StartPostion.y - pos_y_offset);
            square.GetComponent<RectTransform>().localPosition = new Vector3(StartPostion.x + pos_x_offset, StartPostion.y - pos_y_offset, 0);

            columnNumber++;

        }
    }

    
     
}
