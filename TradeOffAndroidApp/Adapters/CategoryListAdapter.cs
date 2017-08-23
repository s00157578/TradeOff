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
    //adapter for populating the category listview, implements the BaseAdapter<t> interface
    public class CategoryListAdapter:BaseAdapter<CategoryModel>
    {
        List<CategoryModel> items;
        Activity context;
        //default methods, for interface
        //sets context and the list<CategoryModel>
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
        //called for each item that will be in listview
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            if (convertView == null)
            {
                convertView = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItemActivated1, null);
            }
            //default textViews in assembly
            convertView.FindViewById<TextView>(Android.Resource.Id.Text1).SetTextColor(Android.Graphics.Color.Black);
            convertView.FindViewById<TextView>(Android.Resource.Id.Text1).Text = item.CategoryName;
            return convertView;
        }
    }
}