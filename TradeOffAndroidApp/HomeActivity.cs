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

namespace TradeOffAndroidApp
{
    [Activity(Label = "HomeActivity", MainLauncher = true)]
    public class HomeActivity : Activity
    {
        private Button btnCategories;
        private Button btnSellProduct;
        private Button btnYourProducts;
        private Button btnLogin;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.HomePage);
            FindViews();
            HandleEvents();
            // Create your application here
        }
        private void FindViews()
        {
            btnCategories = FindViewById<Button>(Resource.Id.btnCategories);
            btnSellProduct = FindViewById<Button>(Resource.Id.btnSellProduct);
            btnYourProducts = FindViewById<Button>(Resource.Id.btnYourProducts);
            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
        }
        private void HandleEvents()
        {
            btnCategories.Click += BtnCategories_Click;
            btnSellProduct.Click += BtnSellProduct_Click;
            btnYourProducts.Click += BtnYourProducts_Click;
            btnLogin.Click += BtnLogin_Click;
        }
        
        private void BtnCategories_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(CategoriesActivity));
            StartActivity(intent);
        }
        private void BtnSellProduct_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(AddProductActivity));
            StartActivity(intent);
        }
        private void BtnYourProducts_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(ProductListActivity));
            StartActivity(intent);
        }
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(LoginActivity));
            StartActivity(intent);
        }

    }
}