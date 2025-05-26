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
    public sealed class BannerPresenter: IInitializable, IDisposable
    {
        private IDisposable _previewDisposable;
        private IDisposable _holdDisposable;

        private readonly Board _board;
        private readonly BannerDrawer _drawer;

        [Inject]
        public BannerPresenter(Board board, BannerDrawer drawer)
        {
            _board = board;
            _drawer = drawer;
        }

        public void Initialize()
        {
            _previewDisposable = _board.OnUpdatePreviews
                .Subscribe(previews =>
                {
                    _drawer.DrawPrevies(previews);
                });
            
            _holdDisposable = _board.OnUpdateHold
                .Subscribe(hold =>
                {
                    _drawer.DrawHold(hold);
                });
        }

        public void Dispose()
        {
            _previewDisposable.Dispose();
            _holdDisposable.Dispose();
        }
        
    }
}