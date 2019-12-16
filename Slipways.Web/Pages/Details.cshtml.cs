using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.b_velop.Slipways.Web.Data;
using com.b_velop.Slipways.Web.Data.Models;
using com.b_velop.Slipways.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace com.b_velop.Slipways.Web.Pages
{
    public class DetailsModel : PageModel
    {
        private IDataStore _dataStore;
        private ISecretProvider _secret;
        private ILogger<DetailsModel> _logger;

        [BindProperty]
        public Slipway Slipway { get; set; }

        [BindProperty]
        public HashSet<Extra> Extras { get; set; }

        [BindProperty]
        public string ApiKey { get; set; }

        public DetailsModel(
            IDataStore dataStore,
            ISecretProvider secret,
            ILogger<DetailsModel> logger)
        {
            _dataStore = dataStore;
            _secret = secret;
            _logger = logger;
        }
        public async Task OnGetAsync(
            Guid id)
        {
            ApiKey = _secret.GetSecret("google_maps_secret");
            var slipways = await _dataStore.GetSlipwaysAsync();
            Slipway = slipways.FirstOrDefault(_ => _.Id == id);
        }
    }
}