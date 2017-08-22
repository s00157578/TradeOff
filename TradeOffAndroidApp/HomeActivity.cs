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
using Plugin.SecureStorage;
using Android.Graphics;
using TradeOffAndroidApp.Core.Services;

namespace TradeOffAndroidApp
{
    [Activity(Label = "HomeActivity", MainLauncher = true)]
    public class HomeActivity : Activity
    {
        private Button btnCategories;
        private Button btnSellProduct;
        private Button btnYourProducts;
        private Button btnLogin;
        private CredentialRepository _credentialRepository;
        private bool _isLoggedIn = false;
        // private TokenManager token;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            SecureStorageImplementation.StoragePassword = "TradeOffPassword18/08/17";
            _credentialRepository = new CredentialRepository();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.HomePage);
            FindViews();
            //  token = new TokenManager();
            //token.DecodeIdToken();
            if (CrossSecureStorage.Current.HasKey("idToken"))
            {
                btnLogin.Text = "Log out";
                btnLogin.SetBackgroundColor(Color.Red);
                _isLoggedIn = true;
            }
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
            if(_isLoggedIn)
            {

                btnLogin.Text = "Log In";
                CrossSecureStorage.Current.DeleteKey("idToken");
                if(_credentialRepository.LogOut())
                {
                    
                }
                btnLogin.SetBackgroundColor(Android.Graphics.Color.ParseColor("#ff99cc00"));
                _isLoggedIn = false;
            }
            else
            {
                var intent = new Intent(this, typeof(LoginActivity));
                StartActivity(intent);
            }          
        }

    }
}