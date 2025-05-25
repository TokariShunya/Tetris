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

        /// <summary>
        /// セルの状態を設定
        /// </summary>
        /// <param name="cell"></param>
        public void Set(Cell cell)
        {
            _blockRenderer.color = cell.IsEmpty ? Color.black : cell.Block.Color;
        }
    }
}