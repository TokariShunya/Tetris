using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.InputSystem;
using Tetris.Logic;

namespace Tetris.Input
{
    public sealed class InputHandler : IDisposable
    {
        private readonly Input _input;

        private readonly IObservable<Unit> _onDrop;
        public IObservable<Unit> OnDrop => _onDrop;

        private readonly IObservable<Direction> _onMove;
        public IObservable<Direction> OnMove => _onMove;

        private readonly IObservable<Direction> _onRotate;
        public IObservable<Direction> OnRotate => _onRotate;

        private readonly IObservable<Unit> _onHardDrop;
        public IObservable<Unit> OnHardDrop => _onHardDrop;

        private readonly IObservable<Unit> _onHold;
        public IObservable<Unit> OnHold => _onHold;

        private readonly IObservable<Unit> _onReload;
        public IObservable<Unit> OnReload => _onReload;

        public InputHandler()
        {
            _input = new Input();

            _onDrop = _input.Tetris.Drop.AsObservable()
                .Select(_ => Unit.Default);

            _onMove = _input.Tetris.Move.AsObservable()
                .Select(context => context.ReadValue<float>())
                .Select(value => value > 0 ? Direction.Right : Direction.Left);

            _onRotate = _input.Tetris.Rotate.AsObservable()
                .Select(context => context.ReadValue<float>())
                .Select(value => value > 0 ? Direction.Right : Direction.Left);

            _onHardDrop = _input.Tetris.HardDrop.AsObservable()
                .Select(_ => Unit.Default);

            _onHold = _input.Tetris.Hold.AsObservable()
                .Select(_ => Unit.Default);

            _onReload = _input.Tetris.Reload.AsObservable()
                .Select(_ => Unit.Default);

            _input.Enable();
        }

        public void Dispose()
        {
            _input.Disable();
            _input.Dispose();
        }
    }

    public static class InputSystemExtensions
    {
        public static IObservable<InputAction.CallbackContext> AsObservable(this InputAction action)
        {
            return Observable.FromEvent<InputAction.CallbackContext>(
                h => action.performed += h,
                h => action.performed -= h);
        }
    }
}