using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatAPI.Controllers
{
    public class DocsController : Controller
    {
        // GET: Docs
        public ActionResult Index()
        {
            return View();
        }
    }
}