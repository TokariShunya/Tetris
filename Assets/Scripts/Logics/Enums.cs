using System;

namespace Tetris.Logic
{
    public enum Direction
    {
        Left,
        Right
    }

    public enum Rotation
    {
        North,
        East,
        South,
        West
    }

    public static class RotationExtension {
        public static int Length(this Rotation rotation) => Enum.GetValues(typeof(Rotation)).Length;
    }
}