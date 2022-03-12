using Microsoft.AspNetCore.Mvc;
using Store.Enums;
using Store.Helpers.User;
using Store.Models;
using System.Diagnostics;

namespace Store.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserHelper _userHelper;

        public HomeController(ILogger<HomeController> logger, IUserHelper userHelper)
        {

            _logger = logger;
            _userHelper = userHelper;
        }

        public async Task<IActionResult> Index()
        {
            await CheckRoleAsync();
            await CheckUserAsync("Manuel", "Espinoza", "mespinoza@automoto.com", "76791954", "Chinandega, puente los miillonarios 35 metros al oeste", UserType.Admin);
            return View();
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

        private async Task CheckRoleAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task<Store.Entities.User> CheckUserAsync(string firstName, string lastName, string email, string phone, string address, UserType userType)
        {
            Store.Entities.User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new Entities.User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    PhoneNumber = phone,
                    UserType = userType,
                    UserName = email,
                    Address = address,
                    SecondName = "",
                    SecondLastName = ""
                };
                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }
            return user;
        }
    }
}