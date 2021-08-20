using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetNote.Apis.Controllers
{
    public class WebApiFileUploadDemoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
