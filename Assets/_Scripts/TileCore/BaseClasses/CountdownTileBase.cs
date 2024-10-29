namespace _Scripts.TileCore.BaseClasses {
    public abstract class CountdownTileBase : TileBase {

        public int countdownValue;

        protected abstract void ResolveCountdown();

    }
}