using Microsoft.AspNetCore.Mvc;
using Store.Models;
using System.Diagnostics;

namespace Store.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // await CheckRoleAsync();
            // await CheckUserAsync(
            //     "Manuel",
            //     "Espinoza",
            //     "mespinoza@automoto.com",
            //     "76791954",
            //     "Chinandega, puente los miillonarios 35 metros al oeste"
            // );
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                }
            );
        }

        // private async Task CheckRoleAsync()
        // {
        //     await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
        //     await _userHelper.CheckRoleAsync(UserType.User.ToString());
        // }

        // private async Task<User> CheckUserAsync(
        //     string firstName,
        //     string lastName,
        //     string userName,
        //     string phone,
        //     string address
        // )
        // {
        //     User user = await _userHelper.GetUserAsync(userName);
        //     if (user == null)
        //     {
        //         user = new User
        //         {
        //             FirstName = firstName,
        //             LastName = lastName,
        //             Email = $"{userName}@automoto.com",
        //             PhoneNumber = phone,
        //             UserName = userName,
        //             Address = address,
        //             SecondName = "",
        //             SecondLastName = ""
        //         };
        //         await _userHelper.AddUserAsync(user, "123456");
        //         // await _userHelper.AddUserToRoleAsync(user);
        //     }
        //     return user;
        // }
    }
}
