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
    private LineIndicator _lineIndicator;

    private void OnEnable()
    {
        GameEvents.CheckIfShapeCanBePlaced += CheckIfShapeCanBePlaced;
    }

    private void OnDisable()
    {
        GameEvents.CheckIfShapeCanBePlaced -= CheckIfShapeCanBePlaced;
    }

    private void Start()
    {
        _lineIndicator = GetComponent<LineIndicator>();
        CreateGrid();
    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetGridSquarePostions();
    }

    private void SpawnGridSquares()
    {
        int squareIndex = 0;
        for(int i= 0; i < Rows; i++)
        {
            for(int j = 0; j < Columns; j++)
            {
                GameObject gridSquare = Instantiate(GridSquarePrefab) as GameObject;
                _gridSquares.Add(gridSquare);
                gridSquare.GetComponent<GridSquare>().SquareIndex = squareIndex;
                gridSquare.transform.SetParent(this.transform);
                gridSquare.transform.localScale = Vector3.one * SquareScale;
                squareIndex++;
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

    public void CheckIfShapeCanBePlaced()
    {
        List<int> squareIndexs = new List<int>();
        foreach(GameObject i in _gridSquares)
        {
            GridSquare square = i.GetComponent<GridSquare>();
            if (square.Selected && !square.SquareOccupied) 
            {
                squareIndexs.Add(square.SquareIndex);
                square.Selected = false;
                //square.ActivateSquare();
            }
        }

        Shape currentShape = ShapeGenerator.Instance.GetCurrentSelectedShape();
        if (currentShape == null) return;

        if(currentShape.TotalSquareNumber == squareIndexs.Count)
        {
            foreach(int squareIndex in squareIndexs)
            {
                _gridSquares[squareIndex].GetComponent<GridSquare>().PlaceShapeOnBoard();
                //play sound
                SoundManager.Instance.PlayAudio(SoundManager.Instance.CLICK);
            }

            //check if all shapes are used
            int shapesLeft = 0;
            foreach(Shape shape in ShapeGenerator.Instance.ShapeList)
            {
                if(shape.IsAnyOfSquareActive() && shape.IsOnStartPosition())
                {
                    shapesLeft++;
                }
            }

            if(shapesLeft == 0)
            {
                GameEvents.RequestNewShape();
            }
            else
            {
                GameEvents.SetShapeInactive();
            }

            CheckIfAnyLineCompleted();
        }
        else
        {
            GameEvents.MoveShapeToStartPosition();
        }

    }

    void CheckIfAnyLineCompleted()
    {
        List<int[]> lines = new List<int[]>();

        foreach(int i in _lineIndicator.ColumnIndex)
        {
            lines.Add(_lineIndicator.GetVerticalLine(i));
        }

        for(int row = 0; row < 10; row++)
        {
            List<int> data = new List<int>();
            for(int col = 0; col < 10; col++)
            {
                data.Add(_lineIndicator.line_data[row, col]);
            }
            lines.Add(data.ToArray());
        }

        int completedLines = CheckIfSquaresAreCompleted(lines);
        if (completedLines > 0)
            SoundManager.Instance.PlayAudio(SoundManager.Instance.LEVELUP);

        //Update Scores
        GameEvents.AddScore(10 * completedLines);

        CheckIfPlayerLost();
    }

    private int CheckIfSquaresAreCompleted(List<int[]> data)
    {
        List<int[]> completedLines = new List<int[]>();

        int linesCompleted = 0;

        foreach(int[] line in data)
        {
            bool lineCompleted = true;
            foreach(int squareIndex in line)
            {
                GridSquare square = _gridSquares[squareIndex].GetComponent<GridSquare>();
                if (!square.SquareOccupied)
                {
                    lineCompleted = false;
                }
            }

            if (lineCompleted)
            {
                completedLines.Add(line);
            }
        }

        foreach(int[] line in completedLines)
        {
            bool completed = false;

            foreach(int squareIndex in line)
            {
                GridSquare square = _gridSquares[squareIndex].GetComponent<GridSquare>();
                square.Deactivate();
                square.ClearOccupied();
                completed = true;
            }

            if (completed)
                linesCompleted++;
        }

        return linesCompleted;
    }


    private void CheckIfPlayerLost()
    {
        int validShapes = 0;

        for(int i = 0; i < ShapeGenerator.Instance.ShapeList.Count; i++)
        {
            bool isShapeActive = ShapeGenerator.Instance.ShapeList[i].IsAnyOfSquareActive();
    
            if(CheckIfShapeCanBePlacedOnGrid(ShapeGenerator.Instance.ShapeList[i]) && isShapeActive)
            {
                ShapeGenerator.Instance.ShapeList[i]?.ActivateShape();
                validShapes++;
            }
        }

        if(validShapes == 0)
        {
            //Game Over
            GameEvents.ShowGameOverScreen();
            SoundManager.Instance.PlayAudio(SoundManager.Instance.GAMEOVER);
            //print("game over");
        }

    }

    private bool CheckIfShapeCanBePlacedOnGrid(Shape currentShape)
    {
        ShapeData currentShapeData = currentShape.CurrentShapeData;
        int shapeColumns = currentShapeData.Columns;
        int shapeRows = currentShapeData.Rows;

        List<int> originalShapeFilledUpSquares = new List<int>();
        int squareIndex = 0;

        for(int row = 0; row < shapeRows; row++)
        {
            for(int col = 0; col < shapeColumns; col++)
            {
                if (currentShapeData.board[row].Column[col])
                {
                    originalShapeFilledUpSquares.Add(squareIndex);
                }

                squareIndex++;
            }
        }

        List<int[]> squareList = GetAllSquareCombination(shapeRows, shapeColumns);

        bool canBePlaced = false;

        foreach(var number in squareList)
        {
            bool shapeCanBePlacedOnTheBoard = true;
            foreach(int index in originalShapeFilledUpSquares)
            {
                GridSquare square = _gridSquares[number[index]].GetComponent<GridSquare>();
                if (square.SquareOccupied)
                {
                    shapeCanBePlacedOnTheBoard = false;
                }
            }

            if (shapeCanBePlacedOnTheBoard)
                canBePlaced = true;
        }

        return canBePlaced;
        
    }

    private List<int[]> GetAllSquareCombination(int rows, int columns)
    {
        List<int[]> squareList = new List<int[]>();
        int lastColumnIndex = 0;
        int lastRowIndex = 0;

        int safeIndex = 0;

        while(lastRowIndex + (rows - 1) < 10)
        {
            List<int> rowsData = new List<int>();
            for(int row = lastRowIndex; row < (lastRowIndex + rows); row++)
            {
                for(int column = lastColumnIndex; column < (lastColumnIndex + columns); column++)
                {
                    rowsData.Add(_lineIndicator.line_data[row, column]);
                }
            }
            squareList.Add(rowsData.ToArray());

            lastColumnIndex++;
            if(lastColumnIndex + (columns - 1) >= 10)
            {
                lastRowIndex++;
                lastColumnIndex = 0;
            }

            safeIndex++;
            if (safeIndex > 100)
                break;
        }

        return squareList;

    }

}
