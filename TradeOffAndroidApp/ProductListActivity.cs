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
using Plugin.SecureStorage;

namespace TradeOffAndroidApp
{
    [Activity(Label = "Product List")]
    public class ProductListActivity : Activity
    {
        private ProductRepository _productRepository;
        private TextView _textViewWarning;
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
            SetContentView(Resource.Layout.ProductsList);
            FindViews();
            if (SelectedCategoryId == 0)
            {
                if (!CrossSecureStorage.Current.HasKey("idToken"))
                {
                    GoToLogin();
                    Finish();
                }
                else
                {
                    PopulateUserProductList();
                    if (_productList.Count > 0)

                        _productListView.Adapter = new UserProductListAdapter(this, _productList);
                    else
                        _textViewWarning.Text = "You have no products to view";
                }
            }                   
            else
            {
                PopulateList(SelectedCategoryId);
                _productListView.Adapter = new ProductListDataAdapter(this, _productList, _imageList);
            }          
            _productListView.FastScrollEnabled = true;
            HandleEvents();
            // Create your application here
        }
        private async void PopulateList(int id)
        {
            if (_productList == null)
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
        private async void PopulateUserProductList()
        {
            var products = await _productRepository.GetUserProducts();
            _productList = products.ToList();
        }
        private void FindViews()
        {
            _productListView = FindViewById<ListView>(Resource.Id.ListviewProducts);
            _textViewWarning = FindViewById<TextView>(Resource.Id.txtViewWarning);
        }
        private void HandleEvents()
        {
            _productListView.ItemClick += ProductListView_ItemClick;
        }
        private void ProductListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var product = _productList[e.Position];
            var intent = new Intent();
            intent.SetClass(this, typeof(ProductViewActivity));
            intent.PutExtra("selectedProductId", product.Id);
            StartActivityForResult(intent, 100);
        }
        private void GoToLogin()
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(LoginActivity));
            StartActivity(intent);
        }
    }
}