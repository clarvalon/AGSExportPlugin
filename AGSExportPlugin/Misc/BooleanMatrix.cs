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
    /// </summary>
    public partial class BooleanMatrix
    {
        private int height;
        private int width;
        private byte[] data;

        public BooleanMatrix(int width, int height, byte[] data = null)
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

        public static BooleanMatrix ReadFromStream(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                int width = br.ReadInt32();
                int height = br.ReadInt32();
                int byteLength = br.ReadInt32();
                byte[] data = br.ReadBytes(byteLength);

                BooleanMatrix returnMatrix = new BooleanMatrix(width, height, data);
                return returnMatrix;
            }
        }
    }
}