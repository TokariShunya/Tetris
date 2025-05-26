using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris.Logic;
using VContainer;

namespace Tetris.View
{
    /// <summary>
    /// 盤面描画クラス
    /// </summary>
    public sealed class BoardDrawer
    {
        private readonly CellBehaviour _cellPrefab;
        private readonly Transform _cellParent;

        private CellBehaviour[,] _cellBehaviours;

        [Inject]
        public BoardDrawer(ObjectManager objectManager)
        {
            _cellPrefab = objectManager.CellPrefab;
            _cellParent = objectManager.CellParent;
        }

        /// <summary>
        /// 盤面を描画する
        /// </summary>
        /// <param name="cells"></param>
        public void Draw(Cell[,] cells)
        {
            if (_cellBehaviours is null)
            {
                Create(cells);
            }
            else
            {
                Update(cells);
            }
        }

        private void Create(Cell[,] cells)
        {
            var size = new Vector2Int(cells.GetLength(0), cells.GetLength(1));

            _cellBehaviours = new CellBehaviour[size.x, size.y];

            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    var cell = Object.Instantiate(_cellPrefab, new Vector2(i, j), Quaternion.identity, _cellParent);
                    cell.Set(cells[i, j]);
                    _cellBehaviours[i, j] = cell;
                }
            }

            // 盤面が画面に収まるようにカメラを調整
            Camera.main.transform.position = new Vector3((size.x - 1) * 0.5f, (size.y - 1) * 0.5f, -10);
            Camera.main.orthographicSize = Mathf.Max(size.x * 0.36f, size.y * 0.5f);
        }

        private void Update(Cell[,] cells)
        {
            var size = new Vector2Int(cells.GetLength(0), cells.GetLength(1));

            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    _cellBehaviours[i, j].Set(cells[i, j]);
                }
            }
        }
    }
}