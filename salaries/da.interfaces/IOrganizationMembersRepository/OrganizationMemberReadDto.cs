namespace da.interfaces.IOrganizationMembersRepository;

public class OrganizationMemberReadDto
{
	public int Id { get; set; }
	public int? ParentId { get; set; }
	public string Name { get; set; }
	public DateTime WorkStartDate { get; set; }
	public decimal BaseSalary { get; set; }
	public int OrganizationMemberType { get; set; } //todo 1-employee, 2-manager, 3-sales
}