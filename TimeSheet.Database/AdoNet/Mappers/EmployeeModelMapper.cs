using System.Data;
using TimeSheet.Database.AdoNet.Models;
using TimeSheet.Domain.Entities;

namespace TimeSheet.Database.AdoNet.Mappers;

public static class EmployeeModelMapper
{
    public static EmployeeModel Map(Employee employee)
    {
        var employeeModel = new EmployeeModel();
        employeeModel.Id = employee.Id;
        employeeModel.Name = employee.Name;
        employeeModel.GovernmentIdentification = employee.GovernmentIdentification;
        employeeModel.AllocatedProjects = employee.AllocatedProjects.ConvertAll(x => ProjectModelMapper.Map(x));
        return employeeModel;
    }

    public static Employee Map(EmployeeModel employeeModel)
    {
        var projects = employeeModel.AllocatedProjects?.ToList().ConvertAll(x => ProjectModelMapper.Map(x)) ?? new List<Project>();
        var employee = new Employee(employeeModel.Id, employeeModel.Name, employeeModel.GovernmentIdentification, new TimeSheetEntity(new Dictionary<DateTime, List<TimeSheetEntry>>()), projects);
        return employee;
    }

    public static EmployeeModel Map(IDataRecord dataRecord)
    {
        var employeeModel = new EmployeeModel();
        employeeModel.Id = Convert.ToInt64(dataRecord["id_employee"]);
        employeeModel.Name = Convert.ToString(dataRecord["nm_employee"]);
        employeeModel.GovernmentIdentification = Convert.ToString(dataRecord["cd_employee_government"]);
        return employeeModel;
    }
}