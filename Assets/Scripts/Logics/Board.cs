using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using VContainer;
using System.Linq;

namespace Tetris.Logic
{
    /// <summary>
    /// 盤面クラス
    /// </summary>
    public sealed class Board : IDisposable
    {
        private readonly Vector2Int _size;

        private readonly Cell[,] _cells;
        private readonly IEnumerator<Tetrimino> _tetriminos;
        private readonly Queue<Tetrimino> _tetriminoPreviews;

        private Tetrimino _currentTetrimino;
        private Tetrimino _holdTetrimino;

        private bool _canHoldTetrimino;

        private readonly Cell[,] _drawingCells;

        private readonly static int _space = 5;
        private readonly static int _tetriminoPreviewCount = 5;
        private readonly static Cell _filledCell = new Cell(new Block());

        private readonly ReplaySubject<Cell[,]> _onUpdateCells;
        public IObservable<Cell[,]> OnUpdateCells => _onUpdateCells.AsObservable();

        private readonly ReplaySubject<IEnumerable<Tetrimino>> _onUpdatePreviews;
        public IObservable<IEnumerable<Tetrimino>> OnUpdatePreviews => _onUpdatePreviews.AsObservable();

        private readonly ReplaySubject<Tetrimino> _onUpdateHold;
        public IObservable<Tetrimino> OnUpdateHold => _onUpdateHold.AsObservable();

        [Inject]
        public Board(bool[,] initialBoard, IEnumerable<Tetrimino> tetriminos)
        {
            Debug.Log("New Game");

            // 盤面サイズ
            _size = new Vector2Int(initialBoard.GetLength(0), initialBoard.GetLength(1));

            // セルを生成
            _cells = new Cell[_size.x, _size.y + _space];

            // 空の盤面を生成
            for (int i = 0; i < _size.x; i++)
            {
                for (int j = 0; j < _size.y + _space; j++)
                {
                    _cells[i, j] = new Cell();
                }
            }

            // 初期盤面のブロック
            var initialBlock = new Block(Color.white);

            // 初期盤面を反映
            for (int i = 0; i < _size.x; i++)
            {
                for (int j = 0; j < _size.y; j++)
                {
                    if (initialBoard[i, j])
                    {
                        GetCell(i, j).Block = initialBlock;
                    } 
                }
            }

            // テトリミノの列を生成
            _tetriminos = tetriminos.GetEnumerator();

            _holdTetrimino = null;
            _canHoldTetrimino = true;

            _onUpdateHold = new ReplaySubject<Tetrimino>();
            _onUpdateHold.OnNext(default);

            _tetriminoPreviews = new Queue<Tetrimino>();

            // ネクストにテトリミノをセット
            for (int i = 0; i < _tetriminoPreviewCount; i++)
            {
                if (_tetriminos.MoveNext())
                {
                    _tetriminoPreviews.Enqueue(_tetriminos.Current);
                }
            }

            // 現在のテトリミノをセット
            _onUpdatePreviews = new ReplaySubject<IEnumerable<Tetrimino>>();

            Next();

            // 盤面描画用
            _drawingCells = new Cell[_size.x, _size.y];

            for (int i = 0; i < _size.x; i++)
            {
                for (int j = 0; j < _size.y; j++)
                {
                    _drawingCells[i, j] = new Cell();
                }
            }

            _onUpdateCells = new ReplaySubject<Cell[,]>();

            Update();
        }

        private Cell GetCell(int x, int y)
        {
            if (x >= 0 && x < _size.x && y >= 0 && y < _size.y + _space) return _cells[x, y];

            return _filledCell;
        }

        private Cell GetDrawingCell(int x, int y)
        {
            if (x >= 0 && x < _size.x && y >= 0 && y < _size.y) return _drawingCells[x, y];

            return default;
        }

        private void Update()
        {
            // 現在のセルを描画用セルに反映
            for (int i = 0; i < _size.x; i++)
            {
                for (int j = 0; j < _size.y; j++)
                {
                    GetDrawingCell(i, j).Block = GetCell(i, j).Block;
                }
            }

            // 現在のテトリミノを描画用セルに反映
            if (_currentTetrimino is not null)
            {
                foreach (var (position, block) in _currentTetrimino.GetBlocks(true))
                {
                    if (GetDrawingCell(position.x, position.y) is {} drawingCell)
                    {
                        drawingCell.Block = block;
                    }
                }
            }

            // 描画セル変更イベントを発行
            _onUpdateCells.OnNext(_drawingCells);
        }

        private void Next()
        {
            if (_tetriminoPreviews.TryDequeue(out var next))
            {
                _currentTetrimino = next;

                if (_tetriminos.MoveNext())
                {
                    _tetriminoPreviews.Enqueue(_tetriminos.Current);
                }
                
                if (_tetriminoPreviews.Count > 0)
                {
                    Debug.Log($"Next: {_tetriminoPreviews.Select(x => x.ToString()).Aggregate((result, current) => result + current)}");
                }

                _onUpdatePreviews.OnNext(_tetriminoPreviews.AsEnumerable());
            }
            else
            {
                _currentTetrimino = null;
            }
        }

