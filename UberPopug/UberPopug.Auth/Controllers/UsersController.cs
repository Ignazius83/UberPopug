using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using UberPopug.Auth.Models;
using UberPopug.Auth.ViewModels;

namespace UberPopug.Auth.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
        {
            UserManager<User> _userManager;            
        public UsersController(UserManager<User> userManager)
            {
               
                _userManager = userManager;
            }

            public IActionResult Index() => View(_userManager.Users.ToList());

            public IActionResult Create() => View();

           [HttpPost]
            public async Task<IActionResult> Create(CreateUserViewModel model)
            {
                 if (!ModelState.IsValid)                
                     return View(model);        

                 var user = new User();
                 user.UserName = model.UserName;
                 user.public_id = Guid.NewGuid().ToString("D");

                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                   foreach (var err in result.Errors)
                   {
                     ModelState.AddModelError(string.Empty, err.Description);
                   }
                return View(model);
                } 
           
            return RedirectToAction("Index");
            }

            public async Task<IActionResult> Edit(string id)
            {
                User user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email, UserName = user.UserName };
                return View(model);
            }

           [HttpPost]
            public async Task<IActionResult> Edit(EditUserViewModel model)
            {
                if (!ModelState.IsValid)
                  return View(model);

                User user = await _userManager.FindByIdAsync(model.Id);
                if (user == null) throw new Exception("Error edit user");

                user.Email = model.Email;
                var result = await _userManager.UpdateAsync(user);

                 if (!result.Succeeded)
                 {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, err.Description);
                    }
                    return View(model);
                 }
                return RedirectToAction("Index");
            }

            [HttpPost]
            public async Task<ActionResult> Delete(string id)
            {
                User user = await _userManager.FindByIdAsync(id);
                if (user == null) throw new Exception("Error delete user");

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                      throw new Exception("Error delete user");
                return RedirectToAction("Index");
            }
        }
    
}
