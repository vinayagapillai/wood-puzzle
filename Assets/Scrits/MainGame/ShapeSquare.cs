using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSquare : MonoBehaviour
{
    public void ActivateSquare()
    {
        gameObject.SetActive(true);
    }
    public void DeactivateSquare()
    {
        gameObject.SetActive(false);
    }
}
