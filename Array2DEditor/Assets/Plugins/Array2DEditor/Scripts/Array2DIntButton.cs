using UnityEngine;

namespace Array2DEditor
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Array2D_IntButton", menuName = "Array2D/IntButton")]
    public class Array2DIntButton : Array2D<int>
    {
        [SerializeField]
        CellRowInt[] cells = new CellRowInt[Consts.defaultGridSize];

        protected override CellRow<int> GetCellRow(int idx)
        {
            return cells[idx];
        }
    }

//     [System.Serializable]
//     public class CellRowInt : CellRow<int> { }
}