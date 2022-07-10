using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shape : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject ShapeSquarePrefab;
    public Vector3 ShapeDragScale;
    public Vector2 Offset;
    public int TotalSquareNumber { get; set; }

    [HideInInspector] public ShapeData CurrentShapeData;

    private List<GameObject> _currentShape = new List<GameObject>();
    private Canvas _canvas;
    private RectTransform _transform;
    private Vector3 _startScale;
    //private bool _shapeDragabble;
    private Vector3 _startPosition;
    private bool _shapeActive;

    private void OnEnable()
    {
        GameEvents.MoveShapeToStartPosition += MoveShapeToStartPosition;
        GameEvents.SetShapeInactive += SetShapeInactive;
    }

    private void OnDisable()
    {
        GameEvents.MoveShapeToStartPosition -= MoveShapeToStartPosition;
        GameEvents.SetShapeInactive -= SetShapeInactive;
    }

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        _transform = this.GetComponent<RectTransform>();
        _startScale = this.GetComponent<RectTransform>().localScale;
        _startPosition = this.GetComponent<RectTransform>().localPosition;
        //_shapeDragabble = true;
        _shapeActive = true;
    }

    public bool IsOnStartPosition()
    {
        return _transform.localPosition == _startPosition;
    }
    public bool IsAnyOfSquareActive()
    {
        foreach(GameObject square in _currentShape)
        {
            if (square.activeSelf)
                return true;
        }
        return false;
    }

    public void ActivateShape()
    {
        if (!_shapeActive)
        {
            foreach (GameObject square in _currentShape)
            {
                square?.GetComponent<ShapeSquare>().ActivateSquare();
            }
        }
        _shapeActive = true;
    }
    public void DeactivateShape()
    {
        if (_shapeActive)
        {
            foreach(GameObject square in _currentShape)
            {
                square?.GetComponent<ShapeSquare>().DeactivateSquare();
            }
        }
        _shapeActive = false;
    }

    private void SetShapeInactive()
    {
        if(!IsOnStartPosition() && IsAnyOfSquareActive())
        {
            foreach(GameObject shape in _currentShape)
            {
                shape.SetActive(false);
            }
        }
    }

    public void RequestNewShape(ShapeData shapeData)
    {
        _transform.localPosition = _startPosition;
        CreateShape(shapeData);
    }

    public void CreateShape(ShapeData shapeData)
    {
        CurrentShapeData = shapeData;
        TotalSquareNumber = GetNumberOfActiveSquares(shapeData);

        while(_currentShape.Count <= TotalSquareNumber)
        {
            _currentShape.Add(Instantiate(ShapeSquarePrefab, this.transform) as GameObject); 
        }

        foreach(GameObject square in _currentShape)
        {
            square.transform.position = Vector3.zero;
            square.SetActive(false);
        }

        RectTransform squareRect = ShapeSquarePrefab.GetComponent<RectTransform>();
        Vector2 moveDistance = new Vector2(squareRect.rect.width * squareRect.localScale.x, squareRect.rect.height * squareRect.localScale.y);

        int CurrentIndexInList = 0;

        for(int row = 0; row < shapeData.Rows; row++)
        {
            for(int column = 0; column < shapeData.Columns; column++)
            {
                if (shapeData.board[row].Column[column])
                {
                    _currentShape[CurrentIndexInList].SetActive(true);
                    _currentShape[CurrentIndexInList].GetComponent<RectTransform>().localPosition =
                        new Vector2(GetXPositionForShapeSquare(shapeData, column, moveDistance), GetYPositionForShapeSquare(shapeData, row, moveDistance));
                    CurrentIndexInList++;
                }
            }
        }
    }

    private float GetXPositionForShapeSquare(ShapeData shapeData, int column, Vector2 moveDistance)
    {
        float shiftOnX = 0f;
        if (shapeData.Columns > 1)
        {
            float startXPos;
            if (shapeData.Columns % 2 != 0)
                startXPos = (shapeData.Columns / 2) * moveDistance.x * -1;
            else
                startXPos = ((shapeData.Columns / 2) - 1) * (moveDistance.x * - 1) - (moveDistance.x / 2);
            shiftOnX = startXPos + column * moveDistance.x;

        }
        return shiftOnX;
    }

    private float GetYPositionForShapeSquare(ShapeData shapeData, int row, Vector2 moveDistance)
    {
        float shiftOnY = 0f;
        if (shapeData.Rows > 1)
        {
            float startYPos;
            if (shapeData.Rows % 2 != 0)
                startYPos = (shapeData.Rows / 2) * moveDistance.y;
            else
                startYPos = ((shapeData.Rows / 2) - 1) * moveDistance.y + moveDistance.y / 2;
            shiftOnY = startYPos - row * moveDistance.y;
        }
        return shiftOnY;
    }


    public int GetNumberOfActiveSquares(ShapeData shapeData)
    {
        int number = 0;

        foreach (var rows in shapeData.board)
        {
            foreach(bool active in rows.Column)
            {
                if (active)
                {
                    number++;
                }
            }
        }

        return number;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _transform.anchorMin = Vector2.one * 0.5f;
        _transform.anchorMax = Vector2.one * 0.5f;
        _transform.pivot = Vector2.one * 0.5f;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, eventData.position, Camera.main, out pos);
        _transform.localPosition = pos + Offset;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _transform.localScale = ShapeDragScale;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _transform.localScale = _startScale;
        GameEvents.CheckIfShapeCanBePlaced();
    }

    private void MoveShapeToStartPosition()
    {
        _transform.transform.localPosition = _startPosition;
    }
}
