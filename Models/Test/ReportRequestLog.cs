namespace BackendTestTask.Models.Test;

public class ReportRequestLog
{
    public long Id { get; set; }
    
    public long UserId { get; set; }
    
    public Guid RequestGuid { get; set; }
    
    public DateTime RequestTime { get; set; }
    
    public DateTime PeriodFrom { get; set; }
    
    public DateTime PeriodTo { get; set; }
}

public class ReportRequest
{
    public long Id { get; set; }
    
    public long UserId { get; set; }
    
    public DateTime PeriodFrom { get; set; }
    
    public DateTime PeriodTo { get; set; }
}

public class ReportAnswer
{
    public Guid Query { get; set; }
    public int Percent { get; set; }
    
    public UserStatisticsAnswer? Result { get; set; }
}

public class UserStatisticsAnswer
{
    public long UserId { get; set; }
    public int CountSignIn { get; set; }
}