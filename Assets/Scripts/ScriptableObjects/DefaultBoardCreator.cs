using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris.Logic;

namespace Tetris.System
{
    [CreateAssetMenu]
    public sealed class DefaultBoardCreator : BoardCreator
    {
        /// <summary>
        /// 盤面インスタンスを生成
        /// </summary>
        /// <returns></returns>
        public override Board CreateBoard()
        {
            return new Board(CreateInitialBlocksArray(), RandomTetriminoSequence());
        }

        private IEnumerable<Tetrimino> RandomTetriminoSequence()
        {
            while (true)
            {
                var tetriminos = new Tetrimino[]
                {
                    new TetriminoT(_initialPosition),
                    new TetriminoS(_initialPosition),
                    new TetriminoZ(_initialPosition),
                    new TetriminoL(_initialPosition),
                    new TetriminoJ(_initialPosition),
                    new TetriminoI(_initialPosition),
                    new TetriminoO(_initialPosition)
                };

                for (var i = tetriminos.Length - 1; i > 0; i--)
                {
                    var j = Random.Range(0, i + 1);

                    (tetriminos[i], tetriminos[j]) = (tetriminos[j], tetriminos[i]);
                }

                for (int i = 0; i < tetriminos.Length; i++)
                {
                    yield return tetriminos[i];
                }
            }
        }
    }
}