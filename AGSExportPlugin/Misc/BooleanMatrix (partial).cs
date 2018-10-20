#region License
/* AGSExportPlugin 
 * Copyright 2010-2018 - Dan Alexander
 *
 * Released under the MIT License.  See LICENSE for details. */
#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Clarvalon.XAGE.Global
{
    public partial class BooleanMatrix
    {
        public Bitmap ToBitmap()
        {
            Bitmap bitmap = new Bitmap(Width, Height);

            // TODO:  Plug in FastBitmap

            for (int x = 0; x < Width; x += 1)
            {
                for (int y = 0; y < Height; y += 1)
                {
                    if (this[x, y])
                        bitmap.SetPixel(x, y, Color.White);
                }
            }
            return bitmap;
        }

    }
}
