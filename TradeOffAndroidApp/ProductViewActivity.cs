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
        private EmailRepository _emailRepository;
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
        private TextView _textViewWarning;
        private CredentialRepository _credentialRepository;
        private string _userId;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            _emailRepository = new EmailRepository();
            _productRepository = new ProductRepository();
            _credentialRepository = new CredentialRepository();
            _productImageRepository = new ProductImageRepository();
            _imageCoverter = new ImageConversion();           

            base.OnCreate(savedInstanceState);
            //gets productId from intent
            var selectedProductId = Intent.Extras.GetInt("selectedProductId");
            //gets the product
            GetProduct(selectedProductId);
            //gets the product images
            GetImages(selectedProductId);
            //sets view
            SetContentView(Resource.Layout.IndividualProductView);
            //gets userId
            GetUserId();
            //finds the views
            FindViews();
            SetViews();
            //manages what buttons are available to user
            ManageAuthorisation();
            HandleEvents(); 
        }
        private async void GetUserId()
        {
            //gets logged in userId
            _userId = await _credentialRepository.GetUserId();
        }
        private async void GetProduct(int productId)
        {
            //gets product
            _product = await _productRepository.GetProduct(productId);
        }
        private async void GetImages(int productId)
        {
            //gets product imaes
             IEnumerable<ProductImageModel> images = await _productImageRepository.GetProductImages(productId);
            _imageList = images.ToList();
        }
        private async void ManageAuthorisation()
        {
            //if it is not user product not able to delete or edit
            if (_userId != _product.UserId)
            {
                _btnDelete.Visibility = ViewStates.Invisible;
                _btnEdit.Visibility = ViewStates.Invisible;
            }
            //else if it is user product not able to email themselves
            else
                _btnEmail.Visibility = ViewStates.Invisible;
            bool hasEmailed = await _emailRepository.HasEmailedBefore(_product.Id);
            if (hasEmailed)
            {
                //if has emailed before not able to email again
                _btnEmail.Visibility = ViewStates.Invisible;
                _textViewWarning.Text = "You already emailed the seller of this product.";
            }         
        }
        //finds views
        private void FindViews()
        {
            _textViewWarning = FindViewById<TextView>(Resource.Id.txtViewWarning);
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
        //populates the views with the product content
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
        //handleEvents
        private void HandleEvents()
        {
            _btnEmail.Click += BtnEmail_Click;
            _btnLocation.Click += BtnLocation_Click;
            _btnEdit.Click += BtnEdit_Click;
            _btnDelete.Click += BtnDelete_Click;

        }
        //emails the seller if has token
        private void BtnEmail_Click(object sender, EventArgs e)
        {
            if (CrossSecureStorage.Current.HasKey("idToken"))
            {
                if(_emailRepository.EmailSeller(_product.Id))
                {
                    _btnEmail.Visibility = ViewStates.Invisible;
                    _textViewWarning.Text = "email sent to the seller of this product.";
                }
                //if not successful
                else
                    _textViewWarning.Text = "Something went wrong. Try again Later";
            }
            else
            {
                //go to login page if not logged in
                GoToLogin();
                Finish();
            }

        }
        //go to edit activity
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(EditProductActivity));
            intent.PutExtra("selectedProductId", _product.Id);
            StartActivityForResult(intent, 100);
        } 
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            //deletes the current product and goes back to product list needs to reload 
            _productRepository.DeleteProduct(_product.Id);
            var intent = new Intent();
            intent.SetClass(this, typeof(ProductListActivity));
            intent.PutExtra("selectedCategoryId", _product.CategoryId);
            StartActivity(intent);
            Finish();
        }
        //opens map at the products location
        private void BtnLocation_Click (object sender, EventArgs e)
        {
            var coOrds = $"geo:{_product.Location}";
            var geoUri = Android.Net.Uri.Parse(coOrds);
            var mapIntent = new Intent(Intent.ActionView, geoUri);
            StartActivity(mapIntent);
        }
        //goes to login page
        private void GoToLogin()
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(LoginActivity));
            StartActivity(intent);
        }
    }
}