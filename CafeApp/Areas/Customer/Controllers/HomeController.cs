﻿using CafeApp.Data;
using CafeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CafeApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var menu = _db.Menus.Where(i => i.Ozel).ToList();
            return View(menu);
        }

		public IActionResult CategoryDetails(int? id)
		{
            var menu = _db.Menus.Where(i => i.CategoryId == id).ToList();
            ViewBag.KategoriId = id;
			return View(menu);
		}

		public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Galeri()
        {
            return View();
        }

        // GET: Admin/Rezervasyon/Create
        public IActionResult Rezervasyon()
        {
            return View();
        }

        // POST: Admin/Rezervasyon/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rezervasyon([Bind("Id,Name,Email,TelefonNo,Sayi,Saat,Tarih")] Rezervasyon rezervasyon)
        {
            if (ModelState.IsValid)
            {
                _db.Add(rezervasyon);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rezervasyon);
        }

        public IActionResult Menu()
        {
            var menu = _db.Menus.ToList();
            return View(menu);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}