using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tetris.Logic;
using System.Linq;

namespace Tetris.View
{
    /// <summary>
    /// UI用テトリミノの振る舞いを定義するMonoBehaviour
    /// </summary>
    public sealed class TetriminoUIBehaviour : MonoBehaviour
    {
        [SerializeField] private Image[] _blockImage;
        [SerializeField] private RectTransform _rectTransform;

        private readonly static Vector2Int _size = new Vector2Int(4, 4);
        private readonly static Vector2Int _origin = new Vector2Int(1, 1);

        public void SetPosition(Vector2 position)
        {
            _rectTransform.anchoredPosition = position;
        }

        /// <summary>
        /// テトリミノを設定
        /// </summary>
        /// <param name="cell"></param>
        public void Set(Tetrimino tetrimino)
        {

            foreach (var image in _blockImage)
            {
                image.color = Color.black;
            }

            if (tetrimino is null) return;

            foreach (var (coordinate, block) in tetrimino.GetInitialBlocks().Select(x => (x.Item1 + _origin, x.Item2)))
            {
                _blockImage[coordinate.x + coordinate.y * _size.x].color = block.Color;
            }

        }
    }
}