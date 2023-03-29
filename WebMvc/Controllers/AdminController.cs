using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.ViewModels;

namespace StudentManagement.Controllers
{
    public class AdminController : Controller
    {
        private RoleManager<IdentityRole> roleManager;

        public UserManager<ApplicationUser> UserManager { get; }

        public AdminController(RoleManager<IdentityRole> roleMnager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleMnager;
            this.UserManager = userManager;
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

        [HttpGet]
        public async Task<IActionResult> EditRoles(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"角色ID为{id}的信息不存在，请重试。";
                return View("NotFound");
            }

            var model = new EditRoleViewModel { Id = id, RoleName = role.Name };
            var users = UserManager.Users.ToList();
            foreach (var user in users)
            {
                // 如果用户拥有此角色，请将用户名添加到
                // EditRoleViewModel 模型中的Users属性中
                if(await UserManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            //然后江对象传递给试图显示到客户端
            return View(model);
        }

        public async Task<IActionResult> EditRoles(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"角色ID为{model.Id}的信息不存在，请重试。";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);

        }
    }
}
