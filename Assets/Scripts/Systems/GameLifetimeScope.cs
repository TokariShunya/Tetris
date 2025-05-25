using VContainer;
using VContainer.Unity;
using Tetris.Presenter;
using Tetris.View;
using Tetris.Input;
using UnityEngine;

namespace Tetris.System
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("設定")]
        [SerializeField] private Settings _settings;

        protected override void Configure(IContainerBuilder builder)
        {
            // MVPパターン
            builder.RegisterInstance(_settings.CreateBoard());
            builder.RegisterEntryPoint<BoardPresenter>(Lifetime.Singleton);
            builder.Register<BoardDrawer>(Lifetime.Singleton);

            // 入力イベントハンドラ
            builder.Register<InputHandler>(Lifetime.Singleton);

            // セルのプレハブ
            builder.RegisterComponent(_settings.CellPrefab);

            var transform = new GameObject("Cells").transform;
            builder.RegisterComponent(transform);

            // プレイヤー
            builder.RegisterEntryPoint<GamePlayer>(Lifetime.Singleton);
        }
    }
}