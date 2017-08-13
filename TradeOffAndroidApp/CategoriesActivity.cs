
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;
using System.Linq;
using TradeOffAndroidApp.Adapters;
using TradeOffAndroidApp.Core.Models;
using TradeOffAndroidApp.Core.Services;

namespace TradeOffAndroidApp
{
    [Activity(Label = "CategoriesActivity")]
    public class CategoriesActivity : Activity
    {
        private ListView _categoryListView;
        private static List<CategoryModel> _categoriesList;
        private CategoryRepository _categoryRepository;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            _categoryRepository = new CategoryRepository();
            base.OnCreate(savedInstanceState);
            PopulateList();
            SetContentView(Resource.Layout.Categories);
            FindViews();
            _categoryListView.Adapter = new CategoryListAdapter(this, _categoriesList);
            _categoryListView.FastScrollEnabled = true;
            HandleEvents();
        }
        private void HandleEvents()
        {
            _categoryListView.ItemClick += CategoryListView_ItemClick;
        }
        private async void PopulateList()
        {
            if(_categoriesList == null)
            {
                IEnumerable<CategoryModel> category = await _categoryRepository.GetCategoriesAsync();
                _categoriesList = category.ToList();
            }          
        }
        private void FindViews()
        {
            _categoryListView = FindViewById<ListView>(Resource.Id.ListviewCategories);
        }
        private void CategoryListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var category = _categoriesList[e.Position];
            var intent = new Intent();
            intent.SetClass(this, typeof(ProductListActivity));
            intent.PutExtra("selectedCategoryId", category.Id);
            StartActivityForResult(intent, 100);
        }
    }
}