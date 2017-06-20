using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace MapSolver
{
    public class IntersectionMazeImageFactory
    {
        public IntersectionMazeImage CreateIntersectionMaze(string filePath)
        {
            var s = new Stopwatch();
            s.Start();
            var img = new Bitmap(filePath);
            // Lock the bitmap's bits.  
            var rect = new Rectangle(0, 0, img.Width, img.Height);
            BitmapData bmpData = img.LockBits(rect, ImageLockMode.ReadWrite, img.PixelFormat);

            var newMaze = new IntersectionMazeImage(img.Width, img.Height);
            var hasSeenWallInJ = false;
            var hasSeenWallInI = new bool[img.Width];
            var previousIIntersection = new IntersectionPoint[img.Width];
            var previousJIntersection = new IntersectionPoint();
            var newMazePoints = new List<IntersectionPoint>();
            var hasSetStartConnector = false;
            var lastSeenEndConnector = new IntersectionPoint();
            

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * img.Height;
            var rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            if (img.PixelFormat != PixelFormat.Format1bppIndexed && img.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new SystemException("Image format not supported, supports: Format1bppIndexed");
            }

            for (var i = 0; i < img.Width; i++)
            {
                if (BitmapReader.IsPixel(i, 0, ref rgbValues, img.PixelFormat, bmpData.Stride))
                {
                    newMaze.StartPoint = new IntersectionPoint()
                    {
                        ICoord = i,
                        JCoord = 0
                    };
                }
                if (BitmapReader.IsPixel(i, img.Height - 1, ref rgbValues, img.PixelFormat, bmpData.Stride))
                {
                    newMaze.EndPoint = new IntersectionPoint()
                    {
                        ICoord = i,
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

            for (var i = 1; i < img.Width - 1; i++)
            {
                newMaze.ISections[i] = newMazePoints.Count;
                for (var j = 0; j < img.Height; j++)
                {
                    if (BitmapReader.IsPixel(i, j, ref rgbValues, img.PixelFormat, bmpData.Stride))
                    {
                        if (IsIntersection(i, j, ref rgbValues, img.PixelFormat, bmpData.Stride, img.Width, img.Height))
                        {
                            var currentPoint = new IntersectionPoint()
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
                            if (!hasSeenWallInI[j])
                            {
                                if (previousIIntersection[j] != null)
                                {
                                    currentPoint.ConnectedIntersections.Add(previousIIntersection[j]);
                                    previousIIntersection[j].ConnectedIntersections.Add(currentPoint);
                                }
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
                }
            }

            lastSeenEndConnector.ConnectedIntersections.Add(newMaze.EndPoint);
            newMaze.EndPoint.ConnectedIntersections.Add(lastSeenEndConnector);
            newMaze.Points = newMazePoints.ToArray();
            s.Stop();
            Console.WriteLine("total work " + s.ElapsedMilliseconds + "ms");
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
