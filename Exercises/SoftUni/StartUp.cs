using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftUni
{
	public class StartUp
	{
		static void Main(string[] args)
		{
			Console.WriteLine(GetLatestProjects(new SoftUniContext()));
		}

		public static string GetEmployeesFullInformation(SoftUniContext context)
		{
			StringBuilder stringBuilder = new StringBuilder();
			DbSet<Employee> employees = context.Employees;

			IOrderedQueryable<Employee> orderedEmployees = employees.OrderBy(x => x.EmployeeId);

			foreach (Employee emp in orderedEmployees)
			{
				stringBuilder.AppendLine($"{emp.FirstName} {emp.MiddleName} {emp.LastName} {emp.JobTitle} {emp.Salary:f2}");
			}

			return stringBuilder.ToString();
		}

		public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
		{
			StringBuilder stringBuilder = new StringBuilder();
			IOrderedQueryable<Employee> employeesOver50000 = context.Employees.Where(x => x.Salary > 50000).OrderBy(x => x.FirstName);

			foreach(Employee emp in employeesOver50000)
			{
				stringBuilder.AppendLine($"{emp.FirstName} - {emp.Salary:f2}");
			}

			return stringBuilder.ToString();
		}

		public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
		{
			StringBuilder stringBuilder = new StringBuilder();
			IQueryable<Employee> employeesFromReseacrhAndDevelopment = context.Employees.Where(x => x.Department.Name == "Research and Development");

			var orderedEmployees = employeesFromReseacrhAndDevelopment
				.OrderBy(x => x.Salary)
				.ThenByDescending(x => x.FirstName)
				.Select(x => new
				{
					x.FirstName,
					x.LastName,
					DepartmentName = x.Department.Name,
					x.Salary
				})
				.ToList();

			foreach(var emp in orderedEmployees)
			{
				stringBuilder.AppendLine($"{emp.FirstName} {emp.LastName} from {emp.DepartmentName} - ${emp.Salary:f2}");
			}

			return stringBuilder.ToString();
		}

		public static string AddNewAddressToEmployee(SoftUniContext context)
		{
			var address = new Address()
			{
				AddressText = "Vitoshka 15",
				TownId = 4
			};
			context.Addresses.Add(address);

			Employee nakovEmployee = context.Employees.First(e => e.LastName == "Nakov");
			nakovEmployee.Address = address;

			context.SaveChanges();

			var allEmployees = context.Employees
				.OrderByDescending(e => e.AddressId)
				.Select(e => e.Address.AddressText).Take(10).ToArray();

			return string.Join(Environment.NewLine, allEmployees);
		}

		public static string GetEmployeesInPeriod(SoftUniContext context)
		{
			StringBuilder stringBuilder = new StringBuilder();

			var employees = context.Employees
				.Where(e => e.EmployeesProjects
					.Any(x => x.Project.StartDate.Year >= 2001 && x.Project.StartDate.Year <= 2003))
				.Select(e => new
				{
					FirstName = e.FirstName,
					LastName = e.LastName,

					ManagerFirstName = e.Manager.FirstName,
					ManagerLastName = e.Manager.LastName,

					Projects = e.EmployeesProjects.Select(p => new
					{
						Name = p.Project.Name,
						StartDate = p.Project.StartDate,
						EndDate = p.Project.EndDate,
					})
				})
				.Take(10);

			foreach(var emp in employees)
			{
				stringBuilder.AppendLine($"{emp.FirstName} {emp.LastName} - Manager: {emp.ManagerFirstName} {emp.ManagerLastName}");
				
				foreach(var proj in emp.Projects)
				{
					if(proj.EndDate is null)
					{
						stringBuilder.AppendLine($"--{proj.Name} - {proj.StartDate.ToString("M/d/yyyy h:mm:ss tt")} - not finished");
					}
					else
					{
						stringBuilder.AppendLine($"--{proj.Name} - {proj.StartDate.ToString("M/d/yyyy h:mm:ss tt")} - {proj.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt")}");
					}
				}
			}

			return stringBuilder.ToString();
		}

		public static string GetAddressesByTown(SoftUniContext context)
		{
			var addresses = context.Addresses
				.Select(a => new
				{
					AddressText = a.AddressText,
					TownName = a.Town.Name,
					EmployeeCount = a.Employees.Count()
				})
				.OrderByDescending(a => a.EmployeeCount)
				.ThenBy(a => a.TownName)
				.ThenBy(a => a.AddressText)
				.Take(10)
				.ToList();

			StringBuilder builder = new StringBuilder();

			foreach(var add in addresses)
			{
				builder.AppendLine($"{add.AddressText}, {add.TownName} - {add.EmployeeCount} employees");
			}

			return builder.ToString();
		}

		public static string GetEmployee147(SoftUniContext context)
		{
			var employee = context.Employees
				.Where(x => x.EmployeeId == 147)
				.Select(x => new
				{
					FirstName = x.FirstName,
					LastName = x.LastName,
					JobTitle = x.JobTitle,
					Projects = x.EmployeesProjects
						.Select(ep => ep.Project.Name)
						.OrderBy(ep => ep)
						.ToList()
				})
				.Single();

			string output = $"{employee.FirstName} {employee.LastName} - {employee.JobTitle}{Environment.NewLine}";
			output += string.Join(Environment.NewLine, employee.Projects);

			return output;
		}

		public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
		{
			var departments = context.Departments
				.Where(d => d.Employees.Count() > 5)
				.OrderBy(d => d.Employees.Count())
				.ThenBy(d => d.Name)
				.Select(de => new
				{
					Name = de.Name,
					ManagerFirstName = de.Manager.FirstName,
					ManagerLastName = de.Manager.LastName,
					Employees = de.Employees
						.Select(e => new
						{
							FirstName = e.FirstName,
							LastName = e.LastName,
							JobTitle = e.JobTitle
						})
						.OrderBy(e => e.FirstName)
						.ThenBy(e => e.LastName)
						.ToList()
				});

			StringBuilder builder = new StringBuilder();

			foreach(var dep in departments)
			{
				builder.AppendLine($"{dep.Name} - {dep.ManagerFirstName} {dep.ManagerLastName}");
				
				foreach(var emp in dep.Employees)
				{
					builder.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle}");
				}
			}

			return builder.ToString();
		}

		public static string GetLatestProjects(SoftUniContext context)
		{
			var projects = context.Projects
				.OrderByDescending(x => x.StartDate)
				.Take(10)
				.Select(x => new
				{
					Name = x.Name,
					Description = x.Description,
					StartDate = x.StartDate
				})
				.OrderBy(x => x.Name)
				.ToList();

			StringBuilder builder = new StringBuilder();

			foreach (var project in projects)
			{
				builder.AppendLine(project.Name);
				builder.AppendLine(project.Description);
				builder.AppendLine(project.StartDate.ToString("M/d/yyyy h:mm:ss tt"));
			}

			return builder.ToString();
		}
	}
}
