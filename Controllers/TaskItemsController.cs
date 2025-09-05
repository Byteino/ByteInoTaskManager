using AutoMapper;
using ByteInoTaskManager.Data;
using ByteInoTaskManager.Models;
using ByteInoTaskManager.Models.DTOs;
using ByteInoTaskManager.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ByteInoTaskManager.Controllers
{
    [Authorize]
    public class TaskItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskItemsController(
            ApplicationDbContext context,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        private void PopulateCategories()
        {
            var userId = _userManager.GetUserId(User);
            var categories = _context.Categories
                .Where(c => c.UserId == userId)
                .Select(c => new CategoryDTO { Id = c.Id, Name = c.Name })
                .ToList();

            ViewBag.Categories = categories;
        }

        public async Task<IActionResult> TaskList(string search, int? categoryId, int page = 1, int pageSize = 10)
        {
            var userId = _userManager.GetUserId(User);

            var query = _context.TaskItems
                .Include(t => t.Category)
                .Where(t => t.UserId == userId);

            
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t => t.Title.Contains(search) || t.Description.Contains(search));
            }

           
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(t => t.CategoryId == categoryId.Value);
            }

            var tasks = await query
                .OrderByDescending(t => t.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtoList = _mapper.Map<IEnumerable<TaskItemDTO>>(tasks);

            PopulateCategories();
            ViewBag.SelectedCategoryId = categoryId;

            return View(dtoList);
        }


        public IActionResult Add()
        {
            PopulateCategories();
            return  PartialView("_AddTaskModal", new TaskItemDTO() { Title = "" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TaskItemDTO model)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            model.UserId = userId;
            model.UserName = user?.UserName;

            if(model.CategoryId.HasValue && model.CategoryId.Value > 0)
            {
                var category = await _context.Categories.FindAsync(model.CategoryId.Value);
                model.CategoryName = category?.Name;
            }

            if (!ModelState.IsValid)
            {
                PopulateCategories();
                return PartialView("_AddTaskModal", model);
            }


            var entity = _mapper.Map<TaskItem>(model);
            entity.UserId = userId;
            entity.Date = DateTime.Now;

            if (model.CategoryId == 0)
                entity.CategoryId = null;

            _context.TaskItems.Add(entity);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);

            var task = await _context.TaskItems
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null)
                return NotFound();

            var dto = _mapper.Map<TaskItemDTO>(task);
            PopulateCategories();
            return PartialView("_EditTaskModal", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TaskItemDTO model)
        {
            if (!ModelState.IsValid)
            {
                PopulateCategories();
                return PartialView("_EditTaskModal", model);
            }

            var userId = _userManager.GetUserId(User);

            var task = await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == model.Id && t.UserId == userId);

            if (task == null)
                return NotFound();

            _mapper.Map(model, task);

            if (model.CategoryId == 0)
                task.CategoryId = null;

            task.Status = model.Status;
            task.IsCompleted=(task.Status == Models.TaskStatus.Finished);
            task.Date = DateTime.Now;

            _context.TaskItems.Update(task);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);

            var task = await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null)
                return NotFound();

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finish(int id)
        {
            var userId = _userManager.GetUserId(User);

            var task = await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null)
                return NotFound();

            task.Status = Models.TaskStatus.Finished;
            task.IsCompleted = true;

            _context.TaskItems.Update(task);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
    }


}
