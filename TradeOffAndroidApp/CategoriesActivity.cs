
using Android.App;
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
        private List<CategoryModel> _categories;
        private ICategoryRepository _categoryRepository;
        //public CategoriesActivity(ICategoryRepository categoryRepository)
        //{
        //    _categoryRepository = categoryRepository;
        //}
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Categories);
            PopulateList();
            FindViews();
            _categoryListView.Adapter = new CategoryListAdapter(this, _categories);
            _categoryListView.FastScrollEnabled = true;
        }
        private async void PopulateList()
        {
            IEnumerable<CategoryModel> category = await _categoryRepository.GetCategoriesAsync();
            _categories = category.ToList();
        }
        private void FindViews()
        {
            _categoryListView = FindViewById<ListView>(Resource.Id.ListviewCategories);
        }
    }
}