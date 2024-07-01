using bl;

namespace salaries;

public class Configuration : IConfiguration
{
	public decimal EmployeeLongWorkYearIncreasePercent { get; } = (decimal)0.03;

	public decimal EmployeeMaxPossibleLongWorkIncreasePercent { get; } = (decimal)0.3;

	public decimal ManagerLongWorkYearIncreasePercent { get; } = (decimal)0.05;

	public decimal ManagerMaxPossibleLongWorkIncreasePercent { get; } = (decimal)0.4;

	public decimal ManagerPositionBonusPercentOfFirstLevelChildSalaries { get; } = (decimal)0.005;


	public decimal SalesLongWorkYearIncreasePercent { get; } = (decimal)0.01;

	public decimal SalesMaxPossibleLongWorkIncreasePercent { get; } = (decimal)0.35;

	public decimal SalesPositionBonusPercentOfAllLevelChildSalaries { get; } = (decimal)0.003;
}