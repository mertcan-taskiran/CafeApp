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
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _he;
        private readonly IToastNotification _toast;

        public BlogController(ApplicationDbContext context, IWebHostEnvironment he, IToastNotification toast)
        {
            _context = context;
            _he = he;
            _toast = toast;
        }

        // GET: Admin/Blog
        public async Task<IActionResult> Index()
        {
              return _context.Blogs != null ? 
                          View(await _context.Blogs.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Blogs'  is null.");
        }

        // GET: Admin/Blog/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Admin/Blog/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Blog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    var fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(_he.WebRootPath, @"Site\blog");
                    var ext = Path.GetExtension(files[0].FileName);

                    if (blog.Image != null)
                    {
                        var imagePath = Path.Combine(_he.WebRootPath, blog.Image.TrimStart('\\'));

                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + ext), FileMode.Create))
                    {
                        files[0].CopyTo(filesStreams);
                    }

                    blog.Image = @"\Site\blog\" + fileName + ext;
                }

                _context.Add(blog);
                await _context.SaveChangesAsync();
                _toast.AddSuccessToastMessage("Added Successfully");
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: Admin/Blog/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // POST: Admin/Blog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Name,Email,Image,Onay,Mesaj,Tarih")] Blog blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.Id))
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
            return View(blog);
        }

        // GET: Admin/Blog/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Admin/Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Blogs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Blogs'  is null.");
            }
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
            }
            
            await _context.SaveChangesAsync();
            _toast.AddSuccessToastMessage("Successfully Deleted");
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
          return (_context.Blogs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
