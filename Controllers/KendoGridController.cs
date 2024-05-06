using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using KENDO_PRACTICE.Models;
using KENDO_PRACTICE.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KENDO_PRACTICE.Controllers
{
    // [Route("[controller]")]
    public class KendoGridController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IEmpRepository _repo;
        public KendoGridController( IEmpRepository repo,IWebHostEnvironment webHostEnvironment)
        {
            _repo=repo;
            _environment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Produces("application/json")]
        [HttpGet]
        public IActionResult GetAll()
        {
            // if(HttpContext.Session.GetString("username")=="")
            var cities = _repo.GetAll();
            return Json(cities);
        }
        [HttpGet]
        public IActionResult User()
        {
              var user = HttpContext.Session.GetString("username");
                Console.WriteLine("USER    : : : : : : ::::    " + user);
                List<tblemp> employees = _repo.GetEmployeeFromUserName(user);
                return Json(employees);
        }

        public IActionResult User1()
        {
            return View();
        }

        [HttpPost]
        public IActionResult add(tblemp emp)
        {
            _repo.Insert(emp);
             return Json(new { success = true});  
        }
      
        public IActionResult FetchStates()
        {
            List<tbldept> states = _repo.GetDept();
            return Json(states);
        }
     
        public IActionResult Fetchcourse()
        {
            List<tblcourse> course = _repo.Getcor();
            return Json(course);
        }
       [HttpPost]
public IActionResult UploadImage(IFormFile file)
{
    if (file != null && file.Length > 0)
    {
        var uploads = Path.Combine(_environment.WebRootPath, "images"); // Assuming you have a folder named 'image' in wwwroot
        var filePath = Path.Combine(uploads, file.FileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(fileStream);
        }

        var imageUrl = "/images/" + file.FileName; // Assuming your image URL is relative
        return Json(new { imageUrl });
    }
    return Json(new { error = "No file uploaded or file is empty." });
}
        [HttpPost]
        public IActionResult UpdateCity(tblemp city)
        {
                _repo.Update(city);
                return Json(new { success = true });
        }
         
        [HttpPost]
        public IActionResult Delete(int id)
        {
                Console.WriteLine("Received id:" +id);
                _repo.Delete(id);
                return Json(new { success = true, message = "City deleted." });
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}