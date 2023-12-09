using System.Data;
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
        var project = new Project(projectModel.Name, projectModel.Ticker);
        project.SetId(projectModel.Id);
        return project;
    }

    public static Project Map(IDataRecord dataRecord)
    {
        var projectId = Convert.ToInt64(dataRecord["id_project"]);
        var projectName = Convert.ToString(dataRecord["nm_project"]);
        var projectTicker = Convert.ToString(dataRecord["cd_project_ticker"]);
        var project = new Project(projectId, projectName, projectTicker);
        return project;
    }
}