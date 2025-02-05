using Application.Data;
using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SoftUni
{
    public class StartUp
    {
        static void Main()
        {
            using(SoftUniContext context = new SoftUniContext())
            {
                Console.WriteLine(DeleteProjectById(context));
            }
        }
        public static string GetEmployeesFullInforamtion(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary
                }).ToList();

            string output = string.Join(Environment.NewLine,
                employees.Select(e => 
                $"{e.FirstName} {e.MiddleName} {e.LastName} {e.JobTitle} {e.Salary:f2}"));

            return output;

        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var empolyees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .Where(e => e.Salary > 50000)
                .ToList()
                .OrderByDescending(e => e.Salary);

            return System.String.Join(Environment.NewLine, empolyees.Select(e =>
                $"{e.FirstName} - {e.Salary:f2}"));
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees
                      .Select(e => new
                      {
                          e.FirstName,
                          e.LastName,
                          e.Department,
                          e.Salary,
                      })
                      .Where(e =>
                          e.Department.Name.Equals("Research and Development "))
                      .ToList()
                      .OrderBy(e => e.Salary)
                      .ThenByDescending(e => e.FirstName);

            return String.Join(Environment.NewLine,
                employees.Select(e =>
                    $"{e.FirstName} {e.LastName} from {e.Department} - ${e.Salary:f2}"));
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var employees = context.Employees;

            Address adreess = new()
            {
                AddressText = "Vitoshka 15",
                TownId = 4,
            };

            employees
                .FirstOrDefault(e => e.LastName == "Nakov")
                .Address = adreess;

            context.SaveChanges();

            var outputEmployees = employees.Select(e => new
                {
                    e.Address.AddressText,
                    e.AddressId,
                })
                .ToList()
                .OrderByDescending(e => e.AddressId)
                .Take(10);


            return string.Join(Environment.NewLine, outputEmployees.Select(e => 
                $"{e.AddressText}"));
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            string outputString = "";

            DateTime date1 = new DateTime(2001, 1, 1);
            DateTime date2 = new DateTime(2003, 12, 31);

            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Manager,
                    e.Projects
                }).ToList();

            var firstTen = employees.Take(10);

            foreach(var employee in firstTen)
            {
                outputString += $"{employee.FirstName} {employee.LastName}: " +
                    $"Manager {employee.Manager.FirstName} {employee.Manager.LastName}\n";

                var currentProjects = employee.Projects.Select(p => new
                {
                    p.Name,
                    p.StartDate,
                    p.EndDate,
                });

                foreach(var project in currentProjects)
                {
                    if(DateTime.Compare(date1, project.StartDate) <= 0 &&
                        DateTime.Compare(date2, project.StartDate) >= 0)
                    {
                        if(project.EndDate != null)
                        {
                            outputString += $"--{project.Name} - {project.StartDate} - {project.EndDate}\n";
                        }
                        else
                        {
                            outputString += $"--{project.Name} - {project.StartDate} - not finished\n";
                        }
                    }
                }
            }

            return outputString;
        }

        public static string GetAddressesByTown1(SoftUniContext context)
        {
            var output = "";

            var addresses = context.Addresses.Select(a => new
            {
                TownName = a.Town.Name,
                a.AddressText,
                a.Employees.Count
            })
                .ToList()
                .OrderByDescending(a => a.Count)
                .ThenBy(a => a.TownName)
                .ThenBy(a => a.AddressText)
                .ToList()
                .Take(10)
                .ToList();

            foreach(var address in addresses)
            {
                output += $"{address.AddressText}, {address.TownName} - {address.Count} employees\n";
            }

            return output;
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Projects,
                    e.EmployeeId
                })
                .FirstOrDefault(e => e.EmployeeId == 147);


            return $"{employee.FirstName} {employee.LastName} - {employee.JobTitle}\n" +
                $"{String.Join(Environment.NewLine, employee.Projects.Select(p => p.Name).OrderBy(e => e).ToList())}";
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            string output = "";

            var departments = context.Departments
                .Select(e => new
                {
                    e.Name,
                    e.Manager,
                    e.Employees
                }).ToList()
                .Where(e => e.Employees.Count > 5)
                .OrderBy(e => e.Employees.Count)
                .ThenBy(e => e.Name)
                .ToList();

            foreach(var department in departments)
            {
                output += $"{department.Name} - {department.Manager.FirstName} {department.Manager.LastName}\n";
                
                foreach(var employee in department.Employees
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName))
                {
                    output += $"{employee.FirstName} {employee.LastName} - {employee.JobTitle}\n";
                }
            }

            return output;
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            string output = "";

            var latestProjects = context.Projects.Select(e => new
            {
                e.Name,
                e.Description,
                e.StartDate,
            })
                .ToList()
                .OrderByDescending(e => e.StartDate)
                .Take(10);

            foreach(var project in latestProjects)
            {
                output += $"{project.Name}\n{project.Description}\n{project.StartDate}\n\n";
            }

            return output;
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            string output = "";
            string[] departments = { "Engineering", "Tool Design", "Marketing", "Information Services" };

            var employees = context.Employees.Where(e => departments.Contains(e.Department.Name)).ToList();

            foreach(var employee in employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName))
            {
                employee.Salary *= (decimal)1.12;
                output += $"{employee.FirstName} {employee.LastName} (${employee.Salary:f2})\n";
            }

            return output;
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            string output = "";

            var employees = context.Employees.Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.JobTitle,
                e.Salary
            })
                .Where(e => e.FirstName.ToLower().StartsWith("sa"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            foreach(var employee in employees)
            {
                output += $"{employee.FirstName} {employee.LastName} - {employee.JobTitle}" +
                    $" - (${employee.Salary:f2})\n";
            }

            return output;
        }

        public static string DeleteProjectById(SoftUniContext context)
        {

        string output = "";
         var projectProject = context.Projects.Find(2);

            context.Projects.Remove(projectProject);
            context.Employees
                .Select(e => e.Projects)
                .FirstOrDefault(p => p.Remove(projectProject));

            context.SaveChanges();

            foreach(var project in context.Projects.ToList())
            {
                output += ${ project.Name}\n";
            }
        }
    }
}