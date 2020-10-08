/*
 * Arthur Cousseau, 2019
 * https://www.linkedin.com/in/arthurcousseau/
 * Please share this if you enjoy it! :)
*/

using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;

namespace Array2DEditor
{
    public abstract class Array2DEditor : Editor
    {
        private const int margin = 5;

        protected SerializedProperty gridSize;
        protected SerializedProperty cells;

        private Rect lastRect;
        protected Vector2Int newGridSize;
        private bool gridSizeChanged = false;
        private bool wrongSize = false;

        private Vector2 cellSize;

        private MethodInfo boldFontMethodInfo = null;

        /// <summary>
        /// In pixels.
        /// </summary>
        protected virtual int CellWidth { get { return 16; } }
        /// <summary>
        /// In pixels;
        /// </summary>
        protected virtual int CellHeight { get { return 16; } }

        protected virtual int m_RowIndexNumWidth { get { return 16; } }
        protected virtual int m_ColumnIndexNumWidth { get { return 16; } }

        protected virtual bool m_IsEnableChangeCellColor { get { return true; } }

        // 特別需求Cell要用的顏色
        protected Dictionary<int, Dictionary<int, Color>> m_SpecialCellColorData = new Dictionary<int, Dictionary<int, Color>>();

        protected abstract void SetValue(SerializedProperty cell, int i, int j);
        protected virtual void OnEndInspectorGUI() { }

        protected virtual void OnCellBtnClick(SerializedProperty cell, int i, int j)
        {

        }

        public virtual void OnEnable()
        {
            gridSize = serializedObject.FindProperty("gridSize");
            cells = serializedObject.FindProperty("cells");

            newGridSize = gridSize.vector2IntValue;

            cellSize = new Vector2(CellWidth, CellHeight);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update(); // Always do this at the beginning of InspectorGUI.

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginChangeCheck();

                SetBoldDefaultFont(gridSizeChanged);
                newGridSize = EditorGUILayout.Vector2IntField("Grid Size", newGridSize);
                SetBoldDefaultFont(false);
                gridSizeChanged = newGridSize != gridSize.vector2IntValue;
                wrongSize = (newGridSize.x <= 0 || newGridSize.y <= 0);

                GUI.enabled = gridSizeChanged && !wrongSize;

                if (GUILayout.Button("Apply", EditorStyles.miniButton))
                {
                    bool operationAllowed = false;

                    if (newGridSize.x < gridSize.vector2IntValue.x || newGridSize.y < gridSize.vector2IntValue.y) // Smaller grid
                    {
                        operationAllowed = EditorUtility.DisplayDialog("Are you sure?", "You're about to reduce the width or height of the grid. This may erase some cells. Do you really want this?", "Yes", "No");
                    }
                    else // Bigger grid
                    {
                        operationAllowed = true;
                    }

                    if (operationAllowed)
                    {
                        InitNewGrid(newGridSize);
                    }
                }

                GUI.enabled = true;
            }
            EditorGUILayout.EndHorizontal();

            if (wrongSize)
            {
                EditorGUILayout.HelpBox("Wrong size.", MessageType.Error);
            }

            EditorGUILayout.Space();

            if (Event.current.type == EventType.Repaint)
            {
                lastRect = GUILayoutUtility.GetLastRect();
            }

            DisplayGrid(lastRect);

            OnEndInspectorGUI();

            serializedObject.ApplyModifiedProperties(); // Apply changes to all serializedProperties - always do this at the end of OnInspectorGUI.
        }

        private void InitNewGrid(Vector2 newSize)
        {
            cells.ClearArray();

            for (int i = 0; i < newSize.y; i++)
            {
                cells.InsertArrayElementAtIndex(i);
                SerializedProperty row = GetRowAt(i);

                for (int j = 0; j < newSize.x; j++)
                {
                    row.InsertArrayElementAtIndex(j);

                    SetValue(row.GetArrayElementAtIndex(j), i, j);
                }
            }

            gridSize.vector2IntValue = newGridSize;
        }

