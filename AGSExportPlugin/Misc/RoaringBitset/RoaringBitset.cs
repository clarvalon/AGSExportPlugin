﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BitsetsNET
{
    public class RoaringBitset : IEnumerable 
    {

        RoaringArray containers = new RoaringArray();

        public static RoaringBitset Create(int[] input)
        {
            RoaringBitset rb = new RoaringBitset();
            foreach (int i in input)
            {
                rb.Add(i);
            }
            return rb;
        }

        /// <summary>
        /// Adds the specified value to the current bitmap
        /// </summary>
        /// <param name="x">Value to be added</param>
        public void Add(int x)
        {
            ushort highBits = Utility.GetHighBits(x);
            int containerIndex = this.containers.GetIndex(highBits);

            if (containerIndex >= 0)
            {
                // a container exists at this index already.
                // find the right container, get the low order bits to add to the container and add them
                this.containers.SetContainerAtIndex(containerIndex,
                                                    this.containers.GetContainerAtIndex(containerIndex)
                                                        .Add(Utility.GetLowBits(x)));
            }
            else
            {
                // no container exists for this index
                // create a new ArrayContainer, since it will only hold one integer to start
                // get the low order bits and att to the newly created container
                // add the newly created container to the array of containers
                ArrayContainer ac = new ArrayContainer();
                this.containers.InsertNewKeyValueAt(-containerIndex - 1, highBits, ac.Add(Utility.GetLowBits(x)));
            }
        }

        /// <summary>
        /// Add to the current bitmap all integers in [rangeStart,rangeEnd).
        /// </summary>
        /// <param name="rangeStart">Inclusive beginning of range</param>
        /// <param name="rangeEnd">Exclusive ending of range</param>
        public void Add(int rangeStart, int rangeEnd)
        {
            if (rangeStart >= rangeEnd)
            {
                return; // empty range
            }

            ushort hbStart = Utility.GetHighBits(rangeStart);
            ushort lbStart = Utility.GetLowBits(rangeStart);
            ushort hbLast = Utility.GetHighBits(rangeEnd - 1);
            ushort lbLast = Utility.GetLowBits(rangeEnd - 1);

            for (ushort hb = hbStart; hb <= hbLast; ++hb)
            {

                // first container may contain partial range
                ushort containerStart = 0;
                if (hb == hbStart)
                {
                    containerStart = lbStart;
                }

                // last container may contain partial range
                ushort containerLast = (hb == hbLast) ? lbLast : ushort.MaxValue;
                int containerIndex = this.containers.GetIndex(hb);

                if (containerIndex >= 0)
                {
                    Container c = this.containers.GetContainerAtIndex(containerIndex)
                                                 .Add(containerStart, (ushort)(containerLast + 1));
                    this.containers.SetContainerAtIndex(containerIndex, c);
                }
                else
                {
                    Container ac = new ArrayContainer(100);
                    ac = ac.Add(lbStart, (ushort)(lbLast + 1));
                    this.containers.InsertNewKeyValueAt(-containerIndex - 1, hb, ac);
                }
            }
        }

        /// <summary>
        /// Remove from the current bitmap all integers in [rangeStart,rangeEnd).
        /// </summary>
        /// <param name="rangeStart">inclusive beginning of range</param>
        /// <param name="rangeEnd">exclusive ending of range</param>
        public void Remove(int rangeStart, int rangeEnd)
        {
            if (rangeStart >= rangeEnd)
            {
                return; // empty range
            }

            ushort hbStart = Utility.GetHighBits(rangeStart);
            ushort lbStart = Utility.GetLowBits(rangeStart);
            ushort hbLast = Utility.GetHighBits(rangeEnd - 1);
            ushort lbLast = Utility.GetLowBits(rangeEnd - 1);

            if (hbStart == hbLast)
            {
                int containerIndex = containers.GetIndex(hbStart);

                if (containerIndex < 0)
                {
                    return;
                }

                Container c = containers.GetContainerAtIndex(containerIndex)
                                        .Remove(lbStart, (ushort)(lbLast + 1));

                if (c.GetCardinality() > 0)
                {
                    containers.SetContainerAtIndex(containerIndex, c);
                }
                else
                {
                    containers.RemoveAtIndex(containerIndex);
                }
                return;
            }

            int ifirst = containers.GetIndex(hbStart);
            int ilast = containers.GetIndex(hbLast);

            if (ifirst >= 0)
            {
                if (lbStart != 0)
                {
                    Container c = containers.GetContainerAtIndex(ifirst)
                                            .Remove(lbStart, ushort.MaxValue);

                    if (c.GetCardinality() > 0)
                    {
                        containers.SetContainerAtIndex(ifirst, c);
                        ifirst++;
                    }
                }
            }
            else
            {
                ifirst = -ifirst - 1;
            }

            if (ilast >= 0)
            {
                if (lbLast != ushort.MaxValue)
                {
                    Container c = containers.GetContainerAtIndex(ilast)
                                            .Remove(0, (ushort)(lbLast + 1));

                    if (c.GetCardinality() > 0)
                    {
                        containers.SetContainerAtIndex(ilast, c);
                    }
                    else
                    {
                        ilast++;
                    }
                }
                else
                {
                    ilast++;
                }
            }
            else
            {
                ilast = -ilast - 1;
            }

            containers.RemoveIndexRange(ifirst, ilast);
        }

        public static RoaringBitset And(RoaringBitset x1, RoaringBitset x2)
        {
            RoaringBitset answer = new RoaringBitset();
            int length1 = x1.containers.Size, length2 = x2.containers.Size;
            int pos1 = 0, pos2 = 0;

            while (pos1 < length1 && pos2 < length2)
            {
                ushort s1 = x1.containers.GetKeyAtIndex(pos1);
                ushort s2 = x2.containers.GetKeyAtIndex(pos2);

                if (s1 == s2)
                {
                    Container c1 = x1.containers.GetContainerAtIndex(pos1);
                    Container c2 = x2.containers.GetContainerAtIndex(pos2);
                    Container c = c1.And(c2);

                    if (c.GetCardinality() > 0)
                    {
                        answer.containers.Append(s1, c);
                    }

                    ++pos1;
                    ++pos2;
                }
                else if (s1 < s2) // s1 < s2
                {
                    pos1 = x1.containers.AdvanceUntil(s2, pos1);
                }
                else // s1 > s2
                {
                    pos2 = x2.containers.AdvanceUntil(s1, pos2);
                }
            }
            return answer;
        }

        /// <summary>
        /// Performs an in-place intersection of two Roaring Bitsets.
        /// </summary>
        /// <param name="other">the second Roaring Bitset to intersect</param>
        private void AndWith(RoaringBitset other)
        {
            int thisLength = this.containers.Size;
            int otherLength = other.containers.Size;
            int pos1 = 0, pos2 = 0, intersectionSize = 0;

            while (pos1 < thisLength && pos2 < otherLength)
            {
                ushort s1 = this.containers.GetKeyAtIndex(pos1);
                ushort s2 = other.containers.GetKeyAtIndex(pos2);

                if (s1 == s2)
                {
                    Container c1 = this.containers.GetContainerAtIndex(pos1);
                    Container c2 = other.containers.GetContainerAtIndex(pos2);
                    Container c = c1.IAnd(c2);

                    if (c.GetCardinality() > 0)
                    {
                        this.containers.ReplaceKeyAndContainerAtIndex(intersectionSize++, s1, c);
                    }

                    ++pos1;
                    ++pos2;
                }
                else if (s1 < s2)
                { // s1 < s2
                    pos1 = this.containers.AdvanceUntil(s2, pos1);
                }
                else
                { // s1 > s2
                    pos2 = other.containers.AdvanceUntil(s1, pos2);
                }
            }
            this.containers.Resize(intersectionSize);
        }

        public int Select(int j)
        {
            int leftover = j;
            for (int i = 0; i < this.containers.Size; i++)
            {
                Container c = this.containers.GetContainerAtIndex(i);
                int thisCardinality = c.GetCardinality();
                if (thisCardinality > leftover)
                {
                    uint keycontrib = (uint)this.containers.GetKeyAtIndex(i) << 16;
                    uint lowcontrib = (uint)c.Select(leftover);
                    return (int)(lowcontrib + keycontrib);
                }
                leftover -= thisCardinality;
            }
            throw new ArgumentOutOfRangeException("select " + j + " when the cardinality is " + this.Cardinality());
        }

        /// <summary>
        /// Return whether the given index is a member of this set
        /// </summary>
        /// <param name="index">the index to test</param>
        /// <returns>True if the index is a member of this set</returns>
        public bool Get(int index)
        {
            ushort highBits = Utility.GetHighBits(index);
            int containerIndex = containers.GetIndex(highBits);

            // a container exists at this index already.
            // find the right container, get the low order bits to add to the 
            // container and add them
            if (containerIndex >= 0)
            {
                return containers.GetContainerAtIndex(containerIndex)
                                 .Contains(Utility.GetLowBits(index));
            }
            else
            {
                // no container exists for this index
                return false;
            }
        }

        /// <summary>
        /// Adds the current index to the set if value is true, otherwise 
        /// removes it if the set contains it.
        /// </summary>
        /// <param name="index">The index to set</param>
        /// <param name="value">Boolean of whether to add or remove the index</param>
        public void Set(int index, bool value)
        {
            if (value)
            {
                Add(index);
            }
            else
            {
                ushort hb = Utility.GetHighBits(index);
                int containerIndex = containers.GetIndex(hb);

                if (containerIndex > -1)
                {
                    Container updatedContainer = containers.GetContainerAtIndex(containerIndex)
                                                           .Remove(Utility.GetLowBits(index));
                    containers.SetContainerAtIndex(containerIndex, updatedContainer);
                }
            }
        }

        /// <summary>
        /// For indices in the range [start, end) add the index to the set if
        /// the value is true, otherwise remove it.
        /// </summary>
        /// <param name="start">the index to start from (inclusive)</param>
        /// <param name="end">the index to stop at (exclusive)</param>
        public void Set(int start, int end, bool value)
        {
            if (value)
            {
                Add(start, end);
            }
            else
            {
                Remove(start, end);
            }
        }

        /// <summary>
        /// The number of members of the set
        /// </summary>
        /// <returns>an integer for the number of members in the set</returns>
        public int Cardinality()
        {
            int size = 0;
            for (int i = 0; i < this.containers.Size; i++)
            {
                size += this.containers.GetContainerAtIndex(i).GetCardinality();
            }
            return size;
        }

        /// <summary>
        /// If the given index is not in the set add it, otherwise remove it.
        /// </summary>
        /// <param name="index">The index to flip</param>
        public void Flip(int x)
        {
            ushort hb = Utility.GetHighBits(x);
            int i = containers.GetIndex(hb);

            if (i >= 0)
            {
                Container c = containers.GetContainerAtIndex(i).Flip(Utility.GetLowBits(x));
                if (c.GetCardinality() > 0)
                {
                    containers.SetContainerAtIndex(i, c);
                }
                else
                {
                    containers.RemoveAtIndex(i);
                }
            }
            else
            {
                ArrayContainer newac = new ArrayContainer();
                containers.InsertNewKeyValueAt(-i - 1, hb, newac.Add(Utility.GetLowBits(x)));
            }
        }

        /// <summary>
        /// For indices in the range [start, end) add the index to the set if
        /// it does not exists, otherwise remove it.
        /// </summary>
        /// <param name="start">the index to start from (inclusive)</param>
        /// <param name="end">the index to stop at (exclusive)</param>
        public void Flip(int start, int end)
        {
            if (start >= end)
            {
                return; // empty range
            }

            // Separate out the ranges of higher and lower-order bits
            int hbStart = Utility.ToIntUnsigned(Utility.GetHighBits(start));
            int lbStart = Utility.ToIntUnsigned(Utility.GetLowBits(start));
            int hbLast = Utility.ToIntUnsigned(Utility.GetHighBits(end - 1));
            int lbLast = Utility.ToIntUnsigned(Utility.GetLowBits(end - 1));

            for (int hb = hbStart; hb <= hbLast; hb++)
            {
                // first container may contain partial range
                int containerStart = (hb == hbStart) ? lbStart : 0;
                // last container may contain partial range
                int containerLast = (hb == hbLast) ? lbLast : Utility.GetMaxLowBitAsInteger();
                int i = containers.GetIndex((ushort)hb);

                if (i >= 0)
                {
                    Container c = containers.GetContainerAtIndex(i)
                                            .INot(containerStart, containerLast + 1);
                    if (c.GetCardinality() > 0)
                    {
                        containers.SetContainerAtIndex(i, c);
                    }
                    else
                    {
                        containers.RemoveAtIndex(i);
                    }
                }
                else
                {
                    containers.InsertNewKeyValueAt(-i - 1, (ushort)hb,
                        Container.RangeOfOnes((ushort)containerStart, (ushort)(containerLast + 1)));
                }
            }
        }

        public override bool Equals(Object o)
        {
            if (o is RoaringBitset)
            {
                RoaringBitset srb = (RoaringBitset)o;
                return srb.containers.Equals(this.containers);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return containers.GetHashCode();
        }


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }

        public BitArray ToBitArray()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Write a binary serialization of this roaring bitset.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        public void Serialize(Stream stream)
        {
            //We don't care about the encoding, but we have to specify something to be able to set the stream as leave open.
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                containers.Serialize(writer);
            }
        }

        public void Serialize(BinaryWriter writer)
        {
            containers.Serialize(writer);
        }

        /// <summary>
        /// Read a binary serialization of a roaring bitset, as written by the Serialize method.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The bitset deserialized from the stream.</returns>
        public static RoaringBitset Deserialize(Stream stream)
        {
            RoaringBitset bitset = new RoaringBitset();

            //We don't care about the encoding, but we have to specify something to be able to set the stream as leave open.
            using (BinaryReader reader = new BinaryReader(stream))
            {
                bitset.containers = RoaringArray.Deserialize(reader);
            }

            return bitset;
        }

        /// <summary>
        /// Read a binary serialization of a roaring bitset, as written by the Serialize method.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The bitset deserialized from the stream.</returns>
        public static RoaringBitset Deserialize(BinaryReader reader)
        {
            RoaringBitset bitset = new RoaringBitset();

            bitset.containers = RoaringArray.Deserialize(reader);
          
            return bitset;
        }

        /// <summary>
        /// Get an enumerator of the set indices of this bitset.
        /// </summary>
        /// <returns>A enumerator giving the set (i.e. for which the bit is '1' or true) indices for this bitset.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<int> GetEnumerator()
        {
            return containers.GetEnumerator();
        }

        /// <summary>
        /// Create a new bitset that is a deep copy of this one.
        /// </summary>
        /// <returns>The cloned bitset</returns>
        public RoaringBitset Clone()
        {
            RoaringBitset x = new RoaringBitset();
            x.containers = containers.Clone();
            return x;
        }

        /// <summary>
        /// Get optimised version of this RoaringBitset (empty = optimal, full = not optimal)
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public RoaringBitset GetOptimised(int size, out bool flipped)
        {
            var newRb = this.Clone();
            newRb.Flip(0, size);

            if (newRb.containers.Size < this.containers.Size)
            {
                flipped = true;
                return newRb;
            }
            else
            {
                flipped = false;
                return this;
            }
        }

        
    }
}
