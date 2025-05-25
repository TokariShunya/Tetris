using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Tetris.Logic;
using Tetris.View;
using VContainer;
using VContainer.Unity;

namespace Tetris.Presenter
{
    /// <summary>
    /// 盤面のPresenter
    /// </summary>
    public sealed class BoardPresenter : IInitializable, IDisposable
    {
        private IDisposable _disposable;

        private readonly Board _board;
        private readonly BoardDrawer _drawer;

        [Inject]
        public BoardPresenter(Board board, BoardDrawer drawer)
        {
            _board = board;
            _drawer = drawer;
        }

        public void Initialize()
        {
            _disposable = _board.OnUpdateCells
                .Subscribe(cells =>
                {
                    _drawer.Draw(cells);
                });
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}