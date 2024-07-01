using bl.Models;
using da.interfaces.IOrganizationMembersRepository;

namespace bl;

public class SalariesService : ISalariesService
{
	private readonly IConfiguration _configuration;
	private readonly IOrganizationMembersRepository _organizationMembersRepository;

	public SalariesService(IOrganizationMembersRepository organizationMembersRepository, IConfiguration configuration)
	{
		_organizationMembersRepository = organizationMembersRepository;
		_configuration = configuration;
	}

	public async Task<decimal> GetMonthlySalaryAsync(int organizationMemberId, DateTime date)
	{
		var membersDict = await GetMembersDictAsync();

		if (!membersDict.ContainsKey(organizationMemberId))
		{
			throw new MemberNotFoundException();
		}

		var selectedNode = membersDict[organizationMemberId];
		var salary = selectedNode.CalculateFullSalary(date);
		return salary;

	}

	public async Task<decimal> GetMonthlySalaryForAllOrganizationMembersAsync(DateTime date)
	{
		var membersDict = await GetMembersDictAsync();
		decimal salaryForAllOrganizationMembers = 0;
		foreach (var x in membersDict)
		{
			salaryForAllOrganizationMembers += x.Value.CalculateFullSalary(date);
		}

		return salaryForAllOrganizationMembers;
	}

	private async Task<Dictionary<int, OrganizationMemberBase>> GetMembersDictAsync()
	{
		var flatDtos = await _organizationMembersRepository.GetAsync();
		var blObjects = flatDtos.Select(CreateEmployeeBlModel).ToArray();

		FIllChildNodesForEachNode(blObjects);

		var membersDict = blObjects.ToDictionary(x => x.Id);
		return membersDict;
	}


	private static void FIllChildNodesForEachNode(OrganizationMemberBase[] blObjects)
	{
		var nodes = blObjects.ToLookup(d => d.ParentId);
		var rootNodes = nodes[null].ToArray();
		
		foreach (var rootNode in rootNodes)
		{
			FillChildNodesTraverseDfs(rootNode, nodes);
		}
	}


	// var 1: DFS without recursion:
	private static void FillChildNodesTraverseDfs(OrganizationMemberBase node, ILookup<int?, OrganizationMemberBase> nodes)
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
			ProcessTopStackNode(top, nodes);
			//

			foreach (var child in top.ChildNodes)
			{
				stk.Push(child);
			}
		}
	}

	private static void ProcessTopStackNode(OrganizationMemberBase node, ILookup<int?, OrganizationMemberBase> nodes)
	{
		var childNodes = nodes[node.Id];
		foreach (var childNode in childNodes)
		{
			node.AddChildNode(childNode);
		}
	}

	// var 2: DFS with recursion:
	// 	private static void FillChildNodesTraverseDfs(OrganizationMemberBase node, ILookup<int?, OrganizationMemberBase> nodes)
	// {
	// 	var childNodes = nodes[node.Id];
	// 	foreach (var childNode in childNodes)
	// 	{
	// 		node.ChildNodes.Add(childNode);	
	// 	}
	// 	
	//
	// 	foreach (var childNode in node.ChildNodes)
	// 	{
	// 		FillChildNodesTraverseDfs(childNode, nodes);
	// 	}
	// }


	private OrganizationMemberBase CreateEmployeeBlModel(OrganizationMemberReadDto dto)
	{
		return dto.OrganizationMemberType switch
		{
			1 => new Employee(_configuration) { Id = dto.Id, Name = dto.Name, ParentId = dto.ParentId, WorkStartDate = dto.WorkStartDate, BaseSalary = dto.BaseSalary },
			2 => new Manager(_configuration) { Id = dto.Id, Name = dto.Name, ParentId = dto.ParentId, WorkStartDate = dto.WorkStartDate, BaseSalary = dto.BaseSalary },
			3 => new Sales(_configuration) { Id = dto.Id, Name = dto.Name, ParentId = dto.ParentId, WorkStartDate = dto.WorkStartDate, BaseSalary = dto.BaseSalary },
			_ => throw new NotSupportedException("Unknown type")
		};
	}
}