using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private BlockBehaviour _blockPrefab;
    [SerializeField] private Vector2ReactiveProperty _size;

    private BlockBehaviour[,] _blocks;

    // Start is called before the first frame update
    void Start()
    {
        _size
            .Select(size => new Vector2Int((int)size.x, (int)size.y))
            .Subscribe(size => {
                Initialize(size);
            })
            .AddTo(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 盤面の初期化
    private void Initialize(Vector2Int boardSize)
    {
        if (_blocks != null) {
            foreach (var block in _blocks) {
                Destroy(block.gameObject);
            }
        }

        _blocks = new BlockBehaviour[boardSize.x, boardSize.y];

        for (int i = 0; i < boardSize.x; i++) {
            for (int j = 0; j < boardSize.y; j++) {
                _blocks[i, j] = Instantiate(_blockPrefab, new Vector2(i, j), Quaternion.identity, transform);
                _blocks[i, j].Color.Value = (BlockColor)Random.Range(0, 8);
            }
        }

        Camera.main.transform.position = new Vector3((boardSize.x - 1) * 0.5f, (boardSize.y - 1) * 0.5f, -10);
        Camera.main.orthographicSize = Mathf.Max(boardSize.x * 0.3f, boardSize.y * 0.5f);
    }
}
