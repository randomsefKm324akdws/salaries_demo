using da.interfaces.IOrganizationMembersRepository;
using Moq;

namespace bl.tests;

public class Tests
{
	private IConfiguration _cfg = new TestConfiguration();

	[SetUp]
	public void Setup()
	{
		_cfg = new TestConfiguration();
	}


	[Test]
	public async Task Test_Manager_Has_1Level_2Child()
	{
		//arrange
		var mockRepo = new Mock<IOrganizationMembersRepository>();

		decimal baseSalary = 100;

		OrganizationMemberReadDto[] dtos =
		{
			new()
			{
				Id = 1,
				ParentId = null,
				Name = "manager 1",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 2,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 2,
				ParentId = 1,
				Name = "employee 2",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 3,
				ParentId = 1,
				Name = "employee 3",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 4,
				ParentId = null,
				Name = "employee 4 without manager",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			}
		};

		mockRepo
			.Setup(x => x.GetAsync())
			.ReturnsAsync(dtos.ToArray());

		var dateTime = new DateTime(2024, 2, 1);

		//expected salary calculation:
		var yearsWorked = 5;
		var manager1LongWorkSalary = baseSalary + yearsWorked * baseSalary * _cfg.ManagerLongWorkYearIncreasePercent;
		var employee2FullSalary = baseSalary + yearsWorked * baseSalary * _cfg.EmployeeLongWorkYearIncreasePercent;
		var employee3FullSalary = baseSalary + yearsWorked * baseSalary * _cfg.EmployeeLongWorkYearIncreasePercent;
		var manager1PositionBonus = (employee2FullSalary + employee3FullSalary) * _cfg.ManagerPositionBonusPercentOfFirstLevelChildSalaries;
		var manager1FullExpectedSalary = manager1LongWorkSalary + manager1PositionBonus;


		//act
		ISalariesService service = new SalariesService(mockRepo.Object, new TestConfiguration());
		var salary = await service.GetMonthlySalaryAsync(1, dateTime);

		//assert
		Assert.AreEqual(manager1FullExpectedSalary, salary);
	}

	[Test]
	public async Task Test_Manager_Has_2Level_3Child()
	{
		//arrange
		var mockRepo = new Mock<IOrganizationMembersRepository>();

		decimal baseSalary = 100;
		OrganizationMemberReadDto[] dtos =
		{
			new()
			{
				Id = 1,
				ParentId = null,
				Name = "manager 1",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 2,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 2,
				ParentId = 1,
				Name = "employee 2",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 3,
				ParentId = 1,
				Name = "employee 3",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 4,
				ParentId = null,
				Name = "employee 4 without manager",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 5,
				ParentId = 1,
				Name = "manager 5",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 2,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 6,
				ParentId = 5,
				Name = "employee 6",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			}
		};

		mockRepo
			.Setup(x => x.GetAsync())
			.ReturnsAsync(dtos.ToArray());


		var dateTime = new DateTime(2024, 2, 1);

		//expected salary calculation:
		var yearsWorked = 5;
		var employee2FullSalary = baseSalary + yearsWorked * baseSalary * _cfg.EmployeeLongWorkYearIncreasePercent;
		var employee3FullSalary = baseSalary + yearsWorked * baseSalary * _cfg.EmployeeLongWorkYearIncreasePercent;
		var manager5LongWorkSalary = baseSalary + yearsWorked * baseSalary * _cfg.ManagerLongWorkYearIncreasePercent;

		var employee6FullSalary = baseSalary + yearsWorked * baseSalary * _cfg.EmployeeLongWorkYearIncreasePercent;
		var manager5PositionBonus = employee6FullSalary * _cfg.ManagerPositionBonusPercentOfFirstLevelChildSalaries;
		var manager5FullSalary = manager5LongWorkSalary + manager5PositionBonus;

		var manager1PositionBonus = (employee2FullSalary + employee3FullSalary + manager5FullSalary) * _cfg.ManagerPositionBonusPercentOfFirstLevelChildSalaries;
		var manager1LongWorkSalary = baseSalary + yearsWorked * baseSalary * _cfg.ManagerLongWorkYearIncreasePercent;
		var manager1FullExpectedSalary = manager1LongWorkSalary + manager1PositionBonus;


		//act
		ISalariesService service = new SalariesService(mockRepo.Object, new TestConfiguration());
		var salary = await service.GetMonthlySalaryAsync(1, dateTime);

		//assert
		Assert.AreEqual(manager1FullExpectedSalary, salary);
	}

