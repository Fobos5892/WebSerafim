using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SerafimeWeb.Models;
using SerafimeWeb.ViewModels;

namespace SerafimeWeb.Controllers
{
    public class UsersController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private const int PageSize = 4;

        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult List(SearchType searchType, string searchString, int page = 1)
        {
            IQueryable<User> users = _userManager.Users.OrderBy(p => p.Email);

            if (!string.IsNullOrEmpty(searchString))
            {
                switch (searchType)
                {
                    case SearchType.SearchByLogin:
                        users = users.Where(user => user.UserName.Contains(searchString));
                        break;

                    case SearchType.SearchByFullName:
                        users = users.Where(user => user.FullName.Contains(searchString));
                        break;

                    case SearchType.SearchByEmail:
                        users = users.Where(user => user.Email.Contains(searchString));
                        break;

                    default:
                        users = users.Where(user => user.UserName.Contains(searchString));
                        break;
                }
            }

            ListViewModel userListViewModel = new ListViewModel
            {
                Users = users
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = users.Count()
                },
                SearchString = searchString,
                SearchType = searchType
            };

            return View(userListViewModel);
        }
    }
}