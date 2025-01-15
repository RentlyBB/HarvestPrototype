namespace _Scripts.TileCore.Enums {
    
    // Right now i am not using this in the right way because I create a new enum value for each new tile
    // and it is not right. I just need some main states like DefaultState, Bad Collect, Good Collect, Freeze. 
    public enum TileMainVisualStates {
        DefaultState,
        DefaultState2,
        ReadyToCollect,
        BadCollect,
        GoodCollect,
        Empty,
        FreezeState,
    }

    public enum TileSubVisualStates {
        Unpressed,
        Pressed,
    }
}