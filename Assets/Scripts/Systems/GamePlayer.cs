using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Tetris.Logic;
using Tetris.Presenter;
using Tetris.View;
using Tetris.Input;
using UniRx;

namespace Tetris.System
{
    /// <summary>
    /// プレイヤー
    /// </summary>
    public sealed class GamePlayer : IInitializable, IDisposable
    {
        private readonly Board _board;
        private readonly InputHandler _inputHandler;

        private readonly List<IDisposable> _disposables;

        [Inject]
        public GamePlayer(Board board, InputHandler inputHandler)
        {
            _board = board;
            _inputHandler = inputHandler;
            _disposables = new List<IDisposable>();
        }

        public void Initialize()
        {
            // 落下操作
            _disposables.Add(_inputHandler.OnDrop.Subscribe(_ => _board.Drop()));

            // 移動操作
            _disposables.Add(_inputHandler.OnMove.Subscribe(direction => _board.Move(direction)));

            // 回転操作
            _disposables.Add(_inputHandler.OnRotate.Subscribe(direction => _board.Rotate(direction)));

            // ハードドロップ操作
            _disposables.Add(_inputHandler.OnHardDrop.Subscribe(_ => _board.HardDrop()));

            // ホールド操作
            _disposables.Add(_inputHandler.OnHold.Subscribe(_ => _board.Hold()));

            // シーン再読み込み
            _disposables.Add(_inputHandler.OnReload.Subscribe(_ => TransitionManager.ReloadScene()));
        }

        public void Dispose()
        {
            _board.Dispose();

            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}