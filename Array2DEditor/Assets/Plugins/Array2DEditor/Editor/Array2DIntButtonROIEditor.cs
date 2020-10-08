using Boo.Lang;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Array2DEditor
{
    [CustomEditor(typeof(Array2DIntButtonROI))]
    public class Array2DIntButtonROIEditor : Array2DIntButtonEditor
    {
        protected override int m_NowSettingValueMin { get { return 0; } }
        protected override int m_NowSettingValueMax { get { return 20; } }
        protected override string m_NowSettingValueLabel { get { return "Now Setting Weight"; } }
        protected override string m_NowSettingValueTip { get { return "目前設定的權重"; } }
        protected override string m_ChooseValueLabel { get { return "Choose Weight"; } }

        Vector2Int m_CenterPos = Vector2Int.zero;

        public Array2DIntButtonROIEditor()
        {

        }

        public override void OnEnable()
        {
            base.OnEnable();

            if (null == gridSize)
            {
                Debug.LogWarning("gridSize is null !");
                return;
            }

            m_CenterPos.x = (gridSize.vector2IntValue.x + 1) / 2 - 1;
            m_CenterPos.y = (gridSize.vector2IntValue.y + 1) / 2 - 1;

            // 設定ROI中心點的顏色
            Dictionary<int, Color> TempCellColorData = new Dictionary<int, Color>();
            Color NewColor = Color.Lerp(Color.white, Color.green, 0.7f);
            TempCellColorData.Add(m_CenterPos.y, NewColor);
            m_SpecialCellColorData.Add(m_CenterPos.x, TempCellColorData);
        }

        protected override void DisplayGrid(Rect startRect)
        {
            base.DisplayGrid(startRect);
        }

        protected override void OnEndInspectorGUI()
        {
            base.OnEndInspectorGUI();

            ProcessForROI();
        }

        // ROI的特別處理
        protected virtual void ProcessForROI()
        {
            if (null == gridSize) return;

            SerializedProperty row = GetRowAt(m_CenterPos.y);
            row.GetArrayElementAtIndex(m_CenterPos.x).intValue = 0;
        }
    }
}