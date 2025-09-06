using AutoMapper;
using ByteInoTaskManager.Models;
using ByteInoTaskManager.Models.DTOs;
using ByteInoTaskManager.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace ByteInoTaskManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
             _signinManager= signInManager;
              _userManager= userManager;
        }
        public IActionResult LoginRegister(string activeForm="login")
        {
            var model = new AuthViewModel
            {
                Login = new LoginDTO(),
                Register = new RegisterDTO(),
                ActiveForm = activeForm
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(LoginRegister), new AuthViewModel { Register = model, ActiveForm = "register" });
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };
            

            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded && user.IsActive != false) 
            {
                await _userManager.AddToRoleAsync(user, "User");
                await _signinManager.SignInAsync(user,isPersistent: false);
                return RedirectToAction("TaskList", "TaskItems");
            }


            foreach(var err in result.Errors)
            {
                ModelState.AddModelError(string.Empty, err.Description);
            }

            return View(nameof(LoginRegister),new AuthViewModel { Register=model,ActiveForm= "register" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(LoginRegister), new AuthViewModel { Login = model, ActiveForm = "login" });
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && !user.IsActive)
            {
                ModelState.AddModelError(string.Empty, "Incorrect email or password or account inactive.");
                return View(nameof(LoginRegister), new AuthViewModel { Login = model, ActiveForm = "login" });
            }

            var res = await _signinManager.PasswordSignInAsync(
                model.Username, model.Password, model.RememberMe, lockoutOnFailure: false
                );


            if (res.Succeeded ) 
            {
                return RedirectToAction("TaskList", "TaskItems");
            }

            ModelState.AddModelError(string.Empty, "Incorrect email or password.");
            return View(nameof(LoginRegister),new AuthViewModel { Login=model,ActiveForm= "login" });
        }


     
        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
