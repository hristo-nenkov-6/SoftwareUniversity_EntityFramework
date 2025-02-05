using MiniORM.App.Data.Entities;
using MiniORM.App.Data;
using System.ComponentModel.DataAnnotations;

var connectionString = "Server=.;Database=MiniORM;Integrated Security=True;Encrypt=False";

var context = new SoftUniDbContext(connectionString);

context.Employees.Add(new Employee
{
    FirstName = "Gosho",
    //MiddleName = "Ivanov",
    LastName = "Petkov",
    DepartmentId = context.Departments.First().Id,
    IsEmployed = true,
});

var employee = context.Employees.Last();
employee.FirstName = "Modified";

context.SaveChanges();