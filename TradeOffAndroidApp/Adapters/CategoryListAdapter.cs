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

namespace TradeOffAndroidApp.Adapters
{
    public class CategoryListAdapter:BaseAdapter<CategoryModel>
    {
        List<CategoryModel> items;
        Activity context;
        public CategoryListAdapter(Activity context, List<CategoryModel> items) : base()
        {
            this.context = context;
            this.items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override CategoryModel this[int position]
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
                convertView = context.LayoutInflater.Inflate(Resource.Layout.Categories, null);
            }
            convertView.FindViewById<TextView>(Resource.Id.textCategory).Text = item.CategoryName;
            return convertView;
        }
    }
}