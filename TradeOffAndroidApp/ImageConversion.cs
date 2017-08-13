using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using Android.Graphics;

namespace TradeOffAndroidApp
{
    public class ImageConversion
    {
        public byte[] BitmapToByteArray(Bitmap image)
        {
            byte[] arr;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Compress(Bitmap.CompressFormat.Png, 0, ms);
                arr = ms.ToArray();
            }
            return arr;
        }
        public Bitmap ByteArrayToBitmap(byte[] byteArray)
        {
            return BitmapFactory.DecodeByteArray(byteArray, 0, byteArray.Length);
        }

    }
}