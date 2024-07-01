// See https://aka.ms/new-console-template for more information

using bl;
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
			Console.WriteLine("Calculating salary for all organization members...");
			Console.WriteLine("Year (numeric):");
			var salaryYear = Console.ReadLine();
			Console.WriteLine("Month (numeric):");
			var salaryMonth = Console.ReadLine();
			Console.WriteLine("Day of month (numeric):");
			var salaryDay = Console.ReadLine();
			var salaryForDateTime = new DateTime(Convert.ToInt32(salaryYear), Convert.ToInt32(salaryMonth), Convert.ToInt32(salaryDay));
			Console.WriteLine("Calculating salary for all organization members for date " + salaryForDateTime + "...");

			var serviceProvider = services.BuildServiceProvider();

			var salariesService = serviceProvider.GetService<ISalariesService>();
			var res = (await salariesService.GetMonthlySalaryForEachMemberAsync(salaryForDateTime)).ToArray();
			foreach (var member in res.OrderBy(x => x.Id))
			{
				Console.WriteLine("Salary for member Id " + member.Id + " (" + member.Name + "): " + member.Salary);
			}

			Console.WriteLine("Total Salary for all organization members: " + res.Sum(x => x.Salary));

			Console.WriteLine("End. Press any key");
			Console.ReadKey();
		}
		catch (Exception ex)
		{
			Console.WriteLine("Unhandled exception: " + ex.Message + ": " + ex.StackTrace);
			Console.ReadKey();
		}
	}
}