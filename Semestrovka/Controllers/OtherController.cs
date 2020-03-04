using Microsoft.AspNetCore.Mvc;
using Semestrovka.Data;
using System.Linq;

namespace Semestrovka.Controllers
{
    public class OtherController : Controller
    {
        private static d6h4jeg5tcb9d8Context _context;

        public OtherController(d6h4jeg5tcb9d8Context context)
        {
            _context = context;

        }
        public IActionResult Shop()
        {
            var allItems = _context.Product.ToList();
            var images = _context.Images.ToArray();
            return View(allItems);
        }
        public IActionResult SingleProduct()
        {
            return View();
        }
    }
}