using com.b_velop.Slipways.Web.Data.Models;
using com.b_velop.Slipways.Web.Infrastructure;
using com.b_velop.Slipways.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ISecretProvider _secretProvider;
        private readonly ISlipwayService _slipwayService;
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public IEnumerable<Slipway> Slipways { get; set; }

        public IndexModel(
            ISecretProvider secretProvider,
            ISlipwayService slipwayService,
            ILogger<IndexModel> logger)
        {
            _secretProvider = secretProvider;
            _slipwayService = slipwayService;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            Slipways = await _slipwayService.GetSlipwaysAsync();
        }

        public void OnPost()
        {
            //var appId = _secretProvider.GetSecret("facebook_app_id");
            //var redirect = "https://slipways.de/facebook-signin";
            //var clientId = appId;
            //var state = Guid.NewGuid().ToString();
            //var path = $"https://www.facebook.com/v5.0/dialog/oauth?client_id={clientId}&redirect_uri={redirect}&state={state}";
            //return new RedirectResult(path);
        }
    }
}
