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
using Plugin.SecureStorage;

namespace TradeOffAndroidApp
{
    [Activity(Label = "ProductViewActivity")]
    public class ProductViewActivity : Activity
    {
        private ImageConversion _imageCoverter;
        private ProductRepository _productRepository;
        private ProductImageRepository _productImageRepository;
        private ProductModel _product;
        private List<ProductImageModel> _imageList;
        private ImageView _imageView;
        private TextView _nameView;
        private Button _btnLocation;
        private TextView _shortDescriptionView;
        private TextView _FullDescriptionView;
        private TextView _priceView;
        private Button _btnEmail;
        private Button _btnDelete;
        private Button _btnEdit;
        private CredentialRepository _credentialRepository;
        private string _userId;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            _productRepository = new ProductRepository();
            _credentialRepository = new CredentialRepository();
            _productImageRepository = new ProductImageRepository();
            _imageCoverter = new ImageConversion();           
            base.OnCreate(savedInstanceState);
            var selectedProductId = Intent.Extras.GetInt("selectedProductId");
            GetProduct(selectedProductId);
            GetImages(selectedProductId);
            SetContentView(Resource.Layout.IndividualProductView);
            GetId();
            FindViews();
            SetViews();
            manageAuthorisation();
            HandleEvents();
        }
        private async void GetId()
        {
            _userId = await _credentialRepository.GetUserId();
        }
        private async void GetProduct(int productId)
        {
            _product = await _productRepository.GetProduct(productId);
        }
        private async void GetImages(int productId)
        {
             IEnumerable<ProductImageModel> images = await _productImageRepository.GetProductImages(productId);
            _imageList = images.ToList();
        }
        private void manageAuthorisation()
        {
            if (_userId != _product.UserId)
            {
                _btnDelete.Visibility = ViewStates.Invisible;
                _btnEdit.Visibility = ViewStates.Invisible;
            }
            else
                _btnEmail.Visibility = ViewStates.Invisible;
        }
        private void FindViews()
        {
            _imageView = FindViewById<ImageView>(Resource.Id.imageProduct);
            _nameView = FindViewById<TextView>(Resource.Id.txtViewProductName);
            _btnLocation = FindViewById<Button>(Resource.Id.btnLocation);
            _shortDescriptionView = FindViewById<TextView>(Resource.Id.txtViewShortDescription);
            _FullDescriptionView = FindViewById<TextView>(Resource.Id.txtViewFullDescription);
            _priceView = FindViewById<TextView>(Resource.Id.txtViewPrice);
            _btnEmail = FindViewById<Button>(Resource.Id.btnEmail);
            _btnEdit = FindViewById<Button>(Resource.Id.btnEdit);
            _btnDelete = FindViewById<Button>(Resource.Id.btnDelete);
        }
        private void SetViews()
        {
            var mainImage = _imageList.FirstOrDefault(i => i.IsMainImage == true);
            _imageView.SetImageBitmap(_imageCoverter.ByteArrayToBitmap(mainImage.Image));
            _nameView.Text = _product.Name;
            //_locationView.Text = _product.Location;
            _shortDescriptionView.Text = _product.ShortDescription;
            _FullDescriptionView.Text = _product.FullDescription;
            _priceView.Text = _product.Price.ToString();
        }
        private void HandleEvents()
        {
            _btnEmail.Click += btnEmail_Click;
            _btnLocation.Click += btnLocation_Click;
            _btnEdit.Click += btnEdit_Click;
            _btnDelete.Click += btnDelete_Click;

        }

        private void btnEmail_Click(object sender, EventArgs e)
        {
            if (CrossSecureStorage.Current.HasKey("idToken"))
            {
            }
            else
            {
                GoToLogin();
                Finish();
            }

        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(EditProductActivity));
            intent.PutExtra("selectedProductId", _product.Id);
            StartActivityForResult(intent, 100);
        } 
        private void btnDelete_Click(object sender, EventArgs e)
        {
            _productRepository.DeleteProduct(_product.Id);
            var intent = new Intent();
            intent.SetClass(this, typeof(ProductListActivity));
            intent.PutExtra("selectedCategoryId", _product.CategoryId);
            StartActivity(intent);
            Finish();
        }
        private void btnLocation_Click (object sender, EventArgs e)
        {
            var coOrds = $"geo:{_product.Location}";
            var geoUri = Android.Net.Uri.Parse(coOrds);
            var mapIntent = new Intent(Intent.ActionView, geoUri);
            StartActivity(mapIntent);
        }
        private void GoToLogin()
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(LoginActivity));
            StartActivity(intent);
        }
    }
}