using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Semestrovka.Data;
using Semestrovka.Models.DBModels;

namespace Semestrovka.Controllers
{
    public class DataBaseController : Controller
    {
        // GET
        private static d6h4jeg5tcb9d8Context _context;

        public DataBaseController(d6h4jeg5tcb9d8Context context)
        {
            _context = context;
        }

        //если строка null, значит в форме name не указан
        public IActionResult SearchResults(string searchString)
        {
            var result = _context.Product.Where(item => item.Name.ToLower().Contains(searchString.ToLower()));
            return View(result.ToList());
        }

        public IActionResult AdminPanel()
        {
            var products = _context.Product.ToList();
            var mages = _context.Images.ToList();
            return View(products); //получаем объекты из бд
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            _context.Product.Add(product); //sql выражение insert
            await _context.SaveChangesAsync(); //выполняет выражение
            return RedirectToAction("AdminPanel");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == id);
                if (product != null)
                    return View(product);
            }

            return NotFound();
        }

        //возвращает форму с сданными объекта, которые можно заредачить
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == id);
                if (product != null)
                    return View(product);
            }

            return NotFound();
        }

        //Получает отредактированные данные в виде объекта 
        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            _context.Product.Update(product); //sql выражение insert
            await _context.SaveChangesAsync(); //выполняет выражение
            return RedirectToAction("AdminPanel");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var product = new Product
                    {Id = id.Value};
                _context.Entry(product).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return RedirectToAction("AdminPanel");
            }

            return NotFound();
        }
    }
}