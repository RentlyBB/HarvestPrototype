using System;
using _Scripts.TileCore.Enums;

namespace _Scripts.LevelEditor {
    
    public static class TileVisualParser {

        public static TileMainVisualStates TileTypeToTileMainVisualState(TileType tileType) {
            switch (tileType) {
                case TileType.EmptyTile:
                    return TileMainVisualStates.Empty;
                case TileType.DefaultTile:
                    return TileMainVisualStates.Default;
                case TileType.CountdownTile:
                    return TileMainVisualStates.Default;
                case TileType.RepeatCountdownTile:
                    return TileMainVisualStates.RepeatCountdown;
                case TileType.FreezeTile:
                    return TileMainVisualStates.Default;
                default:
                    return TileMainVisualStates.Empty;
            }
        }
    }
}