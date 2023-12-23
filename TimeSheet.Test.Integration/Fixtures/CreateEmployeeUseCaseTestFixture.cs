using TimeSheet.Application.DTO;

namespace TimeSheet.Test.Integration.Fixtures;

[CollectionDefinition(nameof(CreateEmployeeUseCaseTestFixture))]
public class CreateEmployeeUseCaseTestFixtureCollection
    : ICollectionFixture<CreateEmployeeUseCaseTestFixture>
{}

public class CreateEmployeeUseCaseTestFixture : BaseFixture
{
    private const string governmentIdentification = "111-111-111-22";
    private const string employeeName = "João Pedro";
    
    public static CreateEmployeeInput GetValidInput()
    {
        var createEmployeeInput = new CreateEmployeeInput(employeeName, governmentIdentification);
        return createEmployeeInput;
    }
}