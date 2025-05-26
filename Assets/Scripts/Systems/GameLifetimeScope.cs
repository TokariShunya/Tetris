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
        [Header("盤面セレクタ")]
        [SerializeField] private BoardSelector _boardSelector;

        [Header("オブジェクト管理クラス")]
        [SerializeField] private ObjectManager _objectManager;

        protected override void Configure(IContainerBuilder builder)
        {
            // MVPパターン
            builder.RegisterInstance(_boardSelector.CurrentBoardCreator.CreateBoard());
            builder.RegisterEntryPoint<BoardPresenter>(Lifetime.Singleton);
            builder.RegisterEntryPoint<BannerPresenter>(Lifetime.Singleton);
            builder.Register<BoardDrawer>(Lifetime.Singleton);
            builder.Register<BannerDrawer>(Lifetime.Singleton);

            // 入力イベントハンドラ
            builder.Register<InputHandler>(Lifetime.Singleton);

            // オブジェクト管理クラス
            builder.RegisterInstance(_objectManager);

            // プレイヤー
            builder.RegisterEntryPoint<GamePlayer>(Lifetime.Singleton);
        }
    }
}