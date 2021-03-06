﻿using System;
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
    [Activity(MainLauncher = true)]
    public class HomeActivity : Activity
    {
        private Button btnCategories;
        private Button btnSellProduct;
        private Button btnYourProducts;
        private Button btnLogin;
        private CredentialRepository _credentialRepository;
        private bool _isLoggedIn = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //required password to be set for securestorage plugin
            SecureStorageImplementation.StoragePassword = "TradeOffPassword18/08/17";
            _credentialRepository = new CredentialRepository();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.HomePage);
            //finds the views on the home page
            FindViews();
            //checks if there is is an idToken present ie logged in
            if (CrossSecureStorage.Current.HasKey("idToken"))
            {
                //if logged in sets login button text to log out
                btnLogin.Text = "Log out";
                btnLogin.SetBackgroundColor(Color.Red);
                _isLoggedIn = true;
            }
            HandleEvents();
        }
        //gets the views from page
        private void FindViews()
        {
            btnCategories = FindViewById<Button>(Resource.Id.btnCategories);
            btnSellProduct = FindViewById<Button>(Resource.Id.btnSellProduct);
            btnYourProducts = FindViewById<Button>(Resource.Id.btnYourProducts);
            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
        }
        //handles events
        private void HandleEvents()
        {
            btnCategories.Click += BtnCategories_Click;
            btnSellProduct.Click += BtnSellProduct_Click;
            btnYourProducts.Click += BtnYourProducts_Click;
            btnLogin.Click += BtnLogin_Click;
        }
        //on button click go to the different activities
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
            var intent = new Intent();
            intent.SetClass(this, typeof(ProductListActivity));
            intent.PutExtra("selectedCategoryId", 0);
            StartActivityForResult(intent, 100);
        }
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            //on login.click if logged in logout else go to login page
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