using Microsoft.AspNetCore.Mvc;
using StudentManagement.ViewModels;
using System.Diagnostics;
using WebMvc.Models;
using WebMvc.ViewModels;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Authorization;

namespace WebMvc.Controllers
{
    //[Route("[controler]/[action]")]
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IStudentRepository _studentRepository;
        private readonly IWebHostEnvironment webHostingEnvironment;

        public HomeController(ILogger<HomeController> logger, IStudentRepository studentRepository, IWebHostEnvironment webHostingEnvironment)
        {
            _logger = logger;
            _studentRepository = studentRepository;
            this.webHostingEnvironment = webHostingEnvironment;
            
        }

        //[Route("")]
        //[Route("~/")]
        //[Route("~/Home")]
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Student> students = _studentRepository.GetAllStudents();
            return View(students);
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            //throw new Exception("此异常发生在Detail视图中");

            _logger.LogTrace("Trace(跟踪) Log");
            _logger.LogDebug("Debug(调试) Log");
            _logger.LogInformation("信息(Information）Log");
            _logger.LogWarning("警告(Warning) Log");
            _logger.LogError("错误(Error) Log");
            _logger.LogCritical("严重(Critical) Log");

            Student student = _studentRepository.GetStudent(id);

            if (student == null)
            {
                Response.StatusCode = 404;
                return View("StudentNotFound", id);
            }

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Student = student,
                PageTitle = "学生详细信息"
            };
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(StudentCreateViewModel model)
        {
            //var builders = WebApplication.CreateBuilder();
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if(model.Photos!= null && model.Photos.Count>0)
                {
                    foreach (var photo in model.Photos)
                    {
                        string uploadsFolder = Path.Combine(webHostingEnvironment.WebRootPath, "images");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        photo.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }
                   

                Student newStudent = new Student
                {
                    Name = model.Name,
                    Email = model.Email,
                    ClassName = model.ClassName,
                    PhotoPath = uniqueFileName
                };

                _studentRepository.Add(newStudent);
                //Student newStudent = _studentRepository.Add(student);

                return RedirectToAction("Details", new { id = newStudent.Id });
            }
            return View(model);
            
        }

        //1、视图
        //视图模型
        //
        [HttpGet]
        public ViewResult Edit(int id)
        {
            Student student = _studentRepository.GetStudent(id);

     
                StudentEditViewModel studentEditView = new StudentEditViewModel
                {
                    Id = student.Id,
                    Name = student.Name,
                    Email = student.Email,
                    ClassName = student.ClassName,
                    ExistingPhotoPath = student.PhotoPath
                };

                return View(studentEditView);


            
        }

        public IActionResult Edit(StudentEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                Student student = _studentRepository.GetStudent(model.Id);
                student.Email = model.Email;
                student.Name = model.Name;
                student.ClassName   = model.ClassName;

                if(model.Photos.Count>0)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(webHostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    student.PhotoPath = ProcessUploadFile(model);
                }

                Student updateStudent = _studentRepository.Update(student);

                return RedirectToAction("Index");
            }

            return View(model);
        }
        /// <summary>
        /// 将照片保存到指定的路径中，并返回唯一文件名
        /// </summary>
        /// <returns></returns>
        private string ProcessUploadFile(StudentCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photos.Count > 0)
            {
                foreach (var photo in model.Photos)
                {
                    //必须将图像传到wwwroot中的images文件夹
                    //而要获取wwwroot文件夹的路径，我们需要注入ASP.NET Core提供的WebHostingEnvironment
                    //通过HostingEnvironment服务去获取wwwroot文件夹路径
                    string uploadsFolder = Path.Combine(webHostingEnvironment.WebRootPath, "images");
                    //为了确保文件名是唯一的，我们在文件名后附加一个新的GUID值和下划线
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    //因为使用了非托管资源，所以需要手动进行释放
                    using (var fileStream = new FileStream(filePath,FileMode.Create))
                    {
                        photo.CopyTo(fileStream);
                    }

                   
                }
            }
               
            return uniqueFileName;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}