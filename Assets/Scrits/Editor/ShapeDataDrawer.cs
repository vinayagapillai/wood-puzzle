using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShapeData), false)]
[CanEditMultipleObjects]
[System.Serializable]
public class ShapeDataDrawer : Editor
{
    private ShapeData ShapeDataInstance => target as ShapeData;

    public void DrawBoardTable()
    {
        GUIStyle tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        GUIStyle headerColumnStyle = new GUIStyle();
        headerColumnStyle.fixedWidth = 65;
        headerColumnStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle rowStyle = new GUIStyle();
        rowStyle.fixedWidth = 25;
        rowStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle dataFieldStyle = new GUIStyle(EditorStyles.miniButtonMid);
        dataFieldStyle.normal.background = Texture2D.grayTexture;
        dataFieldStyle.onNormal.background = Texture2D.whiteTexture;

        for (int i = 0; i < ShapeDataInstance.Rows; i++)
        {
            EditorGUILayout.BeginHorizontal(headerColumnStyle);
            
            for(int j = 0; j < ShapeDataInstance.Columns; j++)
            {
                EditorGUILayout.BeginHorizontal(rowStyle);
                bool data = EditorGUILayout.Toggle(ShapeDataInstance.board[i].Column[j], dataFieldStyle);
                ShapeDataInstance.board[i].Column[j] = data;
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    public void DrawColumnsInputField()
    {
        int columnsTemp = ShapeDataInstance.Columns;
        int rowsTemp = ShapeDataInstance.Rows;

        ShapeDataInstance.Columns = EditorGUILayout.IntField("Columns", ShapeDataInstance.Columns);
        ShapeDataInstance.Rows = EditorGUILayout.IntField("Rows", ShapeDataInstance.Rows);

        if((ShapeDataInstance.Columns != columnsTemp || ShapeDataInstance.Rows != rowsTemp) && ShapeDataInstance.Columns > 0 && ShapeDataInstance.Rows > 0)
        {
            ShapeDataInstance.CreateNewBoard();
        }
    }

    public void ClearBoardButton()
    {
        if(GUILayout.Button("Clear Board"))
        {
            ShapeDataInstance.Clear();
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ClearBoardButton();
        EditorGUILayout.Space();

        DrawColumnsInputField();
        EditorGUILayout.Space();

        if(ShapeDataInstance.board != null && ShapeDataInstance.Columns > 0 && ShapeDataInstance.Rows > 0)
        {
            DrawBoardTable();
        }

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(ShapeDataInstance);
        }
    }
}
