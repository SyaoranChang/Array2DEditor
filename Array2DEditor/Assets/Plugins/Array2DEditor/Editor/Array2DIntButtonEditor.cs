using Boo.Lang;
using UnityEditor;
using UnityEngine;

namespace Array2DEditor
{
    [CustomEditor(typeof(Array2DIntButton))]
    public class Array2DIntButtonEditor : Array2DEditor
    {
        protected override int CellWidth { get { return 32; } }
        protected override int CellHeight { get { return 16; } }

        private Vector2 m_CellWeightSize = new Vector2(60, 16);
        private Vector2 m_CellSetAllWeightSize = new Vector2(150, 16);

        private Rect m_LastRect; // 最後繪製的位置
        private Vector2Int m_GridSize = new Vector2Int(2,5); // 基礎按鈕排列的方式
        private const int m_Margin = 5; // 間距

        private int m_NowSettingWeight = 0; // 紀錄目前要設定的權重

        // 基礎權重的清單
        private List<int> m_WeightList = new List<int>();

        public Array2DIntButtonEditor()
        {
            //m_CellWeightSize = new Vector2(CellWeightWidth, CellWeightHeight);

            // 1~9,0
            for (int i=1;i<=9;++i)
            {
                m_WeightList.Add(i);
            }
            m_WeightList.Add(0);
        }

        protected override void SetValue(SerializedProperty cell, int i, int j)
        {
            int[,] previousCells = (target as Array2DIntButton).GetCells();

            cell.intValue = default(int);

            if (i < gridSize.vector2IntValue.y && j < gridSize.vector2IntValue.x)
            {
                cell.intValue = previousCells[i, j];
            }
        }

        // 當按下按鈕的Callback
        protected override void OnCellBtnClick(SerializedProperty cell, int i, int j)
        {
            //int[,] previousCells = (target as Array2DIntButton).GetCells();

            cell.intValue = default(int);

            if (i < gridSize.vector2IntValue.y && j < gridSize.vector2IntValue.x)
            {
                //cell.intValue = previousCells[i, j];
                cell.intValue = m_NowSettingWeight;
            }
        }

        // 當按下Weight的權重按鈕的Callback
        protected void OnWeightBtnClick(int Weight)
        {
            m_NowSettingWeight = Weight;
        }

        // 當按下Weight的權重按鈕的Callback
        protected void OnSetAllWeightBtnClick(int Weight)
        {
            for (int i = 0; i < gridSize.vector2IntValue.y; i++)
            {
                cells.InsertArrayElementAtIndex(i);
                SerializedProperty row = GetRowAt(i);

                for (int j = 0; j < gridSize.vector2IntValue.x; j++)
                {
                    row.GetArrayElementAtIndex(j).intValue = Weight;
                }
            }
        }

        protected override void OnEndInspectorGUI()
        {
            if (GUILayout.Button("Count sum")) // Just an example, you can remove this.
            {
                EditorUtility.DisplayDialog("Sum", "Sum: " + GetSum(), "OK");
            }

            // 開始繪製權重的按鈕
            GUILayout.Label("Choose Weight");

            GUILayout.Space(0.0f); // 修正下面馬上用GetLastRect()取得資料會有誤的問題

            if (Event.current.type == EventType.Repaint)
            {
                m_LastRect = GUILayoutUtility.GetLastRect();
            }

            Rect cellPosition = m_LastRect;

            cellPosition.y += 10; // Same as EditorGUILayout.Space(), but in Rect

            cellPosition.size = m_CellWeightSize;

            float startLineX = cellPosition.x;

            for (int i = 0; i < m_GridSize.x; i++)
            {
                SerializedProperty row = GetRowAt(i);
                cellPosition.x = startLineX; // Get back to the beginning of the line

                for (int j = 0; j < m_GridSize.y; j++)
                {
                    if (this is Array2DIntButtonEditor)
                    {
                        GUILayout.BeginArea(cellPosition);
                        int TempWeight = m_WeightList[i* m_GridSize.y+ j];
                        if (GUILayout.Button(new GUIContent(TempWeight.ToString())))
                        {
                            OnWeightBtnClick(TempWeight);
                        }
                        GUILayout.EndArea();
                    }
                    cellPosition.x += m_CellWeightSize.x + m_Margin;
                }

                cellPosition.y += m_CellWeightSize.y + m_Margin;
                GUILayout.Space(m_CellWeightSize.y + m_Margin); // If we don't do this, the next things we're going to draw after the grid will be drawn on top of the grid
            }

            GUILayout.Space(10.0f); // 修正下面馬上用GetLastRect()取得資料會有誤的問題

            m_NowSettingWeight = Mathf.RoundToInt(EditorGUILayout.Slider(new GUIContent("Now Setting Weight", "目前設定的權重"), m_NowSettingWeight, 0, 20));

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Set All to 0"), GUILayout.MinWidth(40)))
            {
                OnSetAllWeightBtnClick(0);
            }
            if (GUILayout.Button(new GUIContent("Set All to 1"), GUILayout.MinWidth(40)))
            {
                OnSetAllWeightBtnClick(1);
            }

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Just an example, you can remove this.
        /// </summary>
        private int GetSum()
        {
            int[,] cells = (target as Array2DIntButton).GetCells();

            int count = 0;

            for (int i = 0; i < gridSize.vector2IntValue.y; i++)
            {
                for (int j = 0; j < gridSize.vector2IntValue.x; j++)
                {
                    count += cells[i, j];
                }
            }

            return count;
        }
    }
}