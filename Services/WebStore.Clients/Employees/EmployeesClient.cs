﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebStore.Clients.Base;
using WebStore.Domain.Models;
using WebStore.Interfaces;

namespace WebStore.Clients.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesData
    {
        public EmployeesClient(IConfiguration Configuration) : base(Configuration, WebAPI.Employees)
        {
        }

        public int Add(Employee employee) => Post(Address, employee).Content.ReadAsAsync<int>().Result;

        public bool Delete(int id) => Delete($"{Address}/{id}").IsSuccessStatusCode;

        public IEnumerable<Employee> Get() => Get<IEnumerable<Employee>>(Address);

        public Employee Get(int id) => Get<Employee>($"{Address}/{id}");

        public void Update(Employee employee) => Put(Address,employee);
    }
}
