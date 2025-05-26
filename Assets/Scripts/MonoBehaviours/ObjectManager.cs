using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.View
{
    public class ObjectManager : MonoBehaviour
    {
        [SerializeField] private CellBehaviour _cellPrefab;
        public CellBehaviour CellPrefab => _cellPrefab;

        [SerializeField] private TetriminoUIBehaviour _tetriminoUIPrefab;
        public TetriminoUIBehaviour TetriminoUIPrefab => _tetriminoUIPrefab;

        [SerializeField] private Transform _cellParent;
        public Transform CellParent => _cellParent;

        [SerializeField] private RectTransform _previewBanner;
        public RectTransform PreviewBanner => _previewBanner;

        [SerializeField] private RectTransform _holdBanner;
        public RectTransform HoldBanner => _holdBanner;
    }
}

