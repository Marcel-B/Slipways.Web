using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.b_velop.Slipways.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace com.b_velop.Slipways.Web.Areas.Admin.Pages.Service
{
    public class IndexModel : PageModel
    {
        private IDataStore _dataStore;
        [BindProperty]
        public HashSet<Data.Models.Service> Services { get; set; }
        public IndexModel(
            IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task OnGetAsync()
        {
            Services = await _dataStore.GetServicesAsync();
        }
    }
}
