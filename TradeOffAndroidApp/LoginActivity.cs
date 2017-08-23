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
using TradeOffAndroidApp.Core.Services;
using Plugin.SecureStorage;

namespace TradeOffAndroidApp
{
    [Activity(Label = "Login")]
    public class LoginActivity : Activity
    {
        private TextView _textViewWarning;
        private EditText _textEmail;
        private EditText _textPassword;
        private Button _btnLogin;
        private Button _btnCreateAccount;
        private CredentialRepository _credentialRepository;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            _credentialRepository = new CredentialRepository();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);
            GetViews();
            HandleEvents();
            // Create your application here
        }
        private void GetViews()
        {
            _textViewWarning = FindViewById<TextView>(Resource.Id.txtViewWarning);
            _textEmail = FindViewById<EditText>(Resource.Id.txtEditEmail);
            _textPassword = FindViewById<EditText>(Resource.Id.txtEditPassword);
            _btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            _btnCreateAccount = FindViewById<Button>(Resource.Id.btnCreateAccount);
        }
        private void HandleEvents()
        {
            _btnLogin.Click += _btnLogin_Click;
            _btnCreateAccount.Click += _btnCreateAccount_Click;
        }

        private void _btnCreateAccount_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(CreateAccountActivity));
            StartActivity(intent);
            Finish();
        }

        private void _btnLogin_Click(object sender, EventArgs e)
        {
            bool isValid = IsInputValid();
            if (isValid)
            {
                CredentialModel account = new CredentialModel()
                {
                    Email = _textEmail.Text,
                    Password = _textPassword.Text
                };
                _credentialRepository.LoginAsync(account);
                if (CrossSecureStorage.Current.HasKey("idToken"))
                {
                    GoToHome();
                }
                else
                    _textViewWarning.Text = "there was an error on login";

            }
            else
                _textViewWarning.Text = "Input was not valid";
        }
        private bool IsInputValid()
        {
            if (string.IsNullOrEmpty(_textEmail.Text))
                return false;
            if (string.IsNullOrEmpty(_textPassword.Text))
                return false;
            return true;
        }
        private void GoToHome()
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(HomeActivity));
            StartActivityForResult(intent, 100);
            Finish();
        }
    }
}