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
using TradeOffAndroidApp.Core;

namespace TradeOffAndroidApp.Adapters
{
    public class UserProductListAdapter : BaseAdapter<ProductModel>
    {
        List<ProductModel> items;
        Activity context;
        public UserProductListAdapter(Activity context, List<ProductModel> items) : base()
        {
            this.context = context;
            this.items = items;
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
            if (convertView == null)
            {
                convertView = context.LayoutInflater.Inflate(Android.Resource.Layout.ActivityListItem, null);
            }
            convertView.FindViewById<TextView>(Android.Resource.Id.Text1).SetTextColor(Android.Graphics.Color.Black);
            convertView.FindViewById<TextView>(Android.Resource.Id.Text1).Text = item.Name;
            return convertView;
        }
    }
}