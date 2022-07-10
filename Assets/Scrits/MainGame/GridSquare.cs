using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class GridSquare : MonoBehaviour
{
    public Image HoverImage;
    public Image ActiveImage;

    public bool Selected { get; set; }
    public int SquareIndex { get; set; }
    public bool SquareOccupied { get; set; }

    private void Start()
    {
        Selected = false;
        SquareOccupied = false;
    }

    public bool CanWeUseThisSquare()
    {
        return HoverImage.gameObject.activeSelf;
    }
    public void PlaceShapeOnBoard()
    {
        ActivateSquare();
    }

    public void ActivateSquare()
    {
        HoverImage.gameObject.SetActive(false);
        ActiveImage.gameObject.SetActive(true);
        Selected = true;
        SquareOccupied = true;
    }

    public void Deactivate()
    {
        ActiveImage.gameObject.SetActive(false);
    }

    public void ClearOccupied()
    {
        Selected = false;
        SquareOccupied = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!SquareOccupied)
        {
            Selected = true;
            HoverImage.gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Selected = true;
        if (!SquareOccupied)
        {
            HoverImage.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!SquareOccupied)
        {
            Selected = false;
            HoverImage.gameObject.SetActive(false);
        }
    }

 
}
