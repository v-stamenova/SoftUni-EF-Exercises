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

			IOrderedQueryable<Employee> orderedEmployees = employeesFromReseacrhAndDevelopment.OrderBy(x => x.Salary).ThenByDescending(x => x.FirstName);

			foreach (Employee emp in orderedEmployees)
			{
				stringBuilder.AppendLine($"{emp.FirstName} {emp.LastName} from Research and Development - ${emp.Salary:f2}");
			}

			return stringBuilder.ToString();
		}
	}
}
