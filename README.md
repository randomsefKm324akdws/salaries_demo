Description: 
This solution is based on "Three-tier architecture": 
1. presentation layer: "console_app" project;
2. business layer: "bl" project. Calculates salary (Uses Depth First Search algorithm to traverse organization members tree to calculate salaries for specific organization member or for all members). Business logic is partly covered with tests (see "bl.tests" project);
3. data layer: "da" project. Uses "InMemoryTestOrganizationMembersRepository" only for demonstration purposes.


How to run: start "console_app" project in your IDE and follow the instruction in console.


Additional things need to do for production use:
- create database with business logic constrains and replace "InMemoryTestOrganizationMembersRepository" with real database repository. DB table must contain columns for all fields from "OrganizationMemberReadDto.cs" class.
- set up logger
- more validators in business layer
- more tests
