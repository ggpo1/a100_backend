using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A100_AspNetCore.Models.A100_Models.DataBase;
using A100_AspNetCore.Services.API._DBService;
using Microsoft.EntityFrameworkCore;

namespace A100_AspNetCore.Services.API.EmployeesService
{
    public class EmployeeService : IEmployeeService
    {
        public Employee GetEmployee(string userKey)
        {
            return MyDB.db.Employee.First(elem => elem.UserKey == userKey);
        }

        public Task<object> GetEmployeeNameById(int empID)
        {
            Employee emp = MyDB.db.Employee.First(elem => elem.EmployeeId == empID);
            var nameobj = new { EmployeeName = emp.Name };
            return Task.Run(() => (object) nameobj);
        }

        // GET Метод, который получает список клиентов А100
        public async Task<List<Employee>> GetEmployeesClients()
        {
            return await MyDB.db.Employee.ToListAsync();
        }
    }
}
