using CoolEvents.ActionFilter;
using CoolEvents.Data;
using CoolEvents.ExtensionMethods;
using CoolEvents.Models;
using CoolEvents.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CoolEvents.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            this._context = context;
        }

        public IActionResult Index()
        {
            //count of users
            ViewData["CountOfUsers"] = _context.Users.Count().ToString();
            //count of events
            ViewData["CountOfEvents"] = _context.Events.Count().ToString();
            //count of reserved tickets
            ViewData["CountOfTickets"] = _context.UserTickets.Count().ToString();
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM model)
        {
            if (!this.ModelState.IsValid)
                return View(model);

            User loggedUser = _context.Users.Where(u => u.Username == model.Username && u.Password == model.Password).FirstOrDefault();

            if (loggedUser == null)
            {
                this.ModelState.AddModelError("authError", "Invalid username or password!");
                return View(model);
            }

            HttpContext.Session.SetObject("loggedUser", loggedUser);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {
            if (!this.ModelState.IsValid)
                return View(model);

            User user = new User();
            try
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Username = model.Username;
                user.Password = model.Password;
                user.RoleId = 2;
                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Login", "Home");
            }
            catch (Exception)
            {
                return View();
            }
        }

        [AuthenticationFilter]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("loggedUser");

            return RedirectToAction("Login", "Home");
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