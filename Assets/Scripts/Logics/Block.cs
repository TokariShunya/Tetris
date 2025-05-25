using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Logic
{
    /// <summary>
    /// ブロッククラス
    /// </summary>
    public sealed class Block
    {
        public Color Color { get; }

        public Block() { }

        public Block(Color color)
        {
            Color = color;
        }
    }
}