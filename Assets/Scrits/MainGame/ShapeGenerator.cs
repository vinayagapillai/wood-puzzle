using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator : Singleton<ShapeGenerator>
{
    public List<Shape> ShapeList;
    public List<ShapeData> ShapeData;

    private void OnEnable()
    {
        GameEvents.RequestNewShape += RequestNewShape;
    }

    private void OnDisable()
    {
        GameEvents.RequestNewShape -= RequestNewShape;
    }

    private void RequestNewShape()
    {
        foreach(Shape shape in ShapeList)
        {
            int randomShapeData = UnityEngine.Random.Range(0, ShapeData.Count);
            shape.RequestNewShape(ShapeData[randomShapeData]);
        }
    }

    private void Start()
    {
        foreach(Shape i in ShapeList)
        {
            i.CreateShape(ShapeData[UnityEngine.Random.Range(0, ShapeData.Count)]);
        }
    }

    public Shape GetCurrentSelectedShape()
    {
        foreach(Shape shape in ShapeList)
        {
            if(!shape.IsOnStartPosition() && shape.IsAnyOfSquareActive())
            {
                return shape;
            }
        }
        Debug.Log("no active shape selected");
        return null;
    }
}
