using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static Action CheckIfShapeCanBePlaced;
    public static Action MoveShapeToStartPosition;
    public static Action RequestNewShape;
    public static Action SetShapeInactive;

    public static Action<int> AddScore;
    public static Action ShowGameOverScreen;

}
