using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.ViewModels;

namespace StudentManagement.Controllers
{
    public class AdminController : Controller
    {
        private RoleManager<IdentityRole> roleManager;

        public AdminController(RoleManager<IdentityRole> roleMnager)
        {
            this.roleManager = roleMnager;
        }

        // Get  Post
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoleAsync(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityROle = new IdentityRole
                {
                    Name = model.RoleName
                };
                // 如果您尝试创建具有已经存在的同明明的角色，则会收到验证错误
               IdentityResult result = await roleManager.CreateAsync(identityROle);

                if(result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Admin");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }
    }
}
