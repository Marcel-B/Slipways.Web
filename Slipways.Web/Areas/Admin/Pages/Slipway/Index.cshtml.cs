using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.b_velop.Slipways.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace com.b_velop.Slipways.Web.Areas.Admin.Pages.Slipways
{

    public class SlipwaysModel
    {
        public HashSet<Data.Models.Slipway> Slipways { get; set; }
    }

    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }

        private readonly IDataStore _dataStore;
        private readonly ILogger<IndexModel> _logger;

        public SlipwaysModel Slipways { get; set; }

        public IndexModel(
            IDataStore dataStore,
            ILogger<IndexModel> logger)
        {
            _dataStore = dataStore;
            _logger = logger;
        }
        public async Task OnGetDeleteAsync(
            Guid id)
        {
            var slipways = await _dataStore.RemoveSlipwayAsync(id);
            Slipways.Slipways = slipways;
        }

        public async Task OnGetAsync()
        {
            Slipways = new SlipwaysModel();
            var slipways = await _dataStore.GetSlipwaysAsync();
            Slipways.Slipways = slipways;
        }
    }
}