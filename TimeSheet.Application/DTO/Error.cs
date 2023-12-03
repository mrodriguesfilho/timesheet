namespace TimeSheet.Application.DTO;

public class Error
{
    public string Id { get; init; }
    public string Description { get; init; }

    
    public Error(string id, string description)
    {
        Id = id;
        Description = description;
    }
}