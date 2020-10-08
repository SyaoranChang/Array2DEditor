using Boo.Lang;
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

        public Array2DIntButtonROIEditor()
        {
            ProcessForROI();
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
            Vector2Int Center = Vector2Int.zero;
            Center.x = (gridSize.vector2IntValue.x + 1) /2 -1;
            Center.y = (gridSize.vector2IntValue.y + 1) / 2 -1;
            SerializedProperty row = GetRowAt(Center.y);
            row.GetArrayElementAtIndex(Center.x).intValue = 0;
        }
    }
}