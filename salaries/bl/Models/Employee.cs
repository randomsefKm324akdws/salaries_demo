namespace bl.Models;

internal class Employee : OrganizationMemberBase
{
	protected override decimal LongWorkYearIncreasePercent => Configuration.EmployeeLongWorkYearIncreasePercent;
	protected override decimal MaxPossibleLongWorkIncreasePercent => Configuration.EmployeeMaxPossibleLongWorkIncreasePercent;

	protected override decimal CalculatePositionBonus(DateTime date)
	{
		return 0;
	}
	
	public override void AddChildNodes(IEnumerable<OrganizationMemberBase> nodes)
	{
		if (!nodes.Any())
		{
			return;
		}
		throw new Exception("Employee cannot have child members.");
	}

	public Employee(IConfiguration configuration) : base(configuration)
	{
	}
}