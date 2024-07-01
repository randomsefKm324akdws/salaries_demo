using da.interfaces.IOrganizationMembersRepository;

namespace da;

public class InMemoryTestOrganizationMembersRepository : IOrganizationMembersRepository
{
	public async Task<OrganizationMemberReadDto[]> GetAsync()
	{
		//1 employee
		//2 manager
		//3 sales
		OrganizationMemberReadDto[] employees =
		{
			new()
			{
				Id = 1,
				ParentId = null,
				Name = "manager 2",
				WorkStartDate = new DateTime(2000, 1, 1),
				OrganizationMemberType = 2,
				BaseSalary = 100
			},
			new()
			{
				Id = 2,
				ParentId = 1,
				Name = "employee 2",
				WorkStartDate = new DateTime(2000, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = 100
			},
			new()
			{
				Id = 3,
				ParentId = 1,
				Name = "employee 3",
				WorkStartDate = new DateTime(2001, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = 100
			},
			new()
			{
				Id = 4,
				ParentId = null,
				Name = "employee 4 without manager",
				WorkStartDate = new DateTime(2001, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = 100
			},
			new()
			{
				Id = 5,
				ParentId = 1,
				Name = "sales 5",
				WorkStartDate = new DateTime(2001, 1, 1),
				OrganizationMemberType = 3,
				BaseSalary = 100
			},
			new()
			{
				Id = 6,
				ParentId = null,
				Name = "sales 6 without manager",
				WorkStartDate = new DateTime(2001, 1, 1),
				OrganizationMemberType = 3,
				BaseSalary = 100
			}
		};

		return await Task.FromResult(employees);
	}
}