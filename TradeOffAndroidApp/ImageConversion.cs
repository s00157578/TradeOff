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
using Android.Gms.Common.Images;

namespace TradeOffAndroidApp
{
    public class ImageConversion
    {
        public byte[] BitmapToByteArray(Bitmap image)
        {
            //converts bitmap to byteArray
            byte[] arr;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Compress(Bitmap.CompressFormat.Png, 0, ms);
                arr = ms.ToArray();
            }
            return arr;
        }
        //converts byte[] to bitmap
        public Bitmap ByteArrayToBitmap(byte[] byteArray)
        {
            return BitmapFactory.DecodeByteArray(byteArray, 0, byteArray.Length);
        }
        //loads a bitmap from a file and scales down size.
        public Bitmap LoadBitmap( string fileName)
        {
            int maxWidth;
            int maxHeight;
            //sets options
            var options = new BitmapFactory.Options()
            {
                InJustDecodeBounds = false,
                InPurgeable = true,
            };
            //loads image from file
            var image = BitmapFactory.DecodeFile(fileName, options);     
           if (image != null)
           {
                var sourceSize = new Size((int)image.GetBitmapInfo().Height, (int)image.GetBitmapInfo().Width);
                maxWidth = sourceSize.Width / 3;
                maxHeight = sourceSize.Height / 3;
                //scales image
                var bitmapScaled = Bitmap.CreateScaledBitmap(image, maxWidth, maxHeight, true);
                return bitmapScaled;
           }
            return null;
        }

    }
}