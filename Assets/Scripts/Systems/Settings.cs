using System.Collections;
using System.Collections.Generic;
using Tetris.Logic;
using Tetris.View;
using UnityEngine;

namespace Tetris.System
{
    /// <summary>
    /// 盤面設定
    /// </summary>
    [CreateAssetMenu]
    public class Settings : ScriptableObject
    {
        [SerializeField] private Vector2Int _boardSize;
        [SerializeField] private Vector2Int _initialPosition;

        [SerializeField] private CellBehaviour _cellPrefab;
        public CellBehaviour CellPrefab => _cellPrefab;

        /// <summary>
        /// 盤面インスタンスを生成
        /// </summary>
        /// <returns></returns>
        public Board CreateBoard()
        {
            var initialBoard = new bool[_boardSize.x, _boardSize.y];

            return new Board(initialBoard, RandomTetriminoSequence());
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