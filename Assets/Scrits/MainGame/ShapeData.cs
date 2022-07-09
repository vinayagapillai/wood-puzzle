using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class ShapeData : ScriptableObject
{
    [System.Serializable]
    public class Row
    {
        public bool[] Column;
        private int _size = 0;
        public Row(int size)
        {
            CreateRow(size);
        }

        public void CreateRow(int size)
        {
            _size = size;
            Column = new bool[_size];
            ClearRow();
        }

        public void ClearRow()
        {
            for (int i = 0; i < _size; i++)
            {
                Column[i] = false;
            }                        
        }
    }

    public int Columns = 0;
    public int Rows = 0;
    public Row[] board;


    public void CreateNewBoard()
    {
        board = new Row[Rows];
        for(int i = 0; i < Rows; i++)
        {
            board[i] = new Row(Columns);
        }
    }

    public void Clear()
    {
        for(int i = 0; i < Rows; i++)
        {
            board[i].ClearRow();
        }
    }



}
