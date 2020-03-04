using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Semestrovka.Data;
using Semestrovka.Models.DBModels;

namespace Semestrovka.Controllers
{
    public class CartController : Controller
    {
        private readonly d6h4jeg5tcb9d8Context _context;

        public CartController(d6h4jeg5tcb9d8Context context)
        {
            _context = context;
        }

        // GET: Cart
        public async Task<IActionResult> Cart()
        {
            var cartId = JsonSerializer.Deserialize<List<int>>(HttpContext.Request.Cookies["Cart"]);
            var cart = new List<Product>();

            foreach (var productId in cartId)
            {
                var product = _context.Product
                    .Include(o => o.CategoryNavigation)
                    .Include(o => o.MainpictureurlNavigation)
                    .Include(o => o.Productimages)
                    .Include(o => o.Productinorder)
                    .FirstOrDefault(product => product.Id == productId);
                cart.Add(product);
            }

            var id = 1;
            if (cart.Count != 0)
                id = cart.FirstOrDefault().Id;
            var pr = _context.Product.ToList();
            var allProductOrders = _context.Productinorder.ToList();
            var productInOrder = _context.Productinorder.Where(op => op.Productid == id).ToList();
            var ordersWithRequestedProduct = _context.Orders
                .Where(order => productInOrder.Select(op => op.Orderid).Contains(order.Id)).ToList();
            var orderProductsWhichBoughtWithRequest = ordersWithRequestedProduct.Select(x => x.Productinorder)
                .SelectMany(x => x).Where(x => x.Productid != id).ToList();
            var productIds = orderProductsWhichBoughtWithRequest.Select(op => op.Productid).ToList();
            var dict = new Dictionary<int, int>();

            foreach (int productId in productIds)
            {
                if (!dict.ContainsKey((int) productId))
                    dict.Add(productId, 1);
                else
                    dict[productId]++;
            }

            var requestedIds = dict.OrderByDescending(pair => pair.Value).Select(pair => pair.Key).Take(2).ToList();
            var relatedProducts = new List<Product>();
            foreach (var reqId in requestedIds)
                relatedProducts.Add(_context.Product.Find(reqId));
            var images = _context.Images.ToList();
            var images2 = _context.Productimages.ToList();
            ViewBag.RelatedProducts = relatedProducts;
            return View(cart);
        }

        public IActionResult Checkout()
        {
            return View();
        }

        // GET: Cart/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var orders = await _context.Orders
                .Include(o => o.DeliveryNavigation)
                .Include(o => o.OwnerNavigation)
                .Include(o => o.StatusNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (orders == null) return NotFound();

            return View(orders);
        }

        // GET: Cart/Create
        public IActionResult Create()
        {
            ViewData["Delivery"] = new SelectList(_context.Deliveries, "Id", "Id");
            ViewData["Owner"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["Status"] = new SelectList(_context.Statuses, "Id", "Id");
            return View();
        }

        // POST: Cart/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Owner,Status,Datecreated,Delivery,Address,PayType,PhoneNumber,Email")]
            Orders orders)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orders);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Cart));
            }

            ViewData["Delivery"] = new SelectList(_context.Deliveries, "Id", "Id", orders.Delivery);
            ViewData["Owner"] = new SelectList(_context.Users, "Id", "Id", orders.Owner);
            ViewData["Status"] = new SelectList(_context.Statuses, "Id", "Id", orders.Status);
            return View(orders);
        }

        // GET: Cart/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var orders = await _context.Orders.FindAsync(id);

            if (orders == null) return NotFound();

            ViewData["Delivery"] = new SelectList(_context.Deliveries, "Id", "Id", orders.Delivery);
            ViewData["Owner"] = new SelectList(_context.Users, "Id", "Id", orders.Owner);
            ViewData["Status"] = new SelectList(_context.Statuses, "Id", "Id", orders.Status);
            return View(orders);
        }

        // POST: Cart/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Owner,Status,Datecreated,Delivery,Address,PayType,PhoneNumber,Email")]
            Orders orders)
        {
            if (id != orders.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdersExists(orders.Id)) return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Cart));
            }

            ViewData["Delivery"] = new SelectList(_context.Deliveries, "Id", "Id", orders.Delivery);
            ViewData["Owner"] = new SelectList(_context.Users, "Id", "Id", orders.Owner);
            ViewData["Status"] = new SelectList(_context.Statuses, "Id", "Id", orders.Status);
            return View(orders);
        }

        // GET: Cart/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var orders = await _context.Orders
                .Include(o => o.DeliveryNavigation)
                .Include(o => o.OwnerNavigation)
                .Include(o => o.StatusNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (orders == null) return NotFound();
            await DeleteConfirmed(Convert.ToInt32(id));
            return RedirectToAction("Cart");
        }

        // POST: Cart/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orders = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(orders);
            await _context.SaveChangesAsync();
            return RedirectToAction("Cart");
        }

        private bool OrdersExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}