namespace MachineLearningDomain.Base;

public class BaseModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? created_User { get; set; }
    public DateTime created_Dt { get; set; }
    public string? updated_User { get; set; }
    public DateTime updated_Dt { get; set; }
    public bool? is_deleted { get; set; }
}

public class BaseResponse
{
    public Guid Id { get; set; }
    public string? updated_User { get; set; }
    public DateTime updated_Dt { get; set; }
    public bool? is_deleted { get; set; }
}
