namespace Game.Scripts
{
    public static class Utils
    {
        public static bool LayerOnMask(int layer, int mask)
        {
            return (mask | 1 << layer) != 0;
        }
    }
}