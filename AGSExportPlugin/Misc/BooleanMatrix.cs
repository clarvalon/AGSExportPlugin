#region License
/* AGSExportPlugin 
 * Copyright 2010-2018 - Dan Alexander
 *
 * Released under the MIT License.  See LICENSE for details. */
#endregion

using BitsetsNET;
using System;
using System.IO;

namespace Clarvalon.XAGE.Global
{
    /// <summary>
    /// Very space-efficient way of storing 2D array of bools - similar to BitArray.
    /// Typical usage of this is for Bitmap HotSpots, as GetPixel is slow on Textures.
    /// This is a decent trade off between storage space, memory and access speed.
    /// Can be optionally optimised if number of set pixels exceeds more than half of those available.
    /// </summary>
    public partial class BooleanMatrix
    {
        private int height;
        private int width;
        private bool inverted;
        RoaringBitset rb;

        public BooleanMatrix(int width, int height) 
        {
            this.height = height;
            this.width = width;
            this.inverted = false;
            this.rb = RoaringBitset.Create(new int[] { });
        }

        public BooleanMatrix(int width, int height, bool inverted, RoaringBitset rb)
        {
            this.height = height;
            this.width = width;
            this.inverted = inverted;
            this.rb = rb;
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
        /// Gets whether has been inverted to improve performance
        /// </summary>
        public bool Inverted
        {
            get
            {
                return inverted;
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

                int pos = (yPos * width) + xPos;
                
                if (inverted)
                   return !rb.Get(pos);
                else
                   return rb.Get(pos);
            }
            set
            {
                if (yPos < 0 || yPos >= height)
                    return;

                if (xPos < 0 || xPos >= width)
                    return;

                int pos = (yPos * width) + xPos;
    
                if (inverted)
                    rb.Set(pos, !value);
                else
                    rb.Set(pos, value);
            }
        }

        public void WriteToStream(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                // Write Width, Height, Inverted before RoaringBitset
                bw.Write(Width);
                bw.Write(Height);
                bw.Write(Inverted);
                rb.Serialize(bw);
            }
        }

        public static BooleanMatrix ReadFromStream(Stream stream)
        {
            int width;
            int height;
            bool inverted;
            RoaringBitset rb;
            using (BinaryReader br = new BinaryReader(stream))
            {
                width = br.ReadInt32();
                height = br.ReadInt32();
                inverted = br.ReadBoolean();
                rb = RoaringBitset.Deserialize(br);
            }

            BooleanMatrix bm = new BooleanMatrix(width, height, inverted, rb);
            return bm;
        }

        public void Optimise(int countSet)
        {
            // If over half pixels have been set then store the reverse of this RoaringBitmap (i.e. flip it)
            // This will lower the memory usage
            int size = width * height;
            int threshold = size / 2;

            if (countSet > threshold)
            {
                rb.Flip(0, size);
                inverted = true;
            }
            else
                inverted = false;
        }
    }
}
