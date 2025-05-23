using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public class BlockBehaviour : MonoBehaviour
{
    private ReactiveProperty<BlockColor> _color = new ReactiveProperty<BlockColor>(BlockColor.Empty);
    public ReactiveProperty<BlockColor> Color => _color;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    [System.Serializable]
    private struct ColorPair {
        public BlockColor ColorName;
        public Color Color;
    }

    [SerializeField] private ColorPair[] _colors;
    private Dictionary<BlockColor, Color> _colorDictionary;

    private void Awake()
    {
        _colorDictionary = _colors.ToDictionary(x => x.ColorName, x => x.Color);
    }

    // Start is called before the first frame update
    void Start()
    {
        Color
            .Subscribe(color => {
                _spriteRenderer.color = _colorDictionary[color];
            })
            .AddTo(gameObject);
    }
}
