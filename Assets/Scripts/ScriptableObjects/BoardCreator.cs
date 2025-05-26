using System.Collections;
using System.Collections.Generic;
using Tetris.Logic;
using Tetris.View;
using UnityEngine;

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

        /// <summary>
        /// 盤面インスタンスを生成
        /// </summary>
        /// <returns></returns>
        public abstract Board CreateBoard();
    }
}