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

        public static void Rotate(ref this Rotation rotation, Direction direction) {
            rotation = direction switch {
                Direction.Left => (Rotation)(((int)rotation + (rotation.Length() - 1)) % rotation.Length()),
                Direction.Right => (Rotation)(((int)rotation + 1) % rotation.Length()),
                _ => default
            };
        }
    }
}