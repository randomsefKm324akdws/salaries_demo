
namespace bl;

public interface ISalariesService
{
	public Task<decimal> GetMonthlySalaryAsync(int organizationMemberId, DateTime date);
	
	public Task<IEnumerable<OrganizationMemberRead>> GetMonthlySalaryForEachMemberAsync(DateTime date);
	
}