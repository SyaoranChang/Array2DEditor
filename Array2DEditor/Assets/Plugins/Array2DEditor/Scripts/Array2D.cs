/*
 * Arthur Cousseau, 2019
 * https://www.linkedin.com/in/arthurcousseau/
 * Please share this if you enjoy it! :)
*/

using System.Collections.Generic;
using UnityEngine;

namespace Array2DEditor
{
    public abstract class Array2D<T> : ScriptableObject
    {

        [SerializeField]
        protected Vector2Int gridSize = Vector2Int.one * Consts.defaultGridSize;


        public Vector2Int GridSize { get { return gridSize; } }


        protected abstract CellRow<T> GetCellRow(int idx);

        // 回傳2維陣列型態資料
        public T[,] GetCells()
        {
            T[,] ret = new T[gridSize.y, gridSize.x];

            for (int i = 0; i < gridSize.y; i++)
            {
                for (int j = 0; j < gridSize.x; j++)
                {
                    ret[i, j] = GetCellRow(i)[j];
                }
            }

            return ret;
        }

        // 回傳2維List型態資料
        public List<List<T>> GetCellsList()
        {
            var CellsList = new List<List<T>>();

            for (int i = 0; i < gridSize.x; i++)
            {
                List<T> TempList = new List<T>();
                for (int j = 0; j < gridSize.y; j++)
                {
                    TempList.Add(GetCellRow(j)[i]);
                }
                CellsList.Add(TempList);
            }

            return CellsList;
        }
    }

    [System.Serializable]
    public class CellRow<T>
    {
        [SerializeField]
        private T[] row = new T[Consts.defaultGridSize];


        public T this[int i]
        {
            get { return row[i]; }
        }
    }
}