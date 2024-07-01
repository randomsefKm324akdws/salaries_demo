
namespace bl;

public interface ISalariesService
{
	public Task<decimal> GetMonthlySalaryAsync(int organizationMemberId, DateTime date);
	
	public Task<decimal> GetMonthlySalaryForAllOrganizationMembersAsync(DateTime date);
}