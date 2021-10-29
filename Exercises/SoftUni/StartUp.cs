using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System;
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

	}
}
