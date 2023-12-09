using System.Data;
using TimeSheet.Database.Models;
using TimeSheet.Domain.Entities;

namespace TimeSheet.Database.ModelMappers;

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
        var employee = new Employee(employeeModel.Name, employeeModel.GovernmentIdentification, new TimeSheetEntity(new Dictionary<DateTime, List<TimeSheetEntry>>()), projects);
        employee.SetId(employeeModel.Id);
        return employee;
    }

    public static Employee Map(IDataRecord dataRecord)
    {
        var employeeId = Convert.ToInt64(dataRecord["id_employee"]);
        var employeeName = Convert.ToString(dataRecord["nm_employee"]);
        var employeeGovernmentCode = Convert.ToString(dataRecord["cd_employee_government"]);
        
        var employee = new Employee(employeeName, employeeGovernmentCode, new TimeSheetEntity(new Dictionary<DateTime, List<TimeSheetEntry>>()), new List<Project>());
        employee.SetId(employeeId);
        return employee;
    }
}