using Boo.Lang;
using UnityEditor;
using UnityEngine;

namespace Array2DEditor
{
    [CustomEditor(typeof(Array2DIntButtonROI))]
    public class Array2DIntButtonROIEditor : Array2DIntButtonEditor
    {
        protected override string m_NowSettingValueLabel { get { return "Now Setting Weight"; } }
        protected override string m_NowSettingValueTip { get { return "目前設定的權重"; } }
        protected override string m_ChooseValueLabel { get { return "Choose Weight"; } }
    }
}