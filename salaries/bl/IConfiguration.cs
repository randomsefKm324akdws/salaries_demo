namespace bl;

public interface IConfiguration
{
	public decimal EmployeeLongWorkYearIncreasePercent { get; }
	public decimal EmployeeMaxPossibleLongWorkIncreasePercent { get; }
	
	public decimal ManagerLongWorkYearIncreasePercent { get; }
	public decimal ManagerMaxPossibleLongWorkIncreasePercent { get; }
	public decimal ManagerPositionBonusPercentOfFirstLevelChildSalaries { get; }

	public decimal SalesLongWorkYearIncreasePercent { get; }
	public decimal SalesMaxPossibleLongWorkIncreasePercent { get; }
	public decimal SalesPositionBonusPercentOfAllLevelChildSalaries { get; }
}