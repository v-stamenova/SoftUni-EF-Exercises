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
			Console.WriteLine(GetEmployeesFromResearchAndDevelopment(new SoftUniContext()));
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
	}
}
