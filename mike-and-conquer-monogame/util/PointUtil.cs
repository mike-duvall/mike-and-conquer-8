

using Microsoft.Xna.Framework;

namespace mike_and_conquer.util
{
    public class PointUtil
    {

        public static Point ConvertVector2ToPoint(Vector2 aVector2)
        {
            return new Point((int) aVector2.X, (int) aVector2.Y);
        }

        public static Vector2 ConvertPointToVector2(Point aPoint)
        {
            return new Vector2(aPoint.X, aPoint.Y);
        }


    }
}
