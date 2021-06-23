using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GridManager))]
    public class GridManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var gridManager = target as GridManager;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Fill Grid"))
            {
                gridManager.FillGrid();
            }
            
            if (GUILayout.Button("Clear Grid"))
            {
                gridManager.ClearGrid();
            }

            if (GUILayout.Button("Snap"))
            {
                gridManager.Snap();
            }
            GUILayout.EndHorizontal();
        }
    }
}