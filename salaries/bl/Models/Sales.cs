namespace bl.Models;

internal class Sales : OrganizationMemberBase
{
	public Sales(IConfiguration configuration) : base(configuration)
	{
	}

	protected override decimal LongWorkYearIncreasePercent => Configuration.SalesLongWorkYearIncreasePercent;
	protected override decimal MaxPossibleLongWorkIncreasePercent => Configuration.SalesMaxPossibleLongWorkIncreasePercent;

	protected override decimal CalculatePositionBonus(DateTime date)
	{
		decimal allChildSalaries = 0;

		foreach (var childNode in ChildNodes)
		{
			TraverseDfs(ref allChildSalaries, childNode, date);
		}

		var positionBonus = allChildSalaries * Configuration.SalesPositionBonusPercentOfAllLevelChildSalaries;

		return positionBonus;
	}
	
	private static void TraverseDfs(ref decimal salary, OrganizationMemberBase node, DateTime date)
	{
		if (node == null)
		{
			return;
		}

		var stk = new Stack<OrganizationMemberBase>();
		stk.Push(node);

		while (stk.Any())
		{
			var top = stk.Pop();

			//process top stack node:
			salary += top.CalculateFullSalary(date);
			//
			
			foreach (var child in top.ChildNodes)
			{
				stk.Push(child);
			}
		}
	}
}