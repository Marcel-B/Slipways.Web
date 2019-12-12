using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.b_velop.Slipways.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace com.b_velop.Slipways.Web.Areas.Admin.Pages.Slipway
{
    public class DetailsModel : PageModel
    {
        private IDataStore _dataStore;

        [BindProperty]
        public Data.Models.Slipway Slipway { get; set; }

        public DetailsModel(
            IDataStore dataStore)
        {
            _dataStore = dataStore;
        }
        public async Task<IActionResult> OnGet(
            Guid id)
        {
            var slipways = await _dataStore.GetSlipwaysAsync();
            Slipway = slipways.FirstOrDefault(_ => _.Id == id);
            return Page();
        }
    }
}
