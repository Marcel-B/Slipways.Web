using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.b_velop.Slipways.Web.Data;
using com.b_velop.Slipways.Web.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace com.b_velop.Slipways.Web.Pages
{
    public class DetailsModel : PageModel
    {
        private IDataStore _dataStore;
        private ILogger<DetailsModel> _logger;

        [BindProperty]
        public Slipway Slipway { get; set; }

        [BindProperty]
        public HashSet<Extra> Extras { get; set; }

        public DetailsModel(
            IDataStore dataStore,
            ILogger<DetailsModel> logger)
        {
            _dataStore = dataStore;
            _logger = logger;
        }
        public async Task OnGetAsync(
            Guid id)
        {
            var slipways = await _dataStore.GetSlipwaysAsync();
            Slipway = slipways.FirstOrDefault(_ => _.Id == id);
        }
    }
}