        protected virtual void DisplayGrid(Rect startRect)
        {
            Rect cellPosition = startRect;

            cellPosition.y += 10; // Same as EditorGUILayout.Space(), but in Rect

            cellPosition.size = cellSize;

            float startLineX = cellPosition.x;

            //for (int i = 0; i < gridSize.vector2IntValue.y; i++)
            // Ascending Order 遞增的方式排列
            for (int i = (gridSize.vector2IntValue.y-1); i >= 0; i--)
            {
                SerializedProperty row = GetRowAt(i);
                cellPosition.x = startLineX; // Get back to the beginning of the line

                EditorGUI.LabelField(cellPosition, i.ToString());
                cellPosition.x += m_RowIndexNumWidth;
                for (int j = 0; j < gridSize.vector2IntValue.x; j++)
                {
                    // 改為產生按鈕
                    if (this is Array2DIntButtonEditor)
                    {
                        GUILayout.BeginArea(cellPosition);
                        SerializedProperty TempSerializedProperty = row.GetArrayElementAtIndex(j);
                        Color OldBackgroundColor = GUI.backgroundColor;
                        if (m_IsEnableChangeCellColor)
                        {
                            Color NewBackgroundColor = GetCellNewBackgroundColor(TempSerializedProperty, j, i);
                            GUI.backgroundColor = NewBackgroundColor;
                        }
                        // 如果需要改變文字顏色時
                        //var style = new GUIStyle(GUI.skin.button);
                        //style.normal.textColor = Color.red;
                        //style.active.textColor = Color.green;
                        int Num = row.GetArrayElementAtIndex(j).intValue;
                        //if (GUILayout.Button(new GUIContent(Num.ToString()), style))
                        if (GUILayout.Button(new GUIContent(Num.ToString())))
                        {
                            OnCellBtnClick(TempSerializedProperty, i, j);
                        }
                        if (m_IsEnableChangeCellColor)
                        {
                            GUI.backgroundColor = OldBackgroundColor;
                        }
                        GUILayout.EndArea();
                    }
                    else
                    {
                        SerializedProperty TempSerializedProperty = row.GetArrayElementAtIndex(j);
                        Color OldBackgroundColor = GUI.backgroundColor;
                        if (m_IsEnableChangeCellColor)
                        {
                            Color NewBackgroundColor = GetCellNewBackgroundColor(TempSerializedProperty, j, i);
                            GUI.backgroundColor = NewBackgroundColor;
                        }
                        EditorGUI.PropertyField(cellPosition, TempSerializedProperty, GUIContent.none);
                        if (m_IsEnableChangeCellColor)
                        {
                            GUI.backgroundColor = OldBackgroundColor;
                        }
                    }
                    cellPosition.x += cellSize.x + margin;
                }

                cellPosition.y += cellSize.y + margin;
                GUILayout.Space(cellSize.y + margin); // If we don't do this, the next things we're going to draw after the grid will be drawn on top of the grid
            }

            cellPosition.x = startLineX; // Get back to the beginning of the line
            cellPosition.x += m_ColumnIndexNumWidth;

            var centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.MiddleCenter;
            for (int i = 0; i < gridSize.vector2IntValue.x; i++)
            {
                EditorGUI.LabelField(cellPosition, (i).ToString(), centeredStyle);
                cellPosition.x += cellSize.x + margin;
            }
            GUILayout.Space(cellSize.y + margin); // If we don't do this, the next things we're going to draw after the grid will be drawn on top of the grid
        }

        // 取得Cell要用的背景的顏色
        protected Color GetCellNewBackgroundColor(SerializedProperty serializedProperty, int x, int y)
        {
            Color NewColor = Color.white;

            // 檢查是否有在特別需求Cell要用的顏色資料中
            if (CheckSpecialCellColorData(x, y))
            {
                NewColor = GetSpecialCellColor(x, y);
                return NewColor;
            }

            if (this is Array2DIntButtonEditor)
            {
                int Num = serializedProperty.intValue;
                if (Num > 0)
                {
                    Color OrangeColor = new Color(1, 0.5f, 0);
                    Color OrangeRedColor = new Color(1, 0.27f, 0);
                    if (Num <= 3)
                    {
                        int TempNumForColor = Num + 3;
                        NewColor = Color.Lerp(Color.white, Color.yellow, TempNumForColor / 7f);
                    }
                    else if (Num <= 7)
                    {
                        int TempNumForColor = Num - 3;
                        NewColor = Color.Lerp(Color.yellow, OrangeColor, TempNumForColor / 7f);
                    }
                    else
                    {
                        int TempNumForColor = Num - 7;
                        NewColor = Color.Lerp(OrangeColor, OrangeRedColor, TempNumForColor / 16f);
                    }
                }
            }
            else
            {
                //NewColor = Color.cyan;
            }

            return NewColor;
        }

        // 檢查此座標是否有在特別需求Cell要用的顏色資料中
        protected bool CheckSpecialCellColorData(int x, int y)
        {
            Dictionary<int, Color> TempCellColorData = new Dictionary<int, Color>();
            if (!m_SpecialCellColorData.TryGetValue(x, out TempCellColorData))
            {
                return false;
            }

            if (!TempCellColorData.ContainsKey(y))
            {
                return false;
            }

            return true;
        }

        // 取得此座標在特別需求Cell要用的顏色
        protected Color GetSpecialCellColor(int x, int y)
        {
            Color NewColor = Color.white;

            Dictionary<int, Color> TempCellColorData = new Dictionary<int, Color>();
            if(!m_SpecialCellColorData.TryGetValue(x, out TempCellColorData))
            {
                return NewColor;
            }

            Color TempColor = Color.white;
            if (!TempCellColorData.TryGetValue(y, out TempColor))
            {
                return NewColor;
            }

            NewColor = TempColor;

            return NewColor;
        }

        protected SerializedProperty GetRowAt(int idx)
        {
            return cells.GetArrayElementAtIndex(idx).FindPropertyRelative("row");
        }

        private void SetBoldDefaultFont(bool value)
        {
            if (boldFontMethodInfo == null)
                boldFontMethodInfo = typeof(EditorGUIUtility).GetMethod("SetBoldDefaultFont", BindingFlags.Static | BindingFlags.NonPublic);

            boldFontMethodInfo.Invoke(null, new[] { value as object });
        }
    }
}