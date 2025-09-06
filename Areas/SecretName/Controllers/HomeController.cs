using ByteInoTaskManager.Areas.SecretName.Models.ViewModels;
using ByteInoTaskManager.Data;
using ByteInoTaskManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ByteInoTaskManager.Areas.SecretName.Controllers
{
    [Area("SecretName")]
    [Authorize(Roles ="Admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {

            var user = await _userManager.Users.Where(u => u.Email != "ad@gmail.com")
                .Select(u => new UserListViewModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    TaskCount = u.Tasks.Count(),
                    CategoryCount = u.Categories.Count(),
                })
                .ToListAsync();

            return View(user);
        }
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var categories = await _context.Categories.Where(u => u.UserId == id).ToListAsync();
            var tasks = await _context.TaskItems.Include(t => t.Category).Where(u => u.UserId == id).ToListAsync();

            var details = new UserDetailViewModel
            {
                Userd = user,
                Categories = categories,
                TaskItems = tasks
            };

            return View(details);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserDetailViewModel userDetail)
        {
            var user =await _userManager.FindByIdAsync(userDetail.Userd.Id);
            if (user == null) {
                return NotFound();
            }
            user.IsActive = userDetail.Userd.IsActive;
             _context.SaveChanges();
            return RedirectToAction(nameof(Edit),new { user.Id });
        }
      
        public async Task<IActionResult> Delete(string userId)
        {
            var user =await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }
            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));

        }
    }
}
