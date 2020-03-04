using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Semestrovka.Data;
using Semestrovka.Data.Logic;
using Semestrovka.Models.DBModels;

namespace Semestrovka.Controllers
{
    public class UsersController : Controller
    {
        private readonly d6h4jeg5tcb9d8Context _context;

        public UsersController(d6h4jeg5tcb9d8Context context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return PartialView();
        }

        // GET: Users
        public IActionResult Index()
        {
            try
            {
                var users = _context.Users.ToList(); //null exp
                return View(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: Users/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            var users = _context.Users.Find(id);
            if (users == null) return NotFound();

            return View(users);
        }

        public IActionResult MyProfile()
        {
            var token = HttpContext.Request.Cookies["Token"];
            if (token == null) return RedirectToAction("Login");
            var user = _context.Users.Where(x=>x.Id != 41).FirstOrDefault(x => x.Token == token);
            if (user == null) return RedirectToAction("Login");;
            return View(user);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var cities = _context.Cities.ToList();
            ViewBag.Cities = cities.Select(x => x.Name);
            return View();
        }

        // GET: Users/Create
        [HttpPost]
        public IActionResult Create(Users user)
        {
            try
            {
                if (user == null) return BadRequest("user is null");
                user.City = 35;
                user.Address = "";
                user.Token = Hash.MakeHash(user.Login);
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(Users user)
        {
            try
            {
                if (user == null) return NotFound();
                user.City = 35;
                _context.Users.Update(user);
                _context.SaveChanges();
                var a = HttpContext.Request.Cookies["Token"];
                return RedirectToAction("MyProfile");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public IActionResult LogOut()
        {
            ViewBag.Auth = false;
            HttpContext.Response.Cookies.Delete("Token");
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Auth(string log, string pass)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(user => user.Login == log);

                if (user == null) return NotFound("User not found");

                if (user.Pass != pass) return BadRequest("Incorrect pass");

                HttpContext.Response.Cookies.Append("Token", user.Token);
                HttpContext.Response.Cookies.Append("UserId", user.Id.ToString());
                ViewBag.Auth = true;
                return RedirectToAction("MyProfile");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: Users/Delete/5
        [HttpGet("users/delete/{id}")]
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();
            _context.Users.Remove(user);
            _context.SaveChanges();

            return View();
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}