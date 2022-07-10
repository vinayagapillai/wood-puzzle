using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineIndicator : MonoBehaviour
{
    public int[,] line_data = new int[10, 10]
    {
        { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
        {10,11,12,13,14,15,16,17,18,19 },
        {20,21,22,23,24,25,26,27,28,29 },
        {30,31,32,33,34,35,36,37,38,39 },
        {40,41,42,43,44,45,46,47,48,49 },
        {50,51,52,53,54,55,56,57,58,59 },
        {60,61,62,63,64,65,66,67,68,69 },
        {70,71,72,73,74,75,76,77,78,79 },
        {80,81,82,83,84,85,86,87,88,89 },
        {90,91,92,93,94,95,96,97,98,99 }
    };

    public int[] ColumnIndex = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    public (int, int) GetSquarePosition(int squareIndex)
    {
        int pos_row = -1;
        int pos_col = -1;

        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                if (line_data[row, col] == squareIndex)
                {
                    pos_row = row;
                    pos_col = col;
                }
            }
        }

        return (pos_row, pos_col);
    }


    public int[] GetVerticalLine(int squareIndex)
    {
        int[] line = new int[10];

        int squarePosCol = GetSquarePosition(squareIndex).Item2;

        for(int i = 0; i < 10; i++)
        {
            line[i] = line_data[i, squarePosCol];
        }

        return line;
    }

}
