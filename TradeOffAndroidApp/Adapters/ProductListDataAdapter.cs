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
using TradeOffAndroidApp.Core.Models;
using TradeOffAndroidApp.Core;

namespace TradeOffAndroidApp.Adapters
{
    public class ProductListDataAdapter : BaseAdapter<ProductModel>
    {
        List<ProductModel> items;
        private List<ProductImageModel> _images;
        private ImageConversion _imageConversion;

        Activity context;
        public ProductListDataAdapter(Activity context, List<ProductModel> items, List<ProductImageModel> images) : base()
        {
            this.context = context;
            this.items = items;
            this._images = images;
            _imageConversion = new ImageConversion();
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override ProductModel this[int position]
        {
            get
            {
                return items[position];
            }
        }

        public override int Count
        {
            get
            {
                return items.Count;
            }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            var image = _images.FirstOrDefault(i => i.Id == item.Id);
            var imageBitmap = _imageConversion.ByteArrayToBitmap(image.Image);
            if (convertView == null)
            {
                convertView = context.LayoutInflater.Inflate(Android.Resource.Layout.ActivityListItem, null);
            }
            convertView.FindViewById<TextView>(Android.Resource.Id.Text1).SetTextColor(Android.Graphics.Color.Black);
            convertView.FindViewById<TextView>(Android.Resource.Id.Text2).SetTextColor(Android.Graphics.Color.Black);
            convertView.FindViewById<TextView>(Android.Resource.Id.Text1).Text = item.Name;
            convertView.FindViewById<TextView>(Android.Resource.Id.Text2).Text = item.ShortDescription;
            convertView.FindViewById<ImageView>(Android.Resource.Id.Icon).SetImageBitmap(imageBitmap);
            return convertView;
        }
    }
}