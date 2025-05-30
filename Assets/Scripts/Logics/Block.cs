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
        public bool HasAxisSymbol { get; }
        public Color AxisSymbolColor { get; }

        public Block() { }

        public Block(Color color)
        {
            Color = color;
        }

        public Block(Color color, Color axisSymbolColor)
        {
            Color = color;
            HasAxisSymbol = true;
            AxisSymbolColor = axisSymbolColor;
        }
    }
}