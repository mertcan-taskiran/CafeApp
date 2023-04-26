using CafeApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace CafeApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toast;
        public UserController(ApplicationDbContext context, IToastNotification toast)
        {
            _context = context;
            _toast = toast;
        }

        public IActionResult Index()
        {
            var users = _context.ApplicationUsers.ToList();
            var role = _context.Roles.ToList();
            var userRol = _context.UserRoles.ToList();
            foreach (var item in users)
            {
                var roleId = userRol.FirstOrDefault(i => i.UserId == item.Id).RoleId;
                item.Role = role.FirstOrDefault(u => u.Id == roleId).Name;
            }
            return View(users);
        }

        // GET: Admin/Category/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.ApplicationUsers == null)
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(m => m.Id == id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.ApplicationUsers.FindAsync(id);
            if (user != null)
            {
                _context.ApplicationUsers.Remove(user);
            }

            await _context.SaveChangesAsync();
            _toast.AddSuccessToastMessage("Successfully Deleted");
            return RedirectToAction(nameof(Index));
        }

    }
}
