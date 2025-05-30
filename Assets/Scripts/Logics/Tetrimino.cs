using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tetris.Logic
{
    /// <summary>
    /// テトリミノクラス
    /// </summary>
    public class Tetrimino
    {
        protected readonly Vector2Int[] _initialCoordinates;
        protected readonly Vector2Int _initialPosition;

        protected readonly Vector2Int[] _coordinates;
        protected Vector2Int _position;

        protected readonly Vector2Int[] _appliedCoordinates;
        protected Vector2Int _appliedPosition;

        protected Rotation _rotation;
        protected Rotation _appliedRotation;

        protected int _superRotationIndex;
        protected Vector2Int _superRotationCorrection;

        protected Tetrimino(Vector2Int[] coordinates, Vector2Int initialPosition)
        {
            _initialCoordinates = coordinates;
            _initialPosition = initialPosition;

            _coordinates = coordinates.Clone() as Vector2Int[];
            _appliedCoordinates = coordinates.Clone() as Vector2Int[];

            _position = _initialPosition;
            _appliedPosition = _initialPosition;

            _rotation = Rotation.North;
            _appliedRotation = Rotation.North;

            _superRotationIndex = 0;
            _superRotationCorrection = Vector2Int.zero;
        }

        protected virtual Block Block => default;
        protected virtual Block AxisBlock => default;

        public virtual IEnumerable<(Vector2Int, Block)> GetInitialBlocks(bool displayAxis = false) => _initialCoordinates.Select(x => (x, (x.x == 0 && x.y == 0 && displayAxis) ? AxisBlock : Block));
        public virtual IEnumerable<(Vector2Int, Block)> GetBlocks(bool displayAxis = false) => _coordinates.Select(x => (x + _position + _superRotationCorrection, (x.x == 0 && x.y == 0 && displayAxis) ? AxisBlock : Block));

        protected string GetColoredString(string s) => $"<b><color=#{ColorUtility.ToHtmlStringRGB(Block?.Color ?? Color.white)}>{s}</color></b>";

        public virtual void Rotate(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:

                    for (int i = 0; i < _coordinates.Length; i++)
                    {
                        (_coordinates[i].x, _coordinates[i].y) = (-_coordinates[i].y, _coordinates[i].x);
                    }

                    break;
                case Direction.Right:

                    for (int i = 0; i < _coordinates.Length; i++)
                    {
                        (_coordinates[i].x, _coordinates[i].y) = (_coordinates[i].y, -_coordinates[i].x);
                    }

                    break;
            }

            _rotation.Rotate(direction);
        }

        public virtual bool SuperRotationCorrect(Direction direction)
        {
            switch (_superRotationIndex)
            {
                case 0:

                    (_superRotationCorrection.x, _superRotationCorrection.y) = (_rotation, direction) switch
                    {
                        (Rotation.North, Direction.Left) or (Rotation.South, Direction.Right) => (1, 0),
                        (Rotation.North, Direction.Right) or (Rotation.South, Direction.Left) => (-1, 0),
                        (Rotation.East, _) => (-1, 0),
                        (Rotation.West, _) => (1, 0),
                        _ => (0, 0)
                    };

                    _superRotationIndex++;
                    return true;

                case 1:

                    (_superRotationCorrection.x, _superRotationCorrection.y) = (_rotation, direction) switch
                    {
                        (Rotation.North, Direction.Left) or (Rotation.South, Direction.Right) => (1, -1),
                        (Rotation.North, Direction.Right) or (Rotation.South, Direction.Left) => (-1, -1),
                        (Rotation.East, _) => (-1, 1),
                        (Rotation.West, _) => (1, 1),
                        _ => (0, 0)
                    };

                    _superRotationIndex++;
                    return true;

                case 2:

                    (_superRotationCorrection.x, _superRotationCorrection.y) = (_rotation, direction) switch
                    {
                        (Rotation.North, _) or (Rotation.South, _) => (0, 2),
                        (Rotation.East, _) or (Rotation.West, _) => (0, -2),
                        _ => (0, 0)
                    };

                    _superRotationIndex++;
                    return true;

                case 3:

                    (_superRotationCorrection.x, _superRotationCorrection.y) = (_rotation, direction) switch
                    {
                        (Rotation.North, Direction.Left) or (Rotation.South, Direction.Right) => (1, 2),
                        (Rotation.North, Direction.Right) or (Rotation.South, Direction.Left) => (-1, 2),
                        (Rotation.East, _) => (-1, -2),
                        (Rotation.West, _) => (1, -2),
                        _ => (0, 0)
                    };

                    _superRotationIndex++;
                    return true;

                default:

                    _superRotationCorrection = Vector2Int.zero;
                    return false;
            }
        }

        public void Drop()
        {
            _position.y--;
        }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    _position.x--;
                    break;

                case Direction.Right:
                    _position.x++;
                    break;
            }
        }

        public virtual void Apply()
        {
            for (int i = 0; i < _coordinates.Length; i++)
            {
                _appliedCoordinates[i] = _coordinates[i];
            }

            _position += _superRotationCorrection;

            _appliedPosition = _position;
            _appliedRotation = _rotation;

            _superRotationIndex = 0;
            _superRotationCorrection = Vector2Int.zero;
        }

        public virtual void Revert()
        {
            for (int i = 0; i < _coordinates.Length; i++)
            {
                _coordinates[i] = _appliedCoordinates[i];
            }

            _position = _appliedPosition;
            _rotation = _appliedRotation;

            _superRotationIndex = 0;
            _superRotationCorrection = Vector2Int.zero;
        }

        public virtual void Initialize()
        {
            for (int i = 0; i < _initialCoordinates.Length; i++)
            {
                _coordinates[i] = _initialCoordinates[i];
                _appliedCoordinates[i] = _initialCoordinates[i];
            }

            _position = _initialPosition;
            _appliedPosition = _initialPosition;

            _rotation = Rotation.North;
            _appliedRotation = Rotation.North;

            _superRotationIndex = 0;
            _superRotationCorrection = Vector2Int.zero;
        }
    }

    /// <summary>
    /// Tミノクラス
    /// </summary>
    public sealed class TetriminoT : Tetrimino
    {
        public TetriminoT(Vector2Int initialPosition) : base(new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0) }, initialPosition) { }

        private readonly static Block _block = new Block(new Color32(255, 0, 255, 255));
        private readonly static Block _axisBlock = new Block(new Color32(255, 0, 255, 255), new Color32(128, 0, 128, 255));
        protected override Block Block => _block;
        protected override Block AxisBlock => _axisBlock;

        public override string ToString() => GetColoredString("T");
    }

    /// <summary>
    /// Sミノクラス
    /// </summary>
    public sealed class TetriminoS : Tetrimino
    {
        public TetriminoS(Vector2Int initialPosition) : base(new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(-1, 0), new Vector2Int(0, 0) }, initialPosition) { }

        private readonly static Block _block = new Block(new Color32(0, 221, 0, 255));
        private readonly static Block _axisBlock = new Block(new Color32(0, 221, 0, 255), new Color32(0, 110, 0, 255));
        protected override Block Block => _block;
        protected override Block AxisBlock => _axisBlock;

        public override string ToString() => GetColoredString("S");
    }

    /// <summary>
    /// Zミノクラス
    /// </summary>
    public sealed class TetriminoZ : Tetrimino
    {
        public TetriminoZ(Vector2Int initialPosition) : base(new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(0, 0), new Vector2Int(1, 0) }, initialPosition) { }

        private readonly static Block _block = new Block(new Color32(255, 0, 0, 255));
        private readonly static Block _axisBlock = new Block(new Color32(255, 0, 0, 255), new Color32(128, 0, 0, 255));
        protected override Block Block => _block;
        protected override Block AxisBlock => _axisBlock;

        public override string ToString() => GetColoredString("Z");
    }

    /// <summary>
    /// Lミノクラス
    /// </summary>
    public sealed class TetriminoL : Tetrimino
    {
        public TetriminoL(Vector2Int initialPosition) : base(new Vector2Int[] { new Vector2Int(1, 1), new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0) }, initialPosition) { }

        private readonly static Block _block = new Block(new Color32(255, 140, 0, 255));
        private readonly static Block _axisBlock = new Block(new Color32(255, 140, 0, 255), new Color32(128, 70, 0, 255));
        protected override Block Block => _block;
        protected override Block AxisBlock => _axisBlock;

        public override string ToString() => GetColoredString("L");
    }

    /// <summary>
    /// Jミノクラス
    /// </summary>
    public sealed class TetriminoJ : Tetrimino
    {
        public TetriminoJ(Vector2Int initialPosition) : base(new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0) }, initialPosition) { }

        private readonly static Block _block = new Block(new Color32(30, 128, 255, 255));
        private readonly static Block _axisBlock = new Block(new Color32(30, 128, 255, 255), new Color32(15, 64, 128, 255));
        protected override Block Block => _block;
        protected override Block AxisBlock => _axisBlock;

        public override string ToString() => GetColoredString("J");
    }

    /// <summary>
    /// Iミノクラス
    /// </summary>
    public sealed class TetriminoI : Tetrimino
    {
        private readonly Vector2Int[] _initialCenterOriginCoordinates;

        private readonly Vector2Int[] _centerOriginCoordinates;
        private readonly Vector2Int[] _appliedCenterOriginCoordinates;

        public TetriminoI(Vector2Int initialPosition) : base(new Vector2Int[] { new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0) }, initialPosition)
        {
            _initialCenterOriginCoordinates = new Vector2Int[] { new Vector2Int(-3, 1), new Vector2Int(-1, 1), new Vector2Int(1, 1), new Vector2Int(3, 1) };
            
            _centerOriginCoordinates = _initialCenterOriginCoordinates.Clone() as Vector2Int[];
            _appliedCenterOriginCoordinates = _initialCenterOriginCoordinates.Clone() as Vector2Int[];
        }

        private readonly static Block _block = new Block(new Color32(0, 255, 255, 255));
        protected override Block Block => _block;
        protected override Block AxisBlock => _block;

        public override void Rotate(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:

                    for (int i = 0; i < _centerOriginCoordinates.Length; i++)
                    {
                        (_centerOriginCoordinates[i].x, _centerOriginCoordinates[i].y) = (-_centerOriginCoordinates[i].y, _centerOriginCoordinates[i].x);
                    }

                    break;
                case Direction.Right:

                    for (int i = 0; i < _centerOriginCoordinates.Length; i++)
                    {
                        (_centerOriginCoordinates[i].x, _centerOriginCoordinates[i].y) = (_centerOriginCoordinates[i].y, -_centerOriginCoordinates[i].x);
                    }

                    break;
            }

            _rotation.Rotate(direction);

            for (int i = 0; i < _coordinates.Length; i++)
            {
                (_coordinates[i].x, _coordinates[i].y) = ((_centerOriginCoordinates[i].x + 1) / 2, (_centerOriginCoordinates[i].y - 1) / 2);
            }
        }

        public override bool SuperRotationCorrect(Direction direction)
        {
            switch (_superRotationIndex)
            {
                case 0:

                    (_superRotationCorrection.x, _superRotationCorrection.y) = (_rotation, direction) switch
                    {
                        (Rotation.North, Direction.Left) or (Rotation.West, Direction.Right) => (2, 0),
                        (Rotation.East, Direction.Left) or (Rotation.North, Direction.Right) => (1, 0),
                        (Rotation.South, Direction.Left) or (Rotation.East, Direction.Right) => (-2, 0),
                        (Rotation.West, Direction.Left) or (Rotation.South, Direction.Right) => (-1, 0),
                        _ => (0, 0)
                    };

                    _superRotationIndex++;
                    return true;

                case 1:

                    (_superRotationCorrection.x, _superRotationCorrection.y) = (_rotation, direction) switch
                    {
                        (Rotation.North, Direction.Left) or (Rotation.West, Direction.Right) => (-1, 0),
                        (Rotation.East, Direction.Left) or (Rotation.North, Direction.Right) => (-2, 0),
                        (Rotation.South, Direction.Left) or (Rotation.East, Direction.Right) => (1, 0),
                        (Rotation.West, Direction.Left) or (Rotation.South, Direction.Right) => (2, 0),
                        _ => (0, 0)
                    };

                    _superRotationIndex++;
                    return true;

                case 2:

                    (_superRotationCorrection.x, _superRotationCorrection.y) = (_rotation, direction) switch
                    {
                        (Rotation.North, Direction.Left) or (Rotation.West, Direction.Right) => (2, 1),
                        (Rotation.East, Direction.Left) or (Rotation.North, Direction.Right) => (1, -2),
                        (Rotation.South, Direction.Left) or (Rotation.East, Direction.Right) => (-2, -1),
                        (Rotation.West, Direction.Left) or (Rotation.South, Direction.Right) => (-1, 2),
                        _ => (0, 0)
                    };

                    _superRotationIndex++;
                    return true;

                case 3:

                    (_superRotationCorrection.x, _superRotationCorrection.y) = (_rotation, direction) switch
                    {
                        (Rotation.North, Direction.Left) or (Rotation.West, Direction.Right) => (-1, -2),
                        (Rotation.East, Direction.Left) or (Rotation.North, Direction.Right) => (-2, 1),
                        (Rotation.South, Direction.Left) or (Rotation.East, Direction.Right) => (1, 2),
                        (Rotation.West, Direction.Left) or (Rotation.South, Direction.Right) => (2, -1),
                        _ => (0, 0)
                    };

                    _superRotationIndex++;
                    return true;

                default:

                    _superRotationCorrection = Vector2Int.zero;
                    return false;
            }
        }

        public override void Apply()
        {
            for (int i = 0; i < _centerOriginCoordinates.Length; i++)
            {
                _appliedCenterOriginCoordinates[i] = _centerOriginCoordinates[i];
            }

            base.Apply();
        }

        public override void Revert()
        {
            for (int i = 0; i < _centerOriginCoordinates.Length; i++)
            {
                _centerOriginCoordinates[i] = _appliedCenterOriginCoordinates[i];
            }

            base.Revert();
        }

        public override void Initialize()
        {
            for (int i = 0; i < _initialCenterOriginCoordinates.Length; i++)
            {
                _centerOriginCoordinates[i] = _initialCenterOriginCoordinates[i];
                _appliedCenterOriginCoordinates[i] = _initialCenterOriginCoordinates[i];
            }

            base.Initialize();
        }

        public override string ToString() => GetColoredString("I");
    }

    /// <summary>
    /// Oミノクラス
    /// </summary>
    public sealed class TetriminoO : Tetrimino
    {
        public TetriminoO(Vector2Int initialPosition) : base(new Vector2Int[] { new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(0, 0), new Vector2Int(0, 1) }, initialPosition) { }

        private readonly static Block _block = new Block(new Color32(255, 255, 0, 255));
        protected override Block Block => _block;
        protected override Block AxisBlock => _block;

        public override void Rotate(Direction direction) { }

        public override string ToString() => GetColoredString("O");
    }
}