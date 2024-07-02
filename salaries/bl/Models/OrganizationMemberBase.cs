using System.Collections.ObjectModel;
using bl.Helpers;

namespace bl.Models;

internal abstract class OrganizationMemberBase
{
	protected readonly IConfiguration Configuration;

	protected OrganizationMemberBase(IConfiguration configuration)
	{
		Configuration = configuration;
	}
	
	public int Id { get; init; }
	public string Name { get; init; }
	public DateTime WorkStartDate { get; init; }
	public decimal BaseSalary { get; init; }

	public int? ParentId { get; init; } //manager

	private readonly List<OrganizationMemberBase> _childNodesPrivate = new();

	public IList<OrganizationMemberBase> ChildNodes => new ReadOnlyCollection<OrganizationMemberBase>(_childNodesPrivate);

	public virtual void AddChildNodes(IEnumerable<OrganizationMemberBase> nodes)
	{
		_childNodesPrivate.AddRange(nodes);
	}
	

	protected abstract decimal LongWorkYearIncreasePercent { get; }
	protected abstract decimal MaxPossibleLongWorkIncreasePercent { get; }


	protected abstract decimal CalculatePositionBonus(DateTime date);
	
	private decimal? _cachedCalculatedSalary;

	public decimal CalculateFullSalary(DateTime date)
	{
		if (_cachedCalculatedSalary != null)
		{
			return (decimal)_cachedCalculatedSalary;
		}

		if (date < WorkStartDate)
		{
			return 0;
		}

		var total = CalculateLongWorkSalary(date) + CalculatePositionBonus(date);

		_cachedCalculatedSalary = total;

		return total;
	}

	private decimal CalculateLongWorkSalary(DateTime date)
	{
		var yearsWorked = DateHelpers.YearsBetweenDates(WorkStartDate, date);

		var maxPossibleLongWorkBonus = BaseSalary + BaseSalary * MaxPossibleLongWorkIncreasePercent;
		
		var salary = BaseSalary;

		var yearIncrease = BaseSalary * LongWorkYearIncreasePercent;
		for (var i = 0; i < yearsWorked; i++)
		{
			salary += yearIncrease;
		}

		if (salary > maxPossibleLongWorkBonus)
		{
			return maxPossibleLongWorkBonus;
		}

		return salary;
	}
}