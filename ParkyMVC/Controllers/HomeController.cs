using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkyMVC.Models;
using ParkyMVC.Models.ViewModels;
using ParkyMVC.Repository.IRepository;
using ParkyMVC.Utility;

namespace ParkyMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _trailRepo;
        private readonly IAccountRepository _userRepo;

        public HomeController(ILogger<HomeController> logger, INationalParkRepository npRepo, ITrailRepository trailRepo, IAccountRepository userRepo)
        {
            _logger = logger;
            _npRepo = npRepo;
            _trailRepo = trailRepo;
            _userRepo = userRepo;
        }

        public async Task<IActionResult> Index()
        {
            IndexViewModel listOfParksAndTrails = new IndexViewModel()
            {
                NationalParkList = await _npRepo.GetAllAsync(Static.NationalParkAPIPath),
                TrailList = await _trailRepo.GetAllAsync(Static.TrailAPIPath),
            };
            return View(listOfParksAndTrails);
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

        [HttpGet]
        public IActionResult Login()
        {
            User obj = new User();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User obj)
        {
            User objUser = await _userRepo.LoginAsync(Static.AccountAPIPath + "authenticate/", obj);
            if (objUser.Token == null)
            {
                return View();
            }

            HttpContext.Session.SetString("JWToken", objUser.Token);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User obj)
        {
            bool result = await _userRepo.RegisterAsync(Static.AccountAPIPath + "register/", obj);
            if (result == false)
            {
                return View();
            }
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("JWToken", "");
            return RedirectToAction("Login");
        }
    }
}