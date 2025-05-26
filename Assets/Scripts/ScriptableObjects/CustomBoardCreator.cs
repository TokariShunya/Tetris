using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris.Logic;

namespace Tetris.System
{
    [CreateAssetMenu]
    public sealed class CustomBoardCreator : BoardCreator
    {
        /// <summary>
        /// 盤面インスタンスを生成
        /// </summary>
        /// <returns></returns>
        public override Board CreateBoard()
        {
            var initialBoard = new bool[_boardSize.x, _boardSize.y];

            return new Board(initialBoard, TetriminoSequence());
        }

        private IEnumerable<Tetrimino> TetriminoSequence()
        {
            yield return new TetriminoI(_initialPosition);
            yield return new TetriminoO(_initialPosition);
            yield return new TetriminoT(_initialPosition);
        }
    }
}