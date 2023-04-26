using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CafeApp.Data;
using CafeApp.Models;
using NToastNotify;
using Microsoft.AspNetCore.Authorization;

namespace CafeApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AboutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toast;

        public AboutController(ApplicationDbContext context, IToastNotification toast)
        {
            _context = context;
            _toast = toast;
        }

        // GET: Admin/About
        public async Task<IActionResult> Index()
        {
              return _context.Abouts != null ? 
                          View(await _context.Abouts.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Abouts'  is null.");
        }

        // GET: Admin/About/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Abouts == null)
            {
                return NotFound();
            }

            var about = await _context.Abouts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (about == null)
            {
                return NotFound();
            }

            return View(about);
        }

        // GET: Admin/About/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/About/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] About about)
        {
            if (ModelState.IsValid)
            {
                _context.Add(about);
                await _context.SaveChangesAsync();
                _toast.AddSuccessToastMessage("Added Successfully");
                return RedirectToAction(nameof(Index));
            }
            return View(about);
        }

        // GET: Admin/About/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Abouts == null)
            {
                return NotFound();
            }

            var about = await _context.Abouts.FindAsync(id);
            if (about == null)
            {
                return NotFound();
            }
            return View(about);
        }

        // POST: Admin/About/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] About about)
        {
            if (id != about.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(about);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutExists(about.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _toast.AddSuccessToastMessage("Successfully Updated");
                return RedirectToAction(nameof(Index));
            }
            return View(about);
        }

        // GET: Admin/About/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Abouts == null)
            {
                return NotFound();
            }

            var about = await _context.Abouts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (about == null)
            {
                return NotFound();
            }

            return View(about);
        }

        // POST: Admin/About/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Abouts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Abouts'  is null.");
            }
            var about = await _context.Abouts.FindAsync(id);
            if (about != null)
            {
                _context.Abouts.Remove(about);
            }
            
            await _context.SaveChangesAsync();
            _toast.AddSuccessToastMessage("Successfully Deleted");
            return RedirectToAction(nameof(Index));
        }

        private bool AboutExists(int id)
        {
          return (_context.Abouts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