	[Test]
	public async Task Test_Manager_Has_2Level_1Child()
	{
		//arrange
		var mockRepo = new Mock<IOrganizationMembersRepository>();

		decimal baseSalary = 100;

		OrganizationMemberReadDto[] dtos =
		{
			new()
			{
				Id = 1,
				ParentId = null,
				Name = "manager 1",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 2,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 2,
				ParentId = 1,
				Name = "employee 2",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 3,
				ParentId = 1,
				Name = "employee 3",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 4,
				ParentId = null,
				Name = "employee 4 without manager",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 5,
				ParentId = 1,
				Name = "manager 5",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 2,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 6,
				ParentId = 5,
				Name = "employee 6",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			}
		};

		mockRepo
			.Setup(x => x.GetAsync())
			.ReturnsAsync(dtos.ToArray());

		var dateTime = new DateTime(2024, 2, 1);

		//expected salary calculation:
		var yearsWorked = 5;

		var manager5LongWorkSalary = baseSalary + yearsWorked * baseSalary * _cfg.ManagerLongWorkYearIncreasePercent;
		var employee6FullSalary = baseSalary + yearsWorked * baseSalary * _cfg.EmployeeLongWorkYearIncreasePercent;
		var manager5PositionBonus = employee6FullSalary * _cfg.ManagerPositionBonusPercentOfFirstLevelChildSalaries;
		var manager5FullSalary = manager5LongWorkSalary + manager5PositionBonus;

		//act
		ISalariesService service = new SalariesService(mockRepo.Object, new TestConfiguration());
		var salary = await service.GetMonthlySalaryAsync(5, dateTime);

		//assert
		Assert.AreEqual(manager5FullSalary, salary);
	}


	[Test]
	public async Task Test_Sales_Has_3Level()
	{
		//arrange
		var mockRepo = new Mock<IOrganizationMembersRepository>();


		decimal baseSalary = 100;
		OrganizationMemberReadDto[] dtos =
		{
			new()
			{
				Id = 1,
				ParentId = null,
				Name = "sales 1",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 3,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 2,
				ParentId = 1,
				Name = "employee 2",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 3,
				ParentId = 1,
				Name = "employee 3",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 4,
				ParentId = null,
				Name = "employee 4 without manager",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 5,
				ParentId = 1,
				Name = "manager 5",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 2,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 6,
				ParentId = 5,
				Name = "employee 6",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			}
		};

		mockRepo
			.Setup(x => x.GetAsync())
			.ReturnsAsync(dtos.ToArray());

		var dateTime = new DateTime(2024, 2, 1);

		//expected salary calculation:
		var yearsWorked = 5;
		var employee2FullSalary = baseSalary + yearsWorked * baseSalary * _cfg.EmployeeLongWorkYearIncreasePercent;
		var employee3FullSalary = baseSalary + yearsWorked * baseSalary * _cfg.EmployeeLongWorkYearIncreasePercent;
		var manager5LongWorkSalary = baseSalary + yearsWorked * baseSalary * _cfg.ManagerLongWorkYearIncreasePercent;

		var employee6FullSalary = baseSalary + yearsWorked * baseSalary * _cfg.EmployeeLongWorkYearIncreasePercent;
		var manager5PositionBonus = employee6FullSalary * _cfg.ManagerPositionBonusPercentOfFirstLevelChildSalaries;
		var manager5FullSalary = manager5LongWorkSalary + manager5PositionBonus;

		var sales1PositionBonus = (employee2FullSalary + employee3FullSalary + manager5FullSalary + employee6FullSalary) * _cfg.SalesPositionBonusPercentOfAllLevelChildSalaries;
		var sales1LongWorkSalary = baseSalary + yearsWorked * baseSalary * _cfg.SalesLongWorkYearIncreasePercent;
		var sales1FullExpectedSalary = sales1LongWorkSalary + sales1PositionBonus;

		//act
		ISalariesService service = new SalariesService(mockRepo.Object, new TestConfiguration());
		var salary = await service.GetMonthlySalaryAsync(1, dateTime);

		//assert
		Assert.AreEqual(sales1FullExpectedSalary, salary);
	}

