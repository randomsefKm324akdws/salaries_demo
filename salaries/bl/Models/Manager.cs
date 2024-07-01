namespace bl.Models;

internal class Manager : OrganizationMemberBase
{
	protected override decimal LongWorkYearIncreasePercent => Configuration.ManagerLongWorkYearIncreasePercent;
	protected override decimal MaxPossibleLongWorkIncreasePercent => Configuration.ManagerMaxPossibleLongWorkIncreasePercent;

	protected override decimal CalculatePositionBonus(DateTime date)
	{
		decimal firstLevelChildSalaries = 0;
		foreach (var childNode in ChildNodes)
		{
			firstLevelChildSalaries += childNode.CalculateFullSalary(date);
		}
		
		var positionBonus = firstLevelChildSalaries * Configuration.ManagerPositionBonusPercentOfFirstLevelChildSalaries;
		
		return positionBonus;
	}

	public Manager(IConfiguration configuration) : base(configuration)
	{
	}
}