        private void ClearLines()
        {
            var clearedLines = new HashSet<int>();

            for (int j = 0; j < _size.y + _space; j++)
            {
                bool isCleared = true;

                for (int i = 0; i < _size.x; i++)
                {
                    if (GetCell(i, j).IsEmpty)
                    {
                        isCleared = false;
                        break;
                    }
                }

                if (isCleared)
                {
                    clearedLines.Add(j);
                }
            }

            var drop = 0;

            for (int j = 0; j < _size.y + _space; j++)
            {
                if (clearedLines.Contains(j))
                {
                    drop++;
                    continue;
                }

                for (int i = 0; i < _size.x; i++)
                {
                    GetCell(i, j - drop).Block = GetCell(i, j).Block;
                }
            }
        }

        private void Apply()
        {
            // 現在のテトリミノをセルに反映
            foreach (var (position, block) in _currentTetrimino.GetBlocks())
            {
                GetCell(position.x, position.y).Block = block;
            }
        }

        /// <summary>
        /// 現在のテトリミノを1マス落下させる
        /// </summary>
        public void Drop()
        {
            if (_currentTetrimino is null) return;

            _currentTetrimino.Drop();

            var positions = _currentTetrimino.GetBlocks().Select(x => x.Item1);

            foreach (var position in positions)
            {
                if (!GetCell(position.x, position.y).IsEmpty)
                {
                    _currentTetrimino.Revert();

                    // Apply();
                    // ClearLines();
                    // Next();

                    // _canHoldTetrimino = true;

                    // Update();
                    return;
                }
            }

            _currentTetrimino.Apply();

            Update();
        }

        /// <summary>
        /// 現在のテトリミノを接地するまで落下させる
        /// </summary>
        public void HardDrop()
        {
            if (_currentTetrimino is null) return;

            while (true)
            {
                _currentTetrimino.Drop();

                var positions = _currentTetrimino.GetBlocks().Select(x => x.Item1);

                foreach (var position in positions)
                {
                    if (!GetCell(position.x, position.y).IsEmpty)
                    {
                        _currentTetrimino.Revert();

                        Apply();
                        ClearLines();
                        Next();

                        _canHoldTetrimino = true;

                        Update();
                        return;
                    }
                }

                _currentTetrimino.Apply();
            }
        }

        /// <summary>
        /// 現在のテトリミノを1マス移動させる
        /// </summary>
        /// <param name="direction"></param>
        public void Move(Direction direction)
        {
            if (_currentTetrimino is null) return;

            _currentTetrimino.Move(direction);

            var positions = _currentTetrimino.GetBlocks().Select(x => x.Item1);

            foreach (var position in positions)
            {
                if (!GetCell(position.x, position.y).IsEmpty)
                {
                    _currentTetrimino.Revert();
                    return;
                }
            }

            _currentTetrimino.Apply();

            Update();
        }

        /// <summary>
        /// 現在のテトリミノを回転させる
        /// </summary>
        /// <param name="direction"></param>
        public void Rotate(Direction direction)
        {
            if (_currentTetrimino is null) return;

            var canRotate = true;

            _currentTetrimino.Rotate(direction);

            var positions = _currentTetrimino.GetBlocks().Select(x => x.Item1);

            foreach (var position in positions)
            {
                if (!GetCell(position.x, position.y).IsEmpty)
                {
                    canRotate = false;
                    break;
                }
            }

            if (canRotate)
            {
                _currentTetrimino.Apply();
                Update();
                return;
            }

            // SRS
            while (_currentTetrimino.SuperRotationCorrect(direction))
            {
                canRotate = true;

                positions = _currentTetrimino.GetBlocks().Select(x => x.Item1);

                foreach (var position in positions)
                {
                    if (!GetCell(position.x, position.y).IsEmpty)
                    {
                        canRotate = false;
                        break;
                    }
                }

                if (canRotate)
                {
                    _currentTetrimino.Apply();
                    Update();
                    return;
                }
            }

            _currentTetrimino.Revert();
        }

        /// <summary>
        /// 現在のテトリミノをホールドする
        /// </summary>
        public void Hold()
        {
            if (!_canHoldTetrimino) return;
            if (_currentTetrimino is null && _holdTetrimino is null) return;

            if (_holdTetrimino is null)
            {
                _holdTetrimino = _currentTetrimino;
                _holdTetrimino.Initialize();

                _onUpdateHold.OnNext(_holdTetrimino);
                Debug.Log($"Hold: {_holdTetrimino}");

                Next();
            }
            else
            {
                (_currentTetrimino, _holdTetrimino) = (_holdTetrimino, _currentTetrimino);
                _holdTetrimino?.Initialize();

                _onUpdateHold.OnNext(_holdTetrimino);
                Debug.Log($"Hold: {_holdTetrimino}");
            }

            _canHoldTetrimino = false;

            Update();
        }

        public void Dispose()
        {
            _onUpdateCells.OnCompleted();
            _onUpdatePreviews.OnCompleted();
            _onUpdateHold.OnCompleted();
            _onUpdateCells.Dispose();
            _onUpdatePreviews.Dispose();
            _onUpdateHold.Dispose();
        }
    }
}