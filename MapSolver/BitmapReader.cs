using System;
using System.Drawing.Imaging;
// ReSharper disable RedundantCaseLabel

namespace MapSolver
{
    public class BitmapReader
    {
        public static bool IsPixel(int i, int j, ref byte[] pixels, PixelFormat pixelFormat, int stride)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format1bppIndexed:
                    return IsPixel1Bpp(i, j, ref pixels, stride);
                case PixelFormat.Format32bppArgb:
                    return IsPixel32BppArgb(i, j, ref pixels, stride);
                case PixelFormat.Alpha:
                case PixelFormat.Canonical:
                case PixelFormat.DontCare:
                case PixelFormat.Extended:
                case PixelFormat.Format16bppArgb1555:
                case PixelFormat.Format16bppGrayScale:
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                case PixelFormat.Format24bppRgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                case PixelFormat.Format48bppRgb:
                case PixelFormat.Format4bppIndexed:
                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                case PixelFormat.Format8bppIndexed:
                case PixelFormat.Gdi:
                case PixelFormat.Indexed:
                case PixelFormat.Max:
                case PixelFormat.PAlpha:
                default:
                    throw new NotImplementedException();
            }
        }

        private static bool IsPixel32BppArgb(int i, int j, ref byte[] pixels, int stride)
        {
            int scanWidth = Math.Abs(stride);
            int index = j * scanWidth + i * 4;
            byte r = pixels[index];
            byte g = pixels[index + 1];
            byte b = pixels[index + 2];
            return r == 255 && g == 255 && b == 255;
        }

        private static bool IsPixel1Bpp(int i, int j, ref byte[] pixels, int stride)
        {
            int scanWidth = Math.Abs(stride);
            int index = j * scanWidth + (int)Math.Floor(i / 8d);
            int bitNumber = (8 - (i % 8));
            return (pixels[index] & (1 << (bitNumber - 1))) != 0;
        }
    }
}
