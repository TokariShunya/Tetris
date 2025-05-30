using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris.Logic;

namespace Tetris.View
{
    /// <summary>
    /// セルの振る舞いを定義するMonoBehaviour
    /// </summary>
    public sealed class CellBehaviour : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _blockRenderer;
        [SerializeField] private SpriteRenderer _axisSymbolRenderer;

        /// <summary>
        /// セルの状態を設定
        /// </summary>
        /// <param name="cell"></param>
        public void Set(Cell cell)
        {
            if (!cell.IsEmpty)
            {
                _blockRenderer.color = cell.Block.Color;

                if (cell.Block.HasAxisSymbol)
                {
                    _axisSymbolRenderer.color = cell.Block.AxisSymbolColor;
                }

                _axisSymbolRenderer.gameObject.SetActive(cell.Block.HasAxisSymbol);
            }
            else {
                _blockRenderer.color = Color.black;
                _axisSymbolRenderer.gameObject.SetActive(false);
            }
        }
    }
}