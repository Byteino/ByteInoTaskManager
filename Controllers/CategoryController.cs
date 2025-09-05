using AutoMapper;
using ByteInoTaskManager.Data;
using ByteInoTaskManager.Models;
using ByteInoTaskManager.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace ByteInoTaskManager.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public CategoryController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var categories = await _context.Categories
                .Where(c => c.UserId == user.Id)
                .ToListAsync();

            var categoryDto = _mapper.Map<List<CategoryDTO>>(categories);

            return View(categoryDto);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryDTO dto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var category = _mapper.Map<Category>(dto);
                category.UserId = user.Id;

                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var user = await _userManager.GetUserAsync(User);
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.UserId == user.Id);

            if (category == null) return NotFound();
            var dto = _mapper.Map<CategoryDTO>(category);
            return View(dto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryDTO dto)
        {
            if (id != dto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);

                    var category = await _context.Categories
                        .FirstOrDefaultAsync(c => c.Id == id && c.UserId == user.Id);

                    if (category == null) return NotFound();

                    _mapper.Map(dto, category);

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(dto.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var user =await _userManager.GetUserAsync(User);
            var category =await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == user.Id);

            if (category == null) return NotFound();

            var dto = _mapper.Map<CategoryDTO>(category);
            return View(dto);
        }
        [HttpPost]

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var category = await _context.Categories.FirstOrDefaultAsync(c=> c.Id== id && c.UserId == user.Id);

            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }





        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
