using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using WebStore.Domain.Models;
using WebStore.Interfaces;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Employees)]
    [ApiController]
    public class EmployeesApiController : ControllerBase, IEmployeesData
    {
        private readonly IEmployeesData _EmployeesData;
        private readonly ILogger<EmployeesApiController> _Logger;

        public EmployeesApiController(IEmployeesData EmployeesData, ILogger<EmployeesApiController> Logger)
        {
            _EmployeesData = EmployeesData;
            _Logger = Logger;
        }

        [HttpPost]
        public int Add(Employee employee)
        {
            _Logger.LogInformation("Добавление сотрудника {0}", employee);
            return _EmployeesData.Add(employee);
        }

        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            _Logger.LogInformation("Удаление сотрудника id: {0} ...", id);
            var result = _EmployeesData.Delete(id);
            _Logger.LogInformation("Удаление сотрудника id: {0} - {1}", id, result ? "выполнено" : "не найден");

            return result;
        }

        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            return _EmployeesData.Get();
        }

        [HttpGet("{id}")]
        public Employee Get(int id)
        {
            return _EmployeesData.Get(id);
        }

        [HttpPut]
        public void Update(Employee employee)
        {
            _Logger.LogInformation("Редактирование сотрудника {0}", employee);
            _EmployeesData.Update(employee);
        }
    }
}
