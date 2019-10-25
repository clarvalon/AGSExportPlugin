#region License
/* AGSExportPlugin 
 * Copyright 2010-2018 - Dan Alexander
 *
 * Released under the MIT License.  See LICENSE for details. */
#endregion

using System;
using System.IO;

namespace Clarvalon.XAGE.Global
{
    /// <summary>
    /// Reasonably space-efficient way of storing 2D array of bools - similar to BitArray.
    /// Typical usage of this is for Bitmap HotSpots.
    /// This version is no longer used, but code kept handy as reference in case we need to switch back quickly.
    /// </summary>
    public partial class BooleanMatrixOld
    {
        private int height;
        private int width;
        private byte[] data;

        public BooleanMatrixOld(int width, int height, byte[] data = null)
        {
            this.height = height;
            this.width = width;

            // Calculate the needed number of bits and bytes
            int bitCount = this.height * this.width;
            int byteCount = bitCount >> 3;
            if (bitCount % 8 != 0)
            {
                byteCount++;
            }

            // Allocate the needed number of bytes
            if (data == null)
                this.data = new byte[byteCount];
            else
                this.data = data;
        }

        /// <summary>
        /// Gets the number of rows in this bit matrix.
        /// </summary>
        public int Height
        {
            get
            {
                return height;
            }
        }
        /// <summary>
        /// Gets the number of columns in this bit matrix.
        /// </summary>
        public int Width
        {
            get
            {
                return width;
            }
        }

        /// <summary>
        /// Gets/Sets the value at the specified row and column index.
        /// </summary>
        /// <param name="yPos"></param>
        /// <param name="xPos"></param>
        /// <returns></returns>
        public bool this[int xPos, int yPos]
        {
            get
            {
                if (yPos < 0 || yPos >= height)
                    return false;

                if (xPos < 0 || xPos >= width)
                    return false;

                int pos = yPos * width + xPos;
                int index = pos % 8;
                pos >>= 3;
                return (data[pos] & (1 << index)) != 0;
            }
            set
            {
                if (yPos < 0 || yPos >= height)
                    return;

                if (xPos < 0 || xPos >= width)
                    return;

                int pos = yPos * width + xPos;
                int index = pos % 8;
                pos >>= 3;
                data[pos] &= (byte)(~(1 << index));

                if (value)
                {
                    data[pos] |= (byte)(1 << index);
                }
            }
        }

        public byte[] GetBytes()
        {
            return data;
        }

        public void WriteToStream(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                // Write Width & Height
                bw.Write(Width);
                bw.Write(Height);

                // Write Content Length (so we know how much to read)
                bw.Write(data.Length);

                // Write Content (byte array)
                bw.Write(data);
            }
        }

        public static BooleanMatrixOld ReadFromStream(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                int width = br.ReadInt32();
                int height = br.ReadInt32();
                int byteLength = br.ReadInt32();
                byte[] data = br.ReadBytes(byteLength);

                BooleanMatrixOld returnMatrix = new BooleanMatrixOld(width, height, data);
                return returnMatrix;
            }
        }

        public int ReduceNoise()
        {
            // Slow ...
            int pixelsRemoved = 0;
            for (int x = 0; x < width; x += 1)
            {
                for (int y = 0; y < height; y += 1)
                {
                    if (this[x, y])
                        if (ReduceNoise(x, y))
                            pixelsRemoved += 1;
                }
            }
            return pixelsRemoved;
        }

        public const int NoiseRange = 1;
        public const int NoiseCount = 2;

        private bool ReduceNoise(int x, int y)
        {
            int xMin = Math.Max(x - NoiseRange, 0);
            int yMin = Math.Max(y - NoiseRange, 0);

            int xMax = Math.Min(x + NoiseRange, width);
            int yMax = Math.Min(y + NoiseRange, height);

            int found = 0;
            for (int xx = xMin; xx < xMax; xx += 1)
            {
                for (int yy = yMin; yy < yMax; yy += 1)
                {
                    if (this[xx, yy])
                    {
                        found += 1;
                        if (found > NoiseCount)
                            return false;
                    }
                }
            }

            // Not found enough - remove
            this[x, y] = false;
            return true;
        }
    }
}