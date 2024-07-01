// See https://aka.ms/new-console-template for more information

using bl;
using bl.Exceptions;
using da;
using da.interfaces.IOrganizationMembersRepository;
using Microsoft.Extensions.DependencyInjection;
using salaries;

internal class Program
{
	private static async Task Main(string[] args)
	{
		ServiceCollection services = new();
		services.AddTransient<IOrganizationMembersRepository, InMemoryTestOrganizationMembersRepository>();
		services.AddTransient<ISalariesService, SalariesService>();
		services.AddTransient<IConfiguration, Configuration>();


		try
		{
			Console.WriteLine("Would you like to calculate salary for all organization members? (y/n)");
			var questionCalcForAllMembers = Console.ReadLine();
			if (questionCalcForAllMembers == "y")
			{
				Console.WriteLine("Enter year for which salary will be calculated for all organization members:");
				var salaryYear = Console.ReadLine();
				Console.WriteLine("Enter month for which salary will be calculated all organization members:");
				var salaryMonth = Console.ReadLine();
				Console.WriteLine("Enter day for which salary will be calculated all organization members:");
				var salaryDay = Console.ReadLine();

				var salaryForDateTime = new DateTime(Convert.ToInt32(salaryYear), Convert.ToInt32(salaryMonth), Convert.ToInt32(salaryDay));

				Console.WriteLine("Calculating salary for all organization members for date " + salaryForDateTime + "...");

				var serviceProvider = services.BuildServiceProvider();

				var salariesService = serviceProvider.GetService<ISalariesService>();
				var res = (await salariesService.GetMonthlySalaryForEachMemberAsync(salaryForDateTime)).ToArray();
				foreach (var member in res)
				{
					Console.WriteLine("Salary for member Id " + member.Id + ": " + member.Salary);
				}
				
				Console.WriteLine("Total Salary for all organization members: " + res.Sum(x=>x.Salary));
			}
			
			Console.WriteLine("Would you like to calculate salary for specific organization member Id? (y/n)");
			var questionCalcForSpecificMember = Console.ReadLine();
			if (questionCalcForSpecificMember == "y")
			{
				Console.WriteLine("Enter organization member Id:");
				var memberId = Console.ReadLine();
				Console.WriteLine("Enter year for which salary will be calculated:");
				var memberSalaryYear = Console.ReadLine();
				Console.WriteLine("Enter month for which salary will be calculated:");
				var memberSalaryMonth = Console.ReadLine();
				Console.WriteLine("Enter day for which salary will be calculated:");
				var memberSalaryDay = Console.ReadLine();

				var memberSalaryForDateTime = new DateTime(Convert.ToInt32(memberSalaryYear), Convert.ToInt32(memberSalaryMonth), Convert.ToInt32(memberSalaryDay));

				Console.WriteLine("Calculating salary for member Id: " + memberId + " for date " + memberSalaryForDateTime + "...");
				
				var serviceProvider = services.BuildServiceProvider();

				var salariesService = serviceProvider.GetService<ISalariesService>();
				var salary = await salariesService.GetMonthlySalaryAsync(Convert.ToInt32(memberId), memberSalaryForDateTime);
				Console.WriteLine("Salary: " + salary);
			}

			Console.WriteLine("End. Press any key");
			Console.ReadKey();
		}
		catch (MemberNotFoundException)
		{
			Console.WriteLine("Member not found");
			Console.ReadKey();
		}
		catch (Exception ex)
		{
			Console.WriteLine("Unhandled exception: " + ex.Message + ": " + ex.StackTrace);
			Console.ReadKey();
		}
	}
}