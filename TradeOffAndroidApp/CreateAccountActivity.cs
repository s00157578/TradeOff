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
using TradeOffAndroidApp.Core.Models;
using Plugin.SecureStorage;

namespace TradeOffAndroidApp
{
    [Activity(Label = "CreateAccountActivity")]
    public class CreateAccountActivity : Activity
    {
        private TextView _textViewWarning;
        private EditText _textPassword;
        private EditText _textEmail;
        private Button _btnCreate;
        private Button _btnBack;
        private CredentialRepository _credentialRepository;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            _credentialRepository = new CredentialRepository();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CreateAccount);
            GetViews();
            HandleEvents();

            // Create your application here
        }
        private void GetViews()
        {
            _textViewWarning = FindViewById<TextView>(Resource.Id.txtViewWarning);
            _textPassword = FindViewById<EditText>(Resource.Id.txtEditPassword);
            _textEmail = FindViewById<EditText>(Resource.Id.txtEditEmail);
            _btnCreate = FindViewById<Button>(Resource.Id.btnCreate);
            _btnBack = FindViewById<Button>(Resource.Id.btnBack);
        }
        private void HandleEvents()
        {
            _btnBack.Click += _btnBack_Click;
            _btnCreate.Click += _btnCreate_Click;
        }

        private void _btnCreate_Click(object sender, EventArgs e)
        {
            bool isValid = IsInputValid();
            if (isValid)
            {
                CredentialModel account = new CredentialModel()
                {
                    Email = _textEmail.Text,
                    Password = _textPassword.Text
                };
                _credentialRepository.CreateAccountAsync(account);
                if (CrossSecureStorage.Current.HasKey("idToken"))
                {
                    GoToHome();
                    Finish();
                }
                else
                {
                    _textViewWarning.Text = "Something went wrong, try again later";
                }
            }
        }

        private void _btnBack_Click(object sender, EventArgs e)
        {
            Finish();
        }
        private bool IsInputValid()
        {
            if (string.IsNullOrEmpty(_textEmail.Text))
            {
                _textViewWarning.Text = "email is required";
                return false;
            }
            if (string.IsNullOrEmpty(_textPassword.Text))
            {
                _textViewWarning.Text = "password is required";
                return false;
            }               
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