using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Logic
{
    /// <summary>
    /// セルクラス
    /// </summary>
    public sealed class Cell
    {
        public Block Block { get; set; }

        public bool IsEmpty => Block is null;

        public Cell() { }

        public Cell(Block block)
        {
            Block = block;
        }
    }
}