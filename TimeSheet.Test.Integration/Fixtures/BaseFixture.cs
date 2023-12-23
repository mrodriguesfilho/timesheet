using TimeSheet.Test.Integration.InMemory;

namespace TimeSheet.Test.Integration.Fixtures;

public class BaseFixture
{
    private EmployeeInMemoryRepository _employeeInMemoryRepository;

    public BaseFixture() => _employeeInMemoryRepository = new();

    public EmployeeInMemoryRepository CreateEmployeeRepository() => _employeeInMemoryRepository;
}