using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.b_velop.Slipways.Web.Data.Models;
using com.b_velop.Slipways.Web.Services;
using GraphQL.Common.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace com.b_velop.Slipways.Web.Pages.Slipways
{
    public class CreateModel : PageModel
    {
        private ISlipwayService _service;
        private ILogger<CreateModel> _logger;

        [BindProperty]
        public Slipway Slipway { get; set; }

        //[BindProperty]
        //public IEnumerable<Water> Waters { get; set; }

        public SelectList Waters { get; set; }

        public CreateModel(
            ISlipwayService service,
            ILogger<CreateModel> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            Slipway = new Slipway();
            var result = await _service.GetWatersAsync();
            Waters = new SelectList(result, "Id", "Longname");
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _service.InsertSlipway(Slipway);
                if (result)
                {
                    return RedirectToPage("../Index");
                }
            }
            return Page();
        }
    }
}