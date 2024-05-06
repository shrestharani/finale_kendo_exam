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
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ILogger<CustomerController> logger, ICustomerRepository customerRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

    

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(CustomerModel customerModel)
        {
            if (_customerRepository.CheckUsername(customerModel.c_username))
            {
                ViewBag.username = "Username Already Exist";
                return View();
            }
            if (_customerRepository.CheckEmail(customerModel.c_email))
            {
                ViewBag.email = "Email Already Exist";
                return View();
            }
            if (customerModel.c_password != customerModel.ConfirmPassword)
            {
                ViewBag.pass = "Password and Confirm Password Must be same";
                return View();
            }
            _customerRepository.AddUser(customerModel);
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(CustomerModel customerModel)
        {

            if (_customerRepository.Login(customerModel))
            {
                if (HttpContext.Session.GetString("role") == "customer")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (HttpContext.Session.GetString("role") == "admin")
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
            }
            else if (_customerRepository.CheckUsername(customerModel.c_username))
            {
                ViewBag.invalid = "You are not registered";
                return View();
            }
            else
            {
                ViewBag.invalid = "Invalid Password";
                return View();
            }
            return View();



        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}