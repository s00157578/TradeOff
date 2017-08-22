using System;
using Android.App;
using Android.OS;
using Android.Widget;
using TradeOffAndroidApp.Core;
using Android.Content;
using System.Collections.Generic;
using Android.Content.PM;
using Android.Provider;
using Java.IO;
using Android.Graphics;
using Android.Locations;
using Android.Runtime;
using System.Threading.Tasks;
using TradeOffAndroidApp.Core.Models;
using TradeOffAndroidApp.Core.Services;
using System.Linq;
using Android.Views;
using Plugin.SecureStorage;

namespace TradeOffAndroidApp
{
    [Activity(Label = "Sell Product")]   
    public class AddProductActivity : Activity, ILocationListener
    {
        private Button _btnImage;
        private ImageView _imageViewProduct;
        private Spinner _dropDownCategories;
        private EditText _editName;
        private EditText _editShortDescription;
        private EditText _editFullDescription;
        private EditText _editPrice;
        private TextView _textViewWarning;
        private Button _btnSubmit;
        private ImageConversion _imageConverter;
        private List<CategoryModel> _categoryList;
        private CategoryRepository _categoryRepository;
        private ProductRepository _productRepository;
        private ProductImageRepository _productImageRepository;
        private string coords;
        private int _categoryId;
        private Bitmap _productImage;
        LocationManager locMgr;
        private string _userId;
        private CredentialRepository _credentialRepository;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            _credentialRepository = new CredentialRepository();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddProductView);
            GetId();
            if (string.IsNullOrEmpty(_userId))
            {
                GoToLogin();
            }
            else
            {
                _categoryRepository = new CategoryRepository();
                _productImageRepository = new ProductImageRepository();
                _productRepository = new ProductRepository();
                locMgr = GetSystemService(Context.LocationService) as LocationManager;              
                //prevents a uri exception when creating a directory when taking an image
                StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
                builder.DetectFileUriExposure();
                StrictMode.SetVmPolicy(builder.Build());                
                GetViews();
                PopulateLists();
                ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.TextViewItem, _categoryList.Select(x => x.CategoryName).ToArray());
                _dropDownCategories.Adapter = adapter;
                HandleEvents();
            }                            
        }
        //gets logged in users id
        private async void GetId()
        {
           _userId = await _credentialRepository.GetUserId();
        }
        //on return from camera app read file from the specified path and set the imageview to the image, (based off xamarin tutorial)
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            _imageConverter = new ImageConversion();
            base.OnActivityResult(requestCode, resultCode, data);
 
            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Android.Net.Uri contentUri = Android.Net.Uri.FromFile(App._file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);
            string imagePath = App._file.Path;
            _productImage = _imageConverter.LoadBitmap(imagePath);
            if (_productImage != null)
            {
                _imageViewProduct.SetImageBitmap(_productImage);
                App.bitmap = null;
            }           
            GC.Collect();
        }
        //finds and binds the ui views
        private void GetViews()
        {
            _imageViewProduct = FindViewById<ImageView>(Resource.Id.imageViewProduct);
            _btnImage = FindViewById<Button>(Resource.Id.btnImage);
            _dropDownCategories = FindViewById<Spinner>(Resource.Id.dropdownCategoriese);
            _editName = FindViewById<EditText>(Resource.Id.txtEditName);
            _editShortDescription = FindViewById<EditText>(Resource.Id.txtEditShortDescription);
            _editFullDescription = FindViewById<EditText>(Resource.Id.txtEditFullDescription);
            _editPrice = FindViewById<EditText>(Resource.Id.txtEditPrice);
            _btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);
            _textViewWarning = FindViewById<TextView>(Resource.Id.txtViewWarning);
        }
        //handles any events,
        private void HandleEvents()
        {
            _btnImage.Click += BtnImage_Click;
            _btnSubmit.Click += BtnSubmit_Click;
            _dropDownCategories.ItemSelected += CategorySelected;
        }
        private async void PopulateLists()
        {
            IEnumerable<CategoryModel> categories = await _categoryRepository.GetCategoriesAsync();
            _categoryList = categories.ToList();
        }

        //on clicking the image button create a directory for the image, create a new intent that will open the camera app and store the image in the created file
        private void BtnImage_Click(object sender, EventArgs e)
        {
            if (IsThereImageApp())
            {
                CreateImageDirectory();
                Intent intent = new Intent(MediaStore.ActionImageCapture);
                App._file = new File(App._directory, String.Format("productImage{0}.jpg", Guid.NewGuid()));
                intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(App._file));
                StartActivityForResult(intent, 0);
            }         
        }
        private void CategorySelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var category =  _categoryList[e.Position];
            _categoryId = category.Id;
        }
        //checks if image app ie camera exists
        private bool IsThereImageApp()
        {

            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }
        //creates the directory to create the image
        private void CreateImageDirectory()
        {
            App._directory = new File(
                Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures), "ProductImage");
            if (!App._directory.Exists())
            {
                App._directory.Mkdirs();
            }
        }
        private async void BtnSubmit_Click(object sender, EventArgs e)
        {
            //reads inputs for basic validation
            bool isValid = ReadInputs();
            if(isValid)
            {
                int counter = 0;
                bool isLocationFound = true;
                //while the location has not being found implement a delay for a period of time
                while(string.IsNullOrEmpty(coords))
                {
                    await Task.Delay(200);
                    counter++;
                    //if location can not be found return an error
                    if(counter >=10 &&  string.IsNullOrEmpty(coords))
                    {
                        isLocationFound = false;
                        _textViewWarning.Text = "location can not be found at his time, try again later";
                    }
                }
                //pauses location services
                OnPause();
                if(isLocationFound)
                {
                    //creates product
                    ProductCreateModel product = new ProductCreateModel()
                    {
                        Name = _editName.Text,
                        Location = coords,
                        ShortDescription = _editShortDescription.Text,
                        FullDescription = _editFullDescription.Text,
                        Price = decimal.Parse(_editPrice.Text),
                        UserId = _userId,
                    };
                    ProductModel newProduct = await _productRepository.AddProduct(_categoryId, product);
                    if(newProduct !=null)
                    {
                        byte[] imageAsByte = _imageConverter.BitmapToByteArray(_productImage);
                        ProductImageCreateModel productImage = new ProductImageCreateModel()
                        {
                            ProductId = newProduct.Id,
                            Image = imageAsByte,
                            IsMainImage = true
                        };
                        ProductImageModel newImage = await _productImageRepository.AddProductImage(newProduct.Id, productImage);
                        if (newImage != null)
                            GoToProductView(newProduct.Id);
                    }                   
                }               
            }            
        }
        private void GoToProductView(int productId)
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(ProductViewActivity));
            intent.PutExtra("selectedProductId", productId);
            StartActivityForResult(intent, 100);
        }
        //basic validation
        private bool ReadInputs()
        {
            if(_productImage == null)
            {
                _textViewWarning.Text = "An Image is Required";
                return false;
            }
           if(!IsInputValid(_editName))
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
            if(_categoryId ==0)
            {
                _textViewWarning.Text = "Category is Required";
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
        //looks for the best matching location provider based on the criterea, ie gps location celluar or wifi
        protected override void OnResume()
        {
            base.OnResume();
            Criteria locationCriteria = new Criteria();

            locationCriteria.Accuracy = Accuracy.Fine;
            locationCriteria.PowerRequirement = Power.NoRequirement;

            string locationProvider = locMgr.GetBestProvider(locationCriteria, true);
            //basic validation that there is a location provider
            if (locationProvider != null)
            {
                locMgr.RequestLocationUpdates(locationProvider, 2000, 1, this);
            }
            else
            {
                _textViewWarning.Text =  "Location is not available. Does the device have location services enabled?";
            }
        }
        //on location changed (found) sets the coordinates
        public void OnLocationChanged(Location location)
        {
            string latitude = location.Latitude.ToString();
            string longitude = location.Longitude.ToString();
            coords = $"{latitude},{longitude}";
        }
        //stops location updates
        protected override void OnPause()
        {
            base.OnPause();
            locMgr.RemoveUpdates(this);
        }
        //methods required by the interface

        public void OnProviderDisabled(string provider)
        {
            
        }

        public void OnProviderEnabled(string provider)
        {
            
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            
        }
        private void GoToLogin()
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(LoginActivity));
            StartActivity(intent);
            Finish();
        }
    }
    //class for the image location
    public static class App
    {
        public static File _file;
        public static File _directory;
        public static Bitmap bitmap;
    }
} 