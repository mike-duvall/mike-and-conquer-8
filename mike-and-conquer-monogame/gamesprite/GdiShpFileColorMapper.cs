namespace mike_and_conquer.gamesprite
{
    public class GdiShpFileColorMapper : ShpFileColorMapper
    {
        public override int MapColorIndex(int index)
        {
            return index;
        }
    }
}
