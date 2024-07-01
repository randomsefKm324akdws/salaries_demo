namespace da.interfaces.IOrganizationMembersRepository;

public interface IOrganizationMembersRepository
{
	public Task<OrganizationMemberReadDto[]> GetAsync();
}