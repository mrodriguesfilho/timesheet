using System.Data;
using TimeSheet.Database.Extensions;
using TimeSheet.Database.Models;
using TimeSheet.Domain.Entities;

namespace TimeSheet.Database.ModelMappers;

public static class ProjectModelMapper
{
    public static ProjectModel Map(Project project)
    {
        var projectModel = new ProjectModel();
        projectModel.Id = project.Id;
        projectModel.Name = project.Name;
        projectModel.Ticker = project.Ticker;
        return projectModel;
    }

    public static Project Map(ProjectModel projectModel)
    {
        var project = Project.CreateExistingProject(projectModel.Id, projectModel.Name, projectModel.Ticker);
        return project;
    }

    public static ProjectModel Map(IDataRecord dataRecord)
    {
        var projectModel = new ProjectModel();
        projectModel.Id = Convert.ToInt64(dataRecord["id_project"]);
        projectModel.Name = Convert.ToString(dataRecord["nm_project"]);
        projectModel.Ticker = Convert.ToString(dataRecord["cd_project_ticker"]);

        return projectModel;
    }
    
    public static ProjectModel MapWithAllocation(IDataRecord dataRecord)
    {
        var projectModel = Map(dataRecord);
        projectModel.AllocationDate = Convert.ToDateTime(dataRecord["dt_allocated"]);
        projectModel.DeallocationDate = dataRecord["dt_deallocated"] == DBNull.Value ? null : Convert.ToDateTime(dataRecord["dt_dealloacted"]);

        return projectModel;
    }
}