	[Test]
	public async Task Test_Employee()
	{
		//arrange
		var mockRepo = new Mock<IOrganizationMembersRepository>();

		decimal baseSalary = 100;
		OrganizationMemberReadDto[] dtos =
		{
			new()
			{
				Id = 1,
				ParentId = null,
				Name = "sales 1",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 3,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 2,
				ParentId = 1,
				Name = "employee 2",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 3,
				ParentId = 1,
				Name = "employee 3",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 4,
				ParentId = null,
				Name = "employee 4 without manager",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 5,
				ParentId = 1,
				Name = "manager 5",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 2,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 6,
				ParentId = 5,
				Name = "employee 6",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			}
		};

		mockRepo
			.Setup(x => x.GetAsync())
			.ReturnsAsync(dtos.ToArray());

		var dateTime = new DateTime(2024, 2, 1);

		//expected salary calculation:
		var yearsWorked = 5;

		var employee6FullSalary = baseSalary + yearsWorked * baseSalary * _cfg.EmployeeLongWorkYearIncreasePercent;

		//act
		ISalariesService service = new SalariesService(mockRepo.Object, new TestConfiguration());
		var salary = await service.GetMonthlySalaryAsync(6, dateTime);

		//assert
		Assert.AreEqual(employee6FullSalary, salary);
	}
	
	[Test]
	public async Task Test_EmployeeMaxSalary()
	{
		//arrange
		var mockRepo = new Mock<IOrganizationMembersRepository>();

		decimal baseSalary = 100;
		OrganizationMemberReadDto[] dtos =
		{
			new()
			{
				Id = 1,
				ParentId = null,
				Name = "sales 1",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 3,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 2,
				ParentId = 1,
				Name = "employee 2",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 3,
				ParentId = 1,
				Name = "employee 3",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 4,
				ParentId = null,
				Name = "employee 4 without manager",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 5,
				ParentId = 1,
				Name = "manager 5",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 2,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 6,
				ParentId = 5,
				Name = "employee 6",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			}
		};

		mockRepo
			.Setup(x => x.GetAsync())
			.ReturnsAsync(dtos.ToArray());

		var dateTime = new DateTime(2039, 2, 1);

		//expected salary calculation:
		var employee6FullSalary = baseSalary + baseSalary * _cfg.EmployeeMaxPossibleLongWorkIncreasePercent;

		//act
		ISalariesService service = new SalariesService(mockRepo.Object, new TestConfiguration());
		var salary = await service.GetMonthlySalaryAsync(6, dateTime);

		//assert
		Assert.AreEqual(employee6FullSalary, salary);
	}

	[Test]
	public async Task Test_Employee_b()
	{
		//arrange
		var mockRepo = new Mock<IOrganizationMembersRepository>();

		decimal baseSalary = 100;
		OrganizationMemberReadDto[] dtos =
		{
			new()
			{
				Id = 1,
				ParentId = null,
				Name = "sales 1",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 3,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 2,
				ParentId = 1,
				Name = "employee 2",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 3,
				ParentId = 1,
				Name = "employee 3",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 4,
				ParentId = null,
				Name = "employee 4 without manager",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 5,
				ParentId = 1,
				Name = "manager 5",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 2,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 6,
				ParentId = 5,
				Name = "employee 6",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			}
		};

		mockRepo
			.Setup(x => x.GetAsync())
			.ReturnsAsync(dtos.ToArray());

		var dateTime = new DateTime(2024, 2, 1);

		//expected salary calculation:
		var yearsWorked = 5;

		var employee4FullSalary = baseSalary + yearsWorked * baseSalary * _cfg.EmployeeLongWorkYearIncreasePercent;

		//act
		ISalariesService service = new SalariesService(mockRepo.Object, new TestConfiguration());
		var salary = await service.GetMonthlySalaryAsync(4, dateTime);

		//assert
		Assert.AreEqual(employee4FullSalary, salary);
	}


