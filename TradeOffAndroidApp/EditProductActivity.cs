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

namespace TradeOffAndroidApp
{
    [Activity(Label = "EditProductActivity")]
    public class EditProductActivity : Activity
    {
        private EditText _editName;
        private EditText _editShortDescription;
        private EditText _editFullDescription;
        private EditText _editPrice;
        private TextView _textViewWarning;
        private Button _btnSubmit;
        private ProductRepository _productRepository;
        ProductModel product;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            _productRepository = new ProductRepository();
            base.OnCreate(savedInstanceState);           
            var selectedProductId = Intent.Extras.GetInt("selectedProductId");
            GetProduct(selectedProductId);
            SetContentView(Resource.Layout.EditProductView);
            GetViews();
            PopulateViews();
            HandleEvents();
            // Create your application here
        }
        //gets all the views from the editProduct view
        private void GetViews()
        {
            _editName = FindViewById<EditText>(Resource.Id.txtEditName);
            _editShortDescription = FindViewById<EditText>(Resource.Id.txtEditShortDescription);
            _editFullDescription = FindViewById<EditText>(Resource.Id.txtEditFullDescription);
            _editPrice = FindViewById<EditText>(Resource.Id.txtEditPrice);
            _btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);
            _textViewWarning = FindViewById<TextView>(Resource.Id.txtViewWarning);
        }
        private async void GetProduct(int id)
        {
            product = await _productRepository.GetProduct(id);
        }
        //populates the edit info
        private void PopulateViews()
        {
            _editName.Text = product.Name;
            _editShortDescription.Text = product.ShortDescription;
            _editFullDescription.Text = product.FullDescription;
            _editPrice.Text = product.Price.ToString();
        }
        private void HandleEvents()
        {
            _btnSubmit.Click += BtnSubmit_Click;
        }
        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            //reads inputs for basic validation
            bool isValid = ReadInputs();
            if (isValid)
            {
                int userId = 1;
                    //creates product
                    ProductUpdateModel productToUpdate = new ProductUpdateModel()
                    {
                        Name = _editName.Text,
                        ShortDescription = _editShortDescription.Text,
                        FullDescription = _editFullDescription.Text,
                        Price = decimal.Parse(_editPrice.Text),
                        UserId = userId,
                    };
                    bool isUpdated = _productRepository.UpdateProduct(product.CategoryId,product.Id, productToUpdate);
                if (isUpdated)
                {
                    GoToProductView(product.Id);
                }
                else
                    _textViewWarning.Text = "there was a problem updating the product";
            }
        }
        private bool ReadInputs()
        {
            if (!IsInputValid(_editName))
            {
                _textViewWarning.Text = "Product Name is Required";
                return false;
            }
            if (!IsInputValid(_editShortDescription))
            {
                _textViewWarning.Text = "Short Description is Required";
                return false;
            }
            if (!IsInputValid(_editFullDescription))
            {
                _textViewWarning.Text = "FullDescription is Required";
                return false;
            }
            if (!IsInputValid(_editPrice))
            {
                _textViewWarning.Text = "Price is Required";
                return false;
            }
            return true;
        }
        private bool IsInputValid(EditText text)
        {
            if (string.IsNullOrEmpty(text.Text))
                return false;
            return true;
        }
        private void GoToProductView(int productId)
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(ProductViewActivity));
            intent.PutExtra("selectedProductId", productId);
            StartActivityForResult(intent, 100);
        }
    }
}