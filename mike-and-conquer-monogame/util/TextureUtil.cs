
using System.Collections.Generic;
using mike_and_conquer.main;
using mike_and_conquer_monogame.main;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Color = Microsoft.Xna.Framework.Color;

namespace mike_and_conquer.util
{
    public class TextureUtil
    {
        public static Texture2D CopyTexture(Texture2D sourceTexture)
        {
            Color[] sourceTexturePixelData = new Color[sourceTexture.Width * sourceTexture.Height];
            sourceTexture.GetData(sourceTexturePixelData);

            Texture2D copyTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, sourceTexture.Width,
                sourceTexture.Height);

            Color[] copyTexturePixelData = new Color[copyTexture.Width * copyTexture.Height];
            copyTexture.GetData(copyTexturePixelData);

            int index = 0;
            foreach (Color color in sourceTexturePixelData)
            {
                copyTexturePixelData[index] = color;
                index++;
            }

            copyTexture.SetData(copyTexturePixelData);

            return copyTexture;
        }

        public static Texture2D UpdateShadowPixelsToTransparent(Texture2D texture, List<int> shadowIndexList)
        {
            Color[] texturePixelData = new Color[texture.Width * texture.Height];
            texture.GetData(texturePixelData);

            foreach (int shadowIndex in shadowIndexList)
            {
                texturePixelData[shadowIndex] = Color.Transparent;
            }

            texture.SetData(texturePixelData);

            return texture;
        }


        public static Texture2D CreateSpriteBorderRectangleTexture(Color color, int width, int height)
        {
            Texture2D rectangle = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, width, height);
            Color[] data = new Color[rectangle.Width * rectangle.Height];
            FillHorizontalLine(data, rectangle.Width, rectangle.Height, 0, color);
            FillHorizontalLine(data, rectangle.Width, rectangle.Height, rectangle.Height - 1, color);
            FillVerticalLine(data, rectangle.Width, rectangle.Height, 0, color);
            FillVerticalLine(data, rectangle.Width, rectangle.Height, rectangle.Width - 1, color);

            int centerX = (rectangle.Width / 2);
            int centerY = (rectangle.Height / 2);

            // Check how this works for even sized sprites with true center

            int centerOffset = (centerY * rectangle.Width) + centerX;

            data[centerOffset] = Color.Red;

            rectangle.SetData(data);
            return rectangle;
        }

        private static void FillHorizontalLine(Color[] data, int width, int height, int lineIndex, Color color)
        {
            int beginIndex = width * lineIndex;
            for (int i = beginIndex; i < (beginIndex + width); ++i)
            {
                data[i] = color;
            }
        }

        private static void FillVerticalLine(Color[] data, int width, int height, int lineIndex, Color color)
        {
            int beginIndex = lineIndex;
            for (int i = beginIndex; i < (width * height); i += width)
            {
                data[i] = color;
            }
        }




    }
}
