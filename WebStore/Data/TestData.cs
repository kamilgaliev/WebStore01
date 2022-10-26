using System.Collections.Generic;
using WebStore.Models;

namespace WebStore.Data
{
    public static class TestData
    {

        public static List<Employee> Employees { get; } = new()
        {
            new Employee { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 20 },
            new Employee { Id = 2, LastName = "Петров", FirstName = "Петр", Patronymic = "Иванович", Age = 25 },
            new Employee { Id = 3, LastName = "Павлов", FirstName = "Иван", Patronymic = "Петрович", Age = 23 },
        };
    }
}
