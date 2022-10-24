using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private static readonly List<Employee> __Employees = new()
        {
            new Employee { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic= "Иванович", Age = 20 },
            new Employee { Id = 2, LastName = "Петров", FirstName = "Петр", Patronymic = "Иванович", Age = 25 },
            new Employee { Id = 3, LastName = "Павлов", FirstName = "Иван", Patronymic = "Петрович", Age = 23 },
        };
        public IActionResult Index() => View();

        public IActionResult Employees()
        {
            return View(__Employees);
        }
    }
}