	[Test]
	public async Task Test_AllOrganizationMembersSalary()
	{
		//arrange
		var mockRepo = new Mock<IOrganizationMembersRepository>();


		decimal baseSalary = 100;
		OrganizationMemberReadDto[] dtos =
		{
			new()
			{
				Id = 1,
				ParentId = null,
				Name = "sales 1",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 3,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 2,
				ParentId = 1,
				Name = "employee 2",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 3,
				ParentId = 1,
				Name = "employee 3",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 4,
				ParentId = null,
				Name = "employee 4 without manager",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 5,
				ParentId = 1,
				Name = "manager 5",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 2,
				BaseSalary = baseSalary
			},
			new()
			{
				Id = 6,
				ParentId = 5,
				Name = "employee 6",
				WorkStartDate = new DateTime(2019, 1, 1),
				OrganizationMemberType = 1,
				BaseSalary = baseSalary
			}
		};

		mockRepo
			.Setup(x => x.GetAsync())
			.ReturnsAsync(dtos.ToArray());

		var dateTime = new DateTime(2024, 2, 1);

		//expected salary calculation:
		var yearsWorked = 5;
		var employee2FullSalary = baseSalary + yearsWorked * baseSalary * _cfg.EmployeeLongWorkYearIncreasePercent;
		var employee3FullSalary = baseSalary + yearsWorked * baseSalary * _cfg.EmployeeLongWorkYearIncreasePercent;
		var employee4FullSalary = baseSalary + yearsWorked * baseSalary * _cfg.EmployeeLongWorkYearIncreasePercent;
		var employee6FullSalary = baseSalary + yearsWorked * baseSalary * _cfg.EmployeeLongWorkYearIncreasePercent;

		var manager5LongWorkSalary = baseSalary + yearsWorked * baseSalary * _cfg.ManagerLongWorkYearIncreasePercent;
		var manager5PositionBonus = employee6FullSalary * _cfg.ManagerPositionBonusPercentOfFirstLevelChildSalaries;
		var manager5FullSalary = manager5LongWorkSalary + manager5PositionBonus;

		var sales1PositionBonus = (employee2FullSalary + employee3FullSalary + manager5FullSalary + employee6FullSalary) * _cfg.SalesPositionBonusPercentOfAllLevelChildSalaries;
		var sales1LongWorkSalary = baseSalary + yearsWorked * baseSalary * _cfg.SalesLongWorkYearIncreasePercent;
		var sales1FullSalary = sales1LongWorkSalary + sales1PositionBonus;

		var allMembersExpectedSalary = sales1FullSalary + employee2FullSalary + employee3FullSalary + employee4FullSalary + manager5FullSalary + employee6FullSalary;

		//act
		ISalariesService service = new SalariesService(mockRepo.Object, new TestConfiguration());
		var salary = await service.GetMonthlySalaryForAllOrganizationMembersAsync(dateTime);

		//assert
		Assert.AreEqual(allMembersExpectedSalary, salary);
	}


	[Test]
	public async Task Test_ManyLevelsInOrganizationTree()
	{
		static OrganizationMemberReadDto[] CreateTestDataManyEmployeesForBigTree()
		{
			var dtos = new List<OrganizationMemberReadDto>();

			dtos.Add(
				new OrganizationMemberReadDto
				{
					Id = 1,
					ParentId = null,
					Name = "sales 1",
					WorkStartDate = new DateTime(2001, 1, 1),
					OrganizationMemberType = 3,
					BaseSalary = 100
				});

			var id = 1;
			var parentId = 1;
			var treeLevel = 1;

			CreateTreeRecursive(ref treeLevel, ref id, ref parentId, dtos);

			return dtos.ToArray();
		}

		static void CreateTreeRecursive(ref int treeLevel, ref int id, ref int parentId, List<OrganizationMemberReadDto> dtos)
		{
			for (var i = 0; i < 100; i++)
			{
				dtos.Add(
					new OrganizationMemberReadDto
					{
						Id = ++id,
						ParentId = parentId,
						Name = "sales " + id,
						WorkStartDate = new DateTime(2001, 1, 1),
						OrganizationMemberType = 3,
						BaseSalary = 100
					});
			}

			if (treeLevel < 15)
			{
				++treeLevel;
				parentId = id;
				CreateTreeRecursive(ref treeLevel, ref id, ref parentId, dtos);
			}
		}

		//arrange
		var mockRepo = new Mock<IOrganizationMembersRepository>();

		var dtos = CreateTestDataManyEmployeesForBigTree();

		mockRepo
			.Setup(x => x.GetAsync())
			.ReturnsAsync(dtos.ToArray());


		//act
		ISalariesService service = new SalariesService(mockRepo.Object, new TestConfiguration());

		var dateTime = new DateTime(2020, 1, 1);
		var salary = await service.GetMonthlySalaryAsync(1, dateTime);

		//assert
		Assert.IsTrue(salary > 0);
		Assert.Pass();
	}
}