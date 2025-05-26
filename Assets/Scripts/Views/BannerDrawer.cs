using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Tetris.Logic;
using System.Linq;

namespace Tetris.View
{
    public sealed class BannerDrawer 
    {
        private readonly TetriminoUIBehaviour _tetriminoUIPrefab;
        private readonly RectTransform _previewBanner;
        private readonly RectTransform _holdBanner;

        private TetriminoUIBehaviour[] _previewTetriminos;
        private TetriminoUIBehaviour _holdTetrimino;

        private readonly Vector2 _previewPositionOrigin = new Vector2(0, -120);
        private readonly Vector2 _previewPositionDelta = new Vector2(0, -200);
        private readonly Vector2 _holdPosition = new Vector2(0, -120);

        [Inject]
        public BannerDrawer(ObjectManager objectManager)
        {
            _tetriminoUIPrefab = objectManager.TetriminoUIPrefab;
            _previewBanner = objectManager.PreviewBanner;
            _holdBanner = objectManager.HoldBanner;
        }

        /// <summary>
        /// Nextを描画する
        /// </summary>
        /// <param name="previews"></param>
        public void DrawPrevies(IEnumerable<Tetrimino> previews)
        {
            if (_previewTetriminos is null)
            {
                CreatePrevies(previews);
            }
            else
            {
                UpdatePreview(previews);
            }
        }

        /// <summary>
        /// Holdを描画する
        /// </summary>
        /// <param name="hold"></param>
        public void DrawHold(Tetrimino hold)
        {
            if (_holdTetrimino is null)
            {
                CreateHold(hold);
            }
            else
            {
                UpdateHold(hold);
            }
        }

        private void CreatePrevies(IEnumerable<Tetrimino> previews)
        {
            var previewList = new List<TetriminoUIBehaviour>();

            var position = _previewPositionOrigin;

            foreach (var tetrimino in previews)
            {
                var tetriminoUI = Object.Instantiate(_tetriminoUIPrefab, _previewBanner);
                tetriminoUI.SetPosition(position);
                tetriminoUI.Set(tetrimino);
                previewList.Add(tetriminoUI);

                position += _previewPositionDelta;
            }

            _previewTetriminos = previewList.ToArray();
        }

        private void UpdatePreview(IEnumerable<Tetrimino> previews)
        {

            var enumerator = previews.GetEnumerator();

            for (int i = 0; i < _previewTetriminos.Length; i++)
            {
                if (enumerator.MoveNext())
                {
                    _previewTetriminos[i].Set(enumerator.Current);
                }
                else {
                    _previewTetriminos[i].Set(default);
                }
            }
        }

        private void CreateHold(Tetrimino hold)
        {
            var tetriminoUI = Object.Instantiate(_tetriminoUIPrefab, _holdBanner);
            tetriminoUI.SetPosition(_holdPosition);
            tetriminoUI.Set(hold);
            
            _holdTetrimino = tetriminoUI;
        }

        private void UpdateHold(Tetrimino hold)
        {
            _holdTetrimino.Set(hold);
        }

    }
}