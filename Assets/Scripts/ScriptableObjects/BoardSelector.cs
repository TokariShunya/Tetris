using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.System
{
    /// <summary>
    /// 盤面セレクタ
    /// </summary>
    [CreateAssetMenu]
    public sealed class BoardSelector : ScriptableObject
    {
        [Header("盤面生成クラス")]
        [SerializeField] private BoardCreator[] _boardCreator;

        [Header("選択する盤面の番号")]
        [SerializeField] private int _selection;

        public BoardCreator CurrentBoardCreator {
            get {
                if (_selection >= 0 && _selection < _boardCreator.Length) {
                    return _boardCreator[_selection];
                }

                return _boardCreator[0];
            }
        }
    }
}

