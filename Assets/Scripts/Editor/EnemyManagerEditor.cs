using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyManager))]
public class EnemyManagerEditor : UnityEditor.Editor
{

public override void OnInspectorGUI()
{
    base.OnInspectorGUI();
    var enemyManager = target as EnemyManager;

    GUILayout.BeginHorizontal();
    if (GUILayout.Button("Fill Grid"))
    {
        enemyManager.FillGrid();
    }
            
            
    if (GUILayout.Button("Clear Grid"))
    {
        enemyManager.ClearGrid();
    }

    if (GUILayout.Button("Snap"))
    {
        enemyManager.Snap();
    }
    GUILayout.EndHorizontal();
}
}
