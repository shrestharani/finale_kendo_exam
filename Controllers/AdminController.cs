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
    public class AdminController : Controller
    {
       private readonly ILogger<AdminController> _logger;
        private readonly IAdminRepository _adminRepository;
        public static string _uploadedFileName;

        public AdminController(ILogger<AdminController> logger, IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("role") == "customer")
            {
                return RedirectToAction("Login", "Customer");
            }
            return View();
        }
        public IActionResult GetAllAlbums()
        {
            var albums = _adminRepository.GetAlbums();
            return Json(albums);
        }

        [HttpPost]
        public IActionResult UploadPhoto(AlbumModel albumModel)
        {
            if (albumModel.image != null)
            {
                string filename = albumModel.image.FileName;
                filename = Guid.NewGuid() + filename;
                string filepath = "wwwroot/images/" + filename;

                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    albumModel.image.CopyTo(stream);
                }
                _uploadedFileName = filename;

                // return Json(new { filename = filename });
            }
            return Json("No image provided.");
        }
        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("role") == "customer")
            {
                return RedirectToAction("Login", "Customer");
            }
            return View();
        }
        [HttpPost]
        public ActionResult DeleteMultipleStudents(List<int> studentIds)
        {
            if (studentIds == null || studentIds.Count == 0)
            {
                return Json(new { success = false, message = "No student IDs provided" });
            }

            bool success = _adminRepository.DeleteMultipleStudents(studentIds);

            if (success)
            {
                return Json(new { success = true, message = "Students deleted successfully" });
            }
            else
            {
                return Json(new { success = false, message = "Failed to delete students" });
            }
        }

        [HttpPost]
        public IActionResult Create(AlbumModel albumModel)
        {
            albumModel.c_album = _uploadedFileName;
            _adminRepository.AddAlbum(albumModel);
            return View();
        }

        [HttpGet]
        public IActionResult Update(int id)
        {

            if (HttpContext.Session.GetString("role") == "customer")
            {
                return RedirectToAction("Login", "Customer");
            }
            var album = _adminRepository.GetAlbums().FirstOrDefault(x => x.c_id == id);
            return View(album);
        }

        [HttpPost]
        public IActionResult Update(AlbumModel albumModel)
        {
            albumModel.c_album = _uploadedFileName;
            _adminRepository.UpdateAlbum(albumModel);
            return View();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _adminRepository.DeleteAlbum(id);
            return Ok();
        }



        public IActionResult GetRevenue()
        {

            var revenue = _adminRepository.GetRevenue();
            return Json(revenue);
        }

        public IActionResult GetWeekRevenue()
        {
            var revenue = _adminRepository.GetWeeklyRevenue();
            return Json(revenue);
        }

        public IActionResult GetMonthRevenue()
        {
            var revenue = _adminRepository.GetMonthRevenue();
            return Json(revenue);
        }

        public IActionResult GetGenre()
        {
            var genre = _adminRepository.GetGenre();
            return Json(genre);
        }

        public IActionResult Chart()
        {
            if (HttpContext.Session.GetString("role") == "customer")
            {
                return RedirectToAction("Login", "Customer");
            }
            return View();
        }

        public IActionResult Cart()
        {
            var items = _adminRepository.GetAllCart();
            return View(items);
        }

        public IActionResult RemoveFromCart(int id)
        {
            _adminRepository.RemoveFromCart(id);
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult AddToCart(CartModel cart)
        {
            _adminRepository.AddToCart(cart);
            return Ok();
        }


        public IActionResult CheckoutDetails()
        {

            return View();
        }
        [HttpPost]
        public IActionResult CheckoutDetails(CheckoutModel checkoutModel)
        {
            _adminRepository.CheckoutDetails(checkoutModel);
            return RedirectToAction("complete");
        }

        public IActionResult Complete()
        {
            ViewBag.total = HttpContext.Session.GetString("total");
            return View();
        }
        public IActionResult Checkout(string total)
        {
            HttpContext.Session.SetString("total", total);

            return Ok("success");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userid = HttpContext.Session.GetString("userid");
            if (userid == null)
            {
                return RedirectToAction("Login", "user");
            }
            return View();
        }



       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}