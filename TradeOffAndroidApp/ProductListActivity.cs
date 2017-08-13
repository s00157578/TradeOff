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
using TradeOffAndroidApp.Core.Services;
using TradeOffAndroidApp.Core;
using TradeOffAndroidApp.Adapters;

namespace TradeOffAndroidApp
{
    [Activity(Label = "Product List")]
    public class ProductListActivity : Activity
    {
        private ProductRepository _productRepository;
        private ProductImageRepository _productImageRepository;
        private List<ProductImageModel> _imageList;
        private List<ProductModel> _productList;
        private ListView _productListView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            _productRepository = new ProductRepository();
            _productImageRepository = new ProductImageRepository();
            base.OnCreate(savedInstanceState);
            var SelectedCategoryId = Intent.Extras.GetInt("selectedCategoryId");
            PopulateList(SelectedCategoryId);
            SetContentView(Resource.Layout.ProductsList);
            FindViews();
            _productListView.Adapter = new ProductListDataAdapter(this, _productList, _imageList);
            _productListView.FastScrollEnabled = true;
            // Create your application here
        }
        private async void PopulateList(int id)
        {
            if (_productListView == null)
            {
                var products = await _productRepository.GetProductsByCategory(id);
                _productList = products.ToList();
            }
            if(_imageList == null)
            {
                var images = await _productImageRepository.GetMainProductImagesByCategory(id);
                _imageList = images.ToList();
            }
        }
        private void FindViews()
        {
            _productListView = FindViewById<ListView>(Resource.Id.ListviewProducts);
        }
    }
}