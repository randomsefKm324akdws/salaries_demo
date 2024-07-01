namespace bl.Models;

internal class Employee : OrganizationMemberBase
{
	protected override decimal LongWorkYearIncreasePercent => Configuration.EmployeeLongWorkYearIncreasePercent;
	protected override decimal MaxPossibleLongWorkIncreasePercent => Configuration.EmployeeMaxPossibleLongWorkIncreasePercent;

	protected override decimal CalculatePositionBonus(DateTime date)
	{
		return 0;
	}
	
	public override void AddChildNode(OrganizationMemberBase node)
	{
		throw new Exception("Employee cannot have child members.");
	}

	public Employee(IConfiguration configuration) : base(configuration)
	{
	}
}