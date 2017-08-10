using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeOffAndroidApp.Core.Models;

namespace TradeOffAndroidApp.Core.Services
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryModel>> GetCategoriesAsync();
    }
}
