using System.Collections;
using System.Collections.Generic;
using Tetris.Logic;
using Tetris.View;
using UnityEngine;
using System.Linq;

namespace Tetris.System
{
    /// <summary>
    /// 盤面生成基底クラス
    /// </summary>
    public abstract class BoardCreator : ScriptableObject
    {
        [Header("盤面サイズ")]
        [SerializeField] protected Vector2Int _boardSize;

        [Header("テトリミノの初期位置")]
        [SerializeField] protected Vector2Int _initialPosition;

        [Header("初期盤面配置")]
        [TextArea(20, 40)]
        [SerializeField] protected string _initialBlocks;

        /// <summary>
        /// 盤面インスタンスを生成
        /// </summary>
        /// <returns></returns>
        public abstract Board CreateBoard();

        protected bool[,] CreateInitialBlocksArray()
        {
            var blocks = new bool[_boardSize.x, _boardSize.y];

            var lines = _initialBlocks.Split('\n')
                .Select(x => x.Trim())
                .Where(x => x != string.Empty)
                .Concat(Enumerable.Repeat("0", _boardSize.y))
                .Take(_boardSize.y)
                .Reverse()
                .ToArray();
            
            for (int j = 0; j < _boardSize.y; j++)
            {
                if (j >= lines.Length) break;

                var s = lines[j].Split(',').Select(x => x.Trim()).Select(x => x == "1").ToArray();

                for (int i = 0; i < _boardSize.x; i++)
                {
                    if (i >= s.Length) break;

                    blocks[i, j] = s[i];
                }
            }

            return blocks;
        }
    }
}