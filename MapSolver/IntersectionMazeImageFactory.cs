using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace MapSolver
{
    public class IntersectionMazeImageFactory
    {
        public IntersectionMazeImage CreateIntersectionMaze(string filePath)
        {
            var img = new Bitmap(filePath);
            PixelFormat pixelFormat = img.PixelFormat;
            // Lock the bitmap's bits.  
            var rect = new Rectangle(0, 0, img.Width, img.Height);
            BitmapData bmpData = img.LockBits(rect, ImageLockMode.ReadWrite, pixelFormat);

            var newMaze = new IntersectionMazeImage();
            var hasSeenWallInJ = false;
            var hasSeenWallInI = new bool[img.Width];
            var previousIIntersection = new IntersectionPoint[img.Width];
            var previousJIntersection = new IntersectionPoint();
            var newMazePoints = new List<IntersectionPoint>();
            var hasSetStartConnector = false;
            var lastSeenEndConnector = new IntersectionPoint();
            IntersectionPoint currentPoint;
            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * img.Height;
            var rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);


            for (var index = 0; index < img.Width; index++)
            {
                if (BitmapReader.IsPixel(index, 0, ref rgbValues, pixelFormat, bmpData.Stride))
                {
                    newMaze.StartPoint = new IntersectionPoint()
                    {
                        ICoord = index,
                        JCoord = 0
                    };
                }
                if (BitmapReader.IsPixel(index, img.Height - 1, ref rgbValues, pixelFormat, bmpData.Stride))
                {
                    newMaze.EndPoint = new IntersectionPoint()
                    {
                        ICoord = index,
                        JCoord = img.Height - 1
                    };
                }
                if (newMaze.StartPoint != null && newMaze.EndPoint != null)
                {
                    break;
                }
            }

            if (newMaze.StartPoint == null || newMaze.EndPoint == null)
            {
                throw new Exception("Could not find start and endpoint");
            }
            var i = 1;
            var j = 0;
            var height = img.Height;
            var width = img.Width - 1;
            var hasSquares = true;
            while (hasSquares)
            {
                if (BitmapReader.IsPixel(i, j, ref rgbValues, pixelFormat, bmpData.Stride))
                {
                    if (IsIntersection(i, j, ref rgbValues, pixelFormat, bmpData.Stride, img.Width, img.Height))
                    {
                        currentPoint = new IntersectionPoint()
                        {
                            HasVisited = false,
                            ICoord = i,
                            JCoord = j
                        };
                        if (!hasSetStartConnector && i == newMaze.StartPoint.ICoord)
                        {
                            hasSetStartConnector = true;
                            newMaze.StartPoint.ConnectedIntersections.Add(currentPoint);
                            currentPoint.ConnectedIntersections.Add(newMaze.StartPoint);
                        }
                        if (i == newMaze.EndPoint.ICoord)
                        {
                            lastSeenEndConnector = currentPoint;
                        }
                        if (!hasSeenWallInJ)
                        {
                            currentPoint.ConnectedIntersections.Add(previousJIntersection);
                            previousJIntersection.ConnectedIntersections.Add(currentPoint);
                        }
                        if (!hasSeenWallInI[j] && previousIIntersection[j] != null)
                        {
                            currentPoint.ConnectedIntersections.Add(previousIIntersection[j]);
                            previousIIntersection[j].ConnectedIntersections.Add(currentPoint);
                        }
                        newMazePoints.Add(currentPoint);
                        hasSeenWallInJ = false;
                        hasSeenWallInI[j] = false;
                        previousJIntersection = currentPoint;
                        previousIIntersection[j] = currentPoint;
                    }
                }
                else
                {
                    hasSeenWallInJ = true;
                    hasSeenWallInI[j] = true;
                }
                j++;
                if (j >= height)
                {
                    i++;
                    j = 0;
                }
                if (i >= width)
                {
                    hasSquares = false;
                }
            }
            lastSeenEndConnector.ConnectedIntersections.Add(newMaze.EndPoint);
            newMaze.EndPoint.ConnectedIntersections.Add(lastSeenEndConnector);
            newMaze.Points = newMazePoints.ToArray();
            return newMaze;
        }

        private bool IsIntersection(int i, int j, ref byte[] rgbValues, PixelFormat imgPixelFormat, int bmpDataStride, int width, int height)
        {
            var hasUp = false;
            var hasRight = false;
            var hasDown = false;
            var hasLeft = false;

            if (j - 1 >= 0)
            {
                if (BitmapReader.IsPixel(i, j - 1, ref rgbValues, imgPixelFormat, bmpDataStride))
                {
                    hasUp = true;
                }
            }
            if (i + 1 < width)
            {
                if (BitmapReader.IsPixel(i + 1, j, ref rgbValues, imgPixelFormat, bmpDataStride))
                {
                    hasRight = true;
                }
            }
            if (j + 1 < height)
            {
                if (BitmapReader.IsPixel(i, j + 1, ref rgbValues, imgPixelFormat, bmpDataStride))
                {
                    hasDown = true;
                }
            }
            if (i - 1 >= 0)
            {
                if (BitmapReader.IsPixel(i - 1, j, ref rgbValues, imgPixelFormat, bmpDataStride))
                {
                    hasLeft = true;
                }
            }
            return AtLeastThree(hasUp, hasRight, hasDown, hasLeft) || ChangeInDirection(hasUp, hasRight, hasDown, hasLeft);
        }

        private bool ChangeInDirection(bool hasUp, bool hasRight, bool hasDown, bool hasLeft)
        {
            return (hasUp && (hasLeft || hasRight)) || (hasDown && (hasLeft || hasRight));
        }

        private bool AtLeastThree(bool hasUp, bool hasRight, bool hasDown, bool hasLeft)
        {
            return (hasUp ? 1 : 0) + (hasRight ? 1 : 0) + (hasDown ? 1 : 0) + (hasLeft ? 1 : 0) >= 3;
        }
    }
